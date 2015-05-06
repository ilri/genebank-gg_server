using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;
using System.Globalization;
using System.Diagnostics;
using System.Threading;

namespace GrinGlobal.Search.Engine {

	public class BPlusTree : IDisposable {

		private const int ENCODING_BYTE_SIZE = 100;

		const int BUFFER_SIZE = 1024 * 1024;

		private long _rootNodePointerOffset;
		private long _abandonedNodesPointerOffset;

		// The Interlocked class prefers a long here...
		private long _currentSearchCount;
		private long _currentWriteCount;

		private object _lockForFileAccess = new object();
		internal object LockForFileAccess {
			get {
				return _lockForFileAccess;
			}
		}

		public int NodeCacheSize;
		public int KeywordCacheSize;

		internal delegate object ExclusiveLockCallback(object[] arguments);

		internal Dictionary<long, BPlusTreeNode> NodeCache { get; private set; }
		internal Dictionary<string, List<KeywordMatch<TKeyManager, TValue>>> KeywordCache { get; private set; }


		private BPlusTreeNode _root;
		internal BPlusTreeNode Root {
			get {
				return _root;
			}
			set {
				if (value == null) {
					readRootNode();
				} else {
					_root = value;
				}
			}
		}

		/// <summary>
		/// Returns the total number of leaves in the tree.  Note this causes I/O to the tree's file, so do not call it frequently.
		/// </summary>
		public long TotalLeafCount {
			get {
				// since a b+ tree is always balanced by definition,
				// we can use calculations to determine the actual # of leaves.
				BPlusTreeNode node = this.Root;
				int depth = 0;
				while (!node.IsLeaf) {
					node = BPlusTreeNode.Read(this, node.ChildrenByteOffsets[0], true);
					depth++;
				}

				// now, depth contains the # of jumps required to get from root to leaf.
				long ret = (long)Math.Pow((double)this.Fanout, (double)depth);
				return ret;
			}
		}

		/// <summary>
		/// Returns the total number of nodes in the tree.  Note this causes I/O to the tree's file, so do not call it frequently.
		/// </summary>
		public long TotalNodeCount {
			get {
				// since a b+ tree is always balanced by definition,
				// we can use calculations to determine the actual # of nodes.
				BPlusTreeNode node = this.Root;
				int count = 1;
				long ret = 1;
				while (!node.IsLeaf) {
					node = BPlusTreeNode.Read(this, node.ChildrenByteOffsets[0], true);
					// each traversal down results in adding Fanout^count nodes represented by the tree
					ret += (long)Math.Pow((double)this.Fanout, (double)count);
					count++;
				}

				return ret;
			}
		}

		internal bool IsRoot(BPlusTreeNode node) {
			return _root.FileOffset == node.FileOffset;
		}

		private void fillNodeCacheForBulkInsert() {
			NodeCache.Clear();
			List<BPlusTreeNode> rightPath = GetRightmostNodePath(false);
			foreach (BPlusTreeNode node in rightPath) {
				if (NodeCache.Count >= NodeCacheSize) {
					// quit as soon as we hit our nodecachesize
					break;
				} else {
					NodeCache.Add(node.FileOffset, node);
				}
			}
		}

		private void fillNodeCacheBreadthFirst() {

			// wipe any existing cached nodes
			NodeCache.Clear();

			foreach (BPlusTreeNode node in TraverseBreadthFirst()) {
				if (NodeCache.Count >= NodeCacheSize) {
					return;
				} else {
					NodeCache[node.FileOffset] = node;
				}
			}
		}

		private void initCaches(bool fillNodeCache) {
			NodeCache = new Dictionary<long,BPlusTreeNode>();
			KeywordCache = new Dictionary<string, List<KeywordMatch>>();

			if (fillNodeCache) {
				// precache the given amount of nodes (skipping Root, as it's always cached)
				// to do this, we do a breadth-first search of the tree until we hit our cache limit.

				fillNodeCacheBreadthFirst();
			}

		}

		// TODO: Is the following correct?  we allow 3 abandoned nodes to be available, no more.  This is because the most complex operation involves 3 nodes at any one time.
		private const int MAX_ABANDONED_NODES = 3;
		private BPlusAbandonedNode[] _abandonedNodes = new BPlusAbandonedNode[MAX_ABANDONED_NODES];

		internal BPlusAbandonedNode GetNextAvailableNodeLocation(BPlusTreeNode newNode) {

			BPlusAbandonedNode found = null;


			// default to tacking onto the end of the file
			int minSize = newNode.CurrentSize;
			foreach (BPlusAbandonedNode node in _abandonedNodes) {
				if (node.ByteCount >= minSize) {
					found = new BPlusAbandonedNode { ByteCount = node.ByteCount, FileOffset = node.FileOffset };
					// we're allowing this abandoned node to be reclaimed.
					// mark the ByteCount as 0 so the FileOffset isn't accidentally reused later.
					// note the position in the array can be reused, just not the FileOffset.
					node.ByteCount = 0;
					node.FileOffset = 0;
					// note we write out to disk everytime a node is reclaimed
					writeAbandonedNodes(false);

					break;
				}
			}
			if (found == null) {
				found = new BPlusAbandonedNode { ByteCount = minSize, FileOffset = _writer.BaseStream.Length };
			}

			Log((found.FileOffset == _writer.BaseStream.Length ? "(from end of file) " : "****** REUSED NODE *****    ") + "Next available node is @ byteoffset=" + found.FileOffset + ", size=" + found.ByteCount);


			return found;
		}

		internal void AbandonNode(BPlusTreeNode node) {

			Log(("abandoning node @ " + node.FileOffset + " -> ").PadRight(50) + node.ToString());

			// first, yank it from the cache as this node is being relocated (which means its key, FileOffset, will change
			// and the cached one points to a now non-existant node)
			NodeCache.Remove(node.FileOffset);

			bool foundAbandonedNode = false;

			for(int i=0;i<_abandonedNodes.Length;i++){
				BPlusAbandonedNode availableNode = _abandonedNodes[i];
				if (availableNode.ByteCount == 0) {
					Log("Added node to abandoned list -> ".PadRight(50) + node.ToString());
					availableNode.ByteCount = node.ChunkSize;
					availableNode.FileOffset = node.FileOffset;
					// note we write out to disk everytime a node is abandoned
					writeAbandonedNodes(false);
					foundAbandonedNode = true;

					break;
				}
			}

			// also zero-fill the node so if it's accidentally read in again we know it
#if DEBUG
			node.ZeroFill();
#endif

			// NOTE: if all our abandoned nodes are full (or we're not tracking abandoned nodes), 
			// we simply ignore the node they gave us.  that chunk of the file is essentially unusuable forever (will perpetually increase over time)
			LogIf(!foundAbandonedNode && MAX_ABANDONED_NODES > 0,
					"#########################################################          We're abandoning a node, but there is no room to store the offset to it!  Will result in unusable space in the file. Offset=" + node.FileOffset);
		}


		private int _averageKeywordSize;
		/// <summary>
		/// Size of average keyword to assume when creating the B+ tree.  Default is 20 characters (actual number of bytes per character is driven by the Encoding property).  Higher value yields potentially fewer node relocations but larger nodes in general.  Lower value yields potentially more node relocations but smaller nodes in general.
		/// </summary>
		public int AverageKeywordSize { 
			get { 
				return _averageKeywordSize; 
			} 
		}

		private Encoding _encoding;
		/// <summary>
		/// Type of encoding to use when writing text to the B+ tree file.  Default is UTF8.
		/// </summary>
		public Encoding Encoding {
			get {
				return _encoding;
			}
		}


		private string _treeFile;

		BinaryReader _reader;
		internal BinaryReader Reader {
			get {
				return _reader;
			}
		}
		BinaryWriter _writer;
		internal BinaryWriter Writer {
			get {
				return _writer;
			}
		}
		private int _fanout;
		public int Fanout {
			get {
				return _fanout;
			}
			set {
				_fanout = value;
				// auto-set Median and MinimumChildren appropriately based on the fanout
				// (these are crucial when doing node splits)
				MinimumChildren = (int)Math.Ceiling((decimal)_fanout / 2.0M);
				Median = MinimumChildren - 1;
			}
		}

		/// <summary>
		/// This is always the floor of Fanout / 2
		/// </summary>
		internal int Median { get; private set; }

		/// <summary>
		/// This is always the ceiling of Fanout / 2
		/// </summary>
		internal int MinimumChildren { get; private set; }

		private BPlusTree(string fileName, bool readOnly) {

			// Loads a tree from the given file

			_treeFile = Toolkit.ResolveFilePath(fileName, true);

//			var stm = new MemoryStream();

			var stm = new BufferedStream(new FileStream(_treeFile, FileMode.Open, readOnly ? FileAccess.Read : FileAccess.ReadWrite), BUFFER_SIZE);

			// very first thing we do is read in the encoding.
			// this is always stored ASCII encoded and contains the fully qualified .net type name
			// and is the first 100 bytes of the file.
			byte[] encodingBytes = new byte[ENCODING_BYTE_SIZE];
			stm.Read(encodingBytes, 0, encodingBytes.Length);
			string encodingString = ASCIIEncoding.ASCII.GetString(encodingBytes).Replace("\0","").Trim();

			// ok, we've read in the encoding class.  load it and apply to our reader/writer objects
			_encoding = (Encoding)Toolkit.LoadClass(Type.GetType(encodingString));

			_reader = new BinaryReader(stm, _encoding);
			if (!readOnly) {
				_writer = new BinaryWriter(stm, _encoding);
			}

			// read in the fanout size
			Fanout = _reader.ReadInt32();

			// read in average keyword size
			_averageKeywordSize = _reader.ReadInt32();

			// read in cache sizes
			NodeCacheSize = _reader.ReadInt32();
			KeywordCacheSize = _reader.ReadInt32();

			// read in abandoned node info
			// (this also initializes the _rootNodePointerOffset variable)
			readAbandonedNodes();

			// create our caches (so readRootNode doesn't bomb as it expects a valid object for NodeCache)
			initCaches(false);

			// read in the root node
			readRootNode();

			// and initialize our caches
			initCaches(true);



		}

		private BPlusTree(string fileName, int fanout, Encoding encoding, int averageKeywordSize, int nodeCacheSize, int keywordCacheSize) {
			
			// this is a special case -- we're creating a new b+ tree file.

			Fanout = fanout;

			_root = BPlusTreeNode.CreateNode(this);
			_encoding = encoding;
			if (_encoding == null) {
				// default to encoding if it wasn't given
				_encoding = UTF8Encoding.UTF8;
			}
			_averageKeywordSize = averageKeywordSize;
//			NodeCacheSize = nodeCacheSize == 0 ? ;

			string treeFile = Toolkit.ResolveFilePath(fileName, true);

//			var stm = new MemoryStream();

			var stm = new BufferedStream(new FileStream(treeFile, FileMode.CreateNew, FileAccess.ReadWrite), BUFFER_SIZE);

			// very first thing we do is write bytes to store the fully qualified type name of the encoding
			// (so we can load it later)  We always write this one string in ASCII, even if the rest of the file is a different encoding.
			byte[] encodingBytes = new byte[ENCODING_BYTE_SIZE];
			byte[] fromFile = ASCIIEncoding.ASCII.GetBytes(_encoding.GetType().FullName);
			// since the GetBytes above doesn't return exactly 100 bytes, we copy it into our 100 byte array so we can just blindly write it 
			// This makes reading the encoding later easier since it's not variable length and we don't know the encoding up front
			Array.Copy(fromFile, encodingBytes, fromFile.Length);

			stm.Write(encodingBytes, 0, ENCODING_BYTE_SIZE);


			_reader = new BinaryReader(stm, this.Encoding);
			_writer = new BinaryWriter(stm, this.Encoding);



			// write the fanout size 
			_writer.Write(Fanout);

			// write average keyword size
			_writer.Write(_averageKeywordSize);

			// write out cache sizes
			_writer.Write(nodeCacheSize);
			_writer.Write(keywordCacheSize);

			// set our cache sizes for when they're refreshed
			NodeCacheSize = nodeCacheSize;
			KeywordCacheSize = keywordCacheSize;

			NodeCache = new Dictionary<long, BPlusTreeNode>();
			KeywordCache = new Dictionary<string, List<KeywordMatch>>();


			// write the initial abandonednode info
			// (this also initializes the _rootNodePointerOffset)
			writeAbandonedNodes(true);

			// we need to add the size of the root node pointer itself to the offset value
			WriteRootNodeOffset(_rootNodePointerOffset + sizeof(long));

			// init caches just so the Write() below doesn't fail
			initCaches(false);

			// and the initial root node.
			_root.Write(null);

			// initialize our caches
			initCaches(true);


		}

		/// <summary>
		/// Creates a new B+ tree file with the given initial values.  Defaults NodeCacheSize to same as Fanout (essentially meaning Root + 1st level of nodes are cached).
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fanout"></param>
		/// <param name="encoding"></param>
		/// <param name="averageKeywordSize"></param>
		/// <param name="keywordCacheSize"></param>
		/// <returns></returns>
		public static BPlusTree Create(string fileName, int fanout, Encoding encoding, int averageKeywordSize, int keywordCacheSize) {
			return new BPlusTree(fileName, fanout, encoding, averageKeywordSize, fanout + 1, keywordCacheSize);
		}


		/// <summary>
		/// Creates a new B+ tree file with the given initial values.  Defaults NodeCacheSize to same as Fanout (essentially meaning Root + 1st level of nodes are cached).  Defaults KeywordCacheSize to 1000.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fanout"></param>
		/// <param name="encoding"></param>
		/// <param name="averageKeywordSize"></param>
		/// <returns></returns>
		public static BPlusTree Create(string fileName, int fanout, Encoding encoding, int averageKeywordSize) {
			return new BPlusTree(fileName, fanout, encoding, averageKeywordSize, fanout + 1, 1000);
		}

		/// <summary>
		/// Creates a new B+ tree file with the given initial values. Defaults AverageKeywordSize to 20.  Defaults NodeCacheSize to same as Fanout (essentially meaning Root + 1st level of nodes are cached).  Defaults KeywordCacheSize to 1000.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fanout"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static BPlusTree Create(string fileName, int fanout, Encoding encoding) {
			return new BPlusTree(fileName, fanout, encoding, 20, fanout + 1, 1000);
		}

		/// <summary>
		/// Creates a new B+ tree file with the given initial values. Defaults Encoding to UTF8.  Defaults NodeCacheSize to same as Fanout (essentially meaning Root + 1st level of nodes are cached).  Defaults KeywordCacheSize to 1000.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fanout"></param>
		/// <param name="averageKeywordSize"></param>
		/// <returns></returns>
		public static BPlusTree Create(string fileName, int fanout, int averageKeywordSize) {
			return new BPlusTree(fileName, fanout, UTF8Encoding.UTF8, averageKeywordSize, fanout + 1, 1000);
		}

		/// <summary>
		/// Creates a new B+ tree file with the given initial values. Defaults Encoding to UTF8.  Defaults AverageKeywordSize to 20.  Defaults NodeCacheSize to same as Fanout (essentially meaning Root + 1st level of nodes are cached).  Defaults KeywordCacheSize to 1000.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fanout"></param>
		/// <returns></returns>
		public static BPlusTree Create(string fileName, int fanout) {
			return new BPlusTree(fileName, fanout, UTF8Encoding.UTF8, 20, fanout + 1, 1000);
		}


		public static BPlusTree Load(string fileName, bool readOnly) {
			return new BPlusTree(fileName, readOnly);
		}


		private void writeAbandonedNodes(bool assignNodePointerToCurrentPosition) {
			if (assignNodePointerToCurrentPosition) {
				_abandonedNodesPointerOffset = _writer.BaseStream.Position;
			} else {
				_writer.BaseStream.Position = _abandonedNodesPointerOffset;
			}
			for (int i=0; i < MAX_ABANDONED_NODES; i++) {
				if (_abandonedNodes[i] == null) {
					_abandonedNodes[i] = new BPlusAbandonedNode();
				}
				_abandonedNodes[i].Write(_writer);
			}
			_rootNodePointerOffset = _writer.BaseStream.Position;
		}

		private void readAbandonedNodes() {
			for (int i=0; i < MAX_ABANDONED_NODES; i++) {
				_abandonedNodes[i] = BPlusAbandonedNode.Read(_reader);
			}
			// root node pointer ALWAYS follows our abandoned node array :)
			_rootNodePointerOffset = _reader.BaseStream.Position;
		}

		private void readRootNode() {
			// first 4 bytes of file is the fanout size.
			// then comes the root node offset...

			_reader.BaseStream.Position = _rootNodePointerOffset;
			long rootNodeOffset = _reader.ReadInt64();

			// read in the root node
			_root = BPlusTreeNode.Read(this, rootNodeOffset, true);
		}

			
		internal void WriteRootNodeOffset(long rootNodeOffset) {

			Log("Updating root node pointer @ " + _rootNodePointerOffset + " in file to " + rootNodeOffset);

			_writer.BaseStream.Position = _rootNodePointerOffset;

			// write pointer to the initial root node
			_writer.Write(rootNodeOffset);
		}

		private string generateUniqueKeywordCacheKey(string keyword, KeywordMatchMode matchMode, bool ignoreCase) {
			StringBuilder sb = new StringBuilder(keyword);
			sb.Append("|")
				.Append(((int)matchMode).ToString())
				.Append("|")
				.Append((ignoreCase ? "Y" : "N"));
			return sb.ToString();

		}


		/// <summary>
		/// If there is room in the cache, adds/updates the cache value for the given node
		/// </summary>
		/// <param name="node"></param>
		internal void UpdateNodeCache(BPlusTreeNode node) {
			if (NodeCache.Count >= NodeCacheSize) {
				// cache is full. ignore.
			} else {
				// we're either adding or updating it -- we don't care, Dictionary will handle that.
				NodeCache[node.FileOffset] = node;
			}
		}

		private void addKeywordToCache(string generatedKeyword, List<KeywordMatch> results) {

			// first we construct a unique keyword by concatenating the 3 variables
			// (a search for 'joe' with a matchmode of ExactMatch ignoring case could return different results
			//  than a search for 'joe' with a matchmode of Contains and being case-sensitive)


			if (KeywordCache.ContainsKey(generatedKeyword)) {
				// is already in cache. refresh it.
				KeywordCache[generatedKeyword] = results;
			} else {
				if (KeywordCache.Count >= KeywordCacheSize) {
					// cache is full. remove an item and add this one.
					// HACK: it's impossible to tell the the order keys were added in a Dictionary since it is unordered,
					// but we can grab the first one returned by the Keys collection.  we'll just remove it and hope for the best. :)
					string existingKey = null;
					foreach (string key in KeywordCache.Keys) {
						existingKey = key;
						break;
					}
					KeywordCache.Remove(existingKey);
				}

				// add the new keyword
				KeywordCache[generatedKeyword] = results;

			}
		}


//#if DEBUG
//        /// <summary>
//        /// NOTE: FOR DEBUGGING ONLY! DO NOT CALL THIS METHOD IN PRODUCTION! A Thread.Sleep is introduced within the critical section but just after the actual search has been performed to allow multithreaded troubleshooting.
//        /// </summary>
//        /// <param name="keyword"></param>
//        /// <param name="matchMode"></param>
//        /// <param name="ignoreCase"></param>
//        /// <param name="delayInMilliseconds"></param>
//        /// <returns></returns>
//        public List<KeywordMatch> Search(string keyword, KeywordMatchMode matchMode, bool ignoreCase, int delayInMilliseconds) {

//            List<KeywordMatch> results = new List<KeywordMatch>();
			
//            Toolkit.BlockThread(okayToSearch, 100);

//            try {
//                Interlocked.Increment(ref _currentSearchCount);
//                Toolkit.BlockThread(okayToSearch, 100);
//                _root.Search(keyword, matchMode, ignoreCase, results);
//                // Here's the only line that should be different than the 'real' production Search() method...
//                Thread.Sleep(delayInMilliseconds);
//            } finally {
//                Interlocked.Decrement(ref _currentSearchCount);
//            }
//            return results;
//        }
//#endif

		// for managing synchronization between multiple threads (note this is different from lock(), which essentially requires serialization of calls to the critical section)
		private delegate bool BlockThreadCallback();
		private void BlockThread(BlockThreadCallback callback, int maxWaitTime) {
			while (!callback()) {
				Thread.Sleep(maxWaitTime);
			}
		}

		/// <summary>
		/// Returns the data file offset of the given keyword.  If not found, returns -1
		/// </summary>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public List<KeywordMatch> Search(string keyword, KeywordMatchMode matchMode, bool ignoreCase) {


			// NOTE: This is a very _very_ important section of code when it comes to multithreading.
			//       We do NOT use a lock() statement here as that would essentially allow only 1 thread
			//       at a time to search our tree.  We want to serialize only the portion of code that accesses
			//       the file (which is a small portion) and not the entire search method.
			//
			//       We also want to completely serialize inserts -- meaning the thread writing to the tree
			//       is the only one with access during the entire Insert method.
			//
			//       To do this, we have to be very careful :)
			//
			//       We essentially need to be able to queue up calls to Search() while a call to Insert() is
			//       in progress.  However, multiple simultaneous calls to Search() can happen concurrently.
			//       This means we need to track the 




			List<KeywordMatch> results = null;

			// NOTE: the cache completely sidesteps all of the thread blocking and reference counting.
			//       since we're always pulling the data from RAM, there's no need to ensure concurrency.
			//       worst case scenario the user gets data that is outdated by less than the time it takes
			//       for an Insert() or Update() call to execute (very minimal).  Since those both remove the affected
			//       item(s) from the cache, subsequent calls to Search() would cause this method to actually hit the file
			//       meaning fresh data is retreived and placed in the cache.  So blocking the thread and doing reference counting
			//       when we never actually touch the file makes no sense -- so it is avoided during a cache hit.
			string generatedKeyword = generateUniqueKeywordCacheKey(keyword, matchMode, ignoreCase);

			bool incremented = false;

			if (!KeywordCache.TryGetValue(generatedKeyword, out results)) {

				// NOTE: we do NOT use the lock() mechanism here because we want to allow multiple Searches to execute concurrently
				//       on separate threads.  This means the critical section for a Search is not at this level, but only at
				//       the BPlusTreeNode.Read() level, where the file's Position pointer is altered.
				//       However, we completely serialize Insert() and Update() calls, so not only do their critical sections need
				//       to be mutually exclusive, they also must wait for existing Search() calls to finish.  This means we must
				//       track the Search() calls via a counter (so we know when all of them have completed).  This counter must
				//       be thread safe, so we use the Interlocked class to handle acidity of incrementing / decrementing / reading
				//       this counter for us.

				try {

					// we serialize access during an Insert() or Update().  So if an Insert() or Update() is taking place,
					// block our Search() until the Insert() or Update() has completed
					BlockThread(okayToSearch, 100);

					// we get here, the previous Insert() or Update() we were waiting for completed.
					// remember that we're in the middle of a search.  This will effectively prevent
					// another Insert() or Update() from starting in the middle of our search.
					Interlocked.Increment(ref _currentSearchCount);

					// remember we incremented the counter so we know if to decrement it later
					incremented = true;

					// Since a call to Insert() may have taken place since the last check (between BlockThread and Increment calls above)
					// we must check it again.  Essentially, we must block before and after incrementing our SearchCount variable to ensure
					// acidity of the action
					BlockThread(okayToSearch, 100);


					// Begin critical section

					// we get here, there are no Insert() or Update() calls taking place.  Go ahead and do the search.
					results = new List<KeywordMatch>();

					// does not exist in the keyword cache. search starting at the root
					_root.Search(keyword, matchMode, ignoreCase, results);

					// add it to the keyword cache for lookup later
					addKeywordToCache(generatedKeyword, results);


				} finally {
					if (incremented) {
						// End critical section
						// flag that we're done with this search
						Interlocked.Decrement(ref _currentSearchCount);
					} else {
						// we never made it to the critical section. do not decrement search count, as we never incremented it in the first place.
						// (an exception occurred before the call to Interlocked.Increment() took place)
					}
				}
			}

			return results;
		}

		private bool okayToSearch() {
			// NOTE: this is completely independent upon the # of searches currently executing.
			//       we rely on critical sections (via Interlocked class) to handle the nuances of
			//       concurrent searches.  However, we completely serialize writes (Insert() and Update())
			//       so if any writes are in progress, we should block until they're done
			return Interlocked.Read(ref _currentWriteCount) == 0;
		}

		private void removeFromKeywordCache(string keyword) {
			// invalidate the associated keywordcache
			// we can't change a collection while iterating through it, so we queue up the keys to remove first
			// then remove them outside the iteration
			List<string> keysToRemove = new List<string>();
			foreach (string key in KeywordCache.Keys) {
				if (key.Contains(keyword)) {
					keysToRemove.Add(key);
				}
			}
			foreach (string keyToRemove in keysToRemove) {
				if (!String.IsNullOrEmpty(keyToRemove)) {
					KeywordCache.Remove(keyToRemove);
				}
			}
		}


		internal object GetExclusiveLock(ExclusiveLockCallback callback, object[] arguments) {

			if (callback == null) {
				return null;
			}

			// We use the _lockMonitor here because we want to serialize Insert() calls.  So one Insert() cannot 
			// run concurrently with another Insert(), as ensured by using the lock() statement.
			lock (_lockForFileAccess) {
				// Begin critical section for writing
				try {

					// flag us as writing
					// if a second write request comes in on a different thread, it won't get past the lock() above.
					// However, Interlocked likes longs so we'll keep a count (which will always be 0 or 1)
					Interlocked.Increment(ref _currentWriteCount);

					// Now, by this point, we've ensured no other Insert() or Update() calls are taking place.
					// We must also wait for all existing Search() calls to complete. (recheck every 100 ms)
					BlockThread(okayToWrite, 100);


					return callback(arguments);


				} finally {

					// flag us as being done with the write
					Interlocked.Decrement(ref _currentWriteCount);

				}
				// End critical section for writing
			}

		}

		private object processUpdate(object[] arguments) {

			// args = [keyword, dataByteOffset]
			string keyword = (string)arguments[0];
			long dataByteOffset = (long)arguments[1];
			_root.Update(keyword, dataByteOffset, null);

			// this keyword may have been cached, so remove it (and all its matchmode/ignorecase variants) from the keywordcache so
			// invalid results are not returned on subsequent Search() calls.
			removeFromKeywordCache(keyword);

			return null;
		
		}

		public void Update(string keyword, long dataByteOffset) {

			// our implementation requires an exclusive lock of the tree to write anything

			GetExclusiveLock(processUpdate, new object[] {keyword, dataByteOffset});

		}

		private object processBulkInsert(object[] arguments) {

			if (arguments[0] is BinaryReader) {
				// insert the data into the tree from a binary reader
				bulkInsertData((BinaryReader)arguments[0]);
			} else {
				// insert the data into the tree
				bulkInsertData((SortedDictionary<string, long>)arguments[0]);
			}


			// make sure everything is written out
			_writer.Flush();

			initCaches(false);

			return null;

		}

		/// <summary>
		/// Note: Only call this on a completely new, empty tree.  An invalid tree will result if any nodes have been altered prior to a Bulk Insert.
		/// </summary>
		/// <param name="data"></param>
		public void BulkInsert(SortedDictionary<string, long> data) {

			GetExclusiveLock(processBulkInsert, new object[] { data });

		}

		/// <summary>
		/// Note: Only call this on a completely new, empty tree.  An invalid tree will result if any nodes have been altered prior to a Bulk Insert.  BinaryReader's stream should point at a source that is string/long pairs (in binary format with no separators).
		/// </summary>
		/// <param name="rdr"></param>
		public void BulkInsert(BinaryReader rdr) {

			GetExclusiveLock(processBulkInsert, new object[] { rdr });

		}

		private BPlusTreeNode getParentOfIndexNodeOnRightmostPath(long fileOffset) {
			List<BPlusTreeNode> path = GetRightmostNodePath(false);
			BPlusTreeNode parent = null;
			for (int i=1; i < path.Count; i++) {
				if (path[i].FileOffset == fileOffset) {
					// this is the target node. mark the one before it as the parent.
					parent = path[i - 1];
					break;
				}
			}
			return parent;
		}

		private BPlusTreeNode getRightmostLeafNodeInSubtree(BPlusTreeNode subtreeRoot) {
			BPlusTreeNode node = subtreeRoot;
			while (node != null && !node.IsLeaf) {
				// default to rightmost child
				long offset = node.ChildrenByteOffsets[node.KeywordCount];
				if (offset < 1) {
					// it may not exist (during a bulk load, for instance)
					// so the next one down the chain wins
					offset = node.ChildrenByteOffsets[node.KeywordCount - 1];
				}
				node = BPlusTreeNode.Read(this, offset, false);
			}
			return node;
		}

		private BPlusTreeNode getLeftmostLeafNodeInSubtree(BPlusTreeNode subtreeRoot) {
			BPlusTreeNode node = subtreeRoot;
			while (node != null && !node.IsLeaf) {
				node = BPlusTreeNode.Read(this, node.ChildrenByteOffsets[0], false);
			}
			return node;
		}

		private BPlusTreeNode bulkSplitAsNeeded(List<BPlusTreeNode> rightPath) {
			BPlusTreeNode newParent = null;
			BPlusTreeNode lastInPath = rightPath[rightPath.Count - 1];
			if (!lastInPath.IsFull) {
				// we should never ever EVER get here. bomb if we do, code elsewhere has a bug in it.
				throw new  InvalidOperationException("Bad bubbling logic in bulkSplitAsNeeded.... Caller should never had tried to bubble or node appended incorrectly on previous call.");
			} else {
				if (rightPath.Count == 1) {
					// splitting root...
					BPlusTreeNode prevRoot = _root;
					_root = BPlusTreeNode.ReplaceRoot(this, prevRoot, null, 0); // leaf.Keywords[leaf.KeywordCount - 1], leaf.FileOffset);
					_root.Keywords[0] = prevRoot.Keywords[this.Median];
					_root.ChildrenByteOffsets[0] = prevRoot.FileOffset;
					_root.KeywordCount = 0;  // NOTE this is a very special case. the SplitChild is assuming you're adding a keyword, and we aren't. hence the invalid keyword count being reported here.
					_root.Write(null);
					newParent = _root.SplitChild(0, prevRoot, null);



					// since we split @ the root, we should refresh our cache
					// as the most-accessed nodes will be Root and the new rightmost nodes.
					// this will result in far fewer reads during the course of the bulk insert.
					fillNodeCacheForBulkInsert();


				} else {
					// splitting non-root index node...
					BPlusTreeNode parent = rightPath[rightPath.Count - 2];
					if (parent.IsFull) {
						// bubble up until the parent isn't full (or we hit the root)
						var parentPath = new List<BPlusTreeNode>(rightPath);
						parentPath.RemoveAt(parentPath.Count - 1);
						parent = bulkSplitAsNeeded(parentPath);

					}
					// GetParent... method gets the latest version of right path
					BPlusTreeNode grandParent = getParentOfIndexNodeOnRightmostPath(parent.FileOffset);

					// first we write the parent to disk to ensure it has its proper spot
					parent.Write(grandParent);
					newParent = parent.SplitChild(parent.KeywordCount, lastInPath, grandParent);
				}
			}

			// newParent has already been written to disk (and therefore relocated if needed)
			// so callers to this method (including itself) can trust its FileOffset to be correct until another keyword is added
			// (which may cause a relocation)
			return newParent;
		}

		private void bulkSaveNodes(ref BPlusTreeNode parent, ref BPlusTreeNode prevLeaf, ref BPlusTreeNode leaf, bool isLastLeaf) {


			if (prevLeaf == null) {

				// edge case for very first leaf -- no siblings to worry about, no bubbling/splitting to worry about (makes that logic simpler)



				if (isLastLeaf){

					// edge edge case! entire data fit into first leaf.
					// no need to create a new root.
					leaf.Write(null);
					_root = leaf;
					WriteRootNodeOffset(leaf.FileOffset);

				} else {

					// write out the leaf, remember it as the previous leaf
					leaf.Write(null);
					prevLeaf = leaf;
					
					// we need to attach the first leaf differently when there will be multiple leaves
					_root.IsLeaf = false;
					_root.Keywords[0] = leaf.Keywords[leaf.KeywordCount - 1];
					_root.ChildrenByteOffsets[0] = leaf.FileOffset;
					_root.KeywordCount = 1;
					_root.Write(null);
					parent = _root;
				}

				fillNodeCacheForBulkInsert();

			} else {

				// need to save the current leaf node's data
				// (and the previous leaf node, since its sibling's offset (current leaf node) is changing

				if (prevLeaf != null) {

					// point current's left sibling @ previous
					leaf.LeftSiblingFileOffset = prevLeaf.FileOffset;
					leaf.Write(parent);

					// point previous's right sibling @ current
					prevLeaf.RightSiblingFileOffset = leaf.FileOffset;
					prevLeaf.Write(parent);

					// remove previous leaf from the cache, as we're not going to touch it anymore
					NodeCache.Remove(prevLeaf.FileOffset);

				}

				if (!parent.IsFull) {

					if (isLastLeaf) {
						// last leaf does NOT copy its keyword up on a non-full node -- it's literally the rightmost child in the entire tree
						// so this parent's last child pointer should point at it, but not contain the keyword or increment the keyword count.
						// yes, this was a really fun edge case to find. :(
						parent.ChildrenByteOffsets[parent.KeywordCount] = leaf.FileOffset;

					} else {
						// simply copy up the last keyword in the leaf (and the fileoffset to the leaf)
						parent.Keywords[parent.KeywordCount] = leaf.Keywords[leaf.KeywordCount - 1];
						parent.ChildrenByteOffsets[parent.KeywordCount] = leaf.FileOffset;
						parent.KeywordCount++;
					}

				} else {

					if (parent.ChildrenByteOffsets[parent.KeywordCount] < 1) {

						// since the parent might relocate when we write it out, we have
						// to pass its parent (so the childbyteoffset can be updated in its parent and we don't accidentally orphan this new parent)
						BPlusTreeNode grandParent = getParentOfIndexNodeOnRightmostPath(parent.FileOffset);
						parent.ChildrenByteOffsets[parent.KeywordCount] = leaf.FileOffset;
						parent.Write(grandParent);

					} else {

						List<BPlusTreeNode> rightmostPath = GetRightmostNodePath(false);

						// need to get a new parent
						parent = bulkSplitAsNeeded(rightmostPath);

						parent.Keywords[parent.KeywordCount] = prevLeaf.Keywords[prevLeaf.KeywordCount - 1];
						parent.ChildrenByteOffsets[parent.KeywordCount] = prevLeaf.FileOffset;
						parent.KeywordCount++;

						parent.Keywords[parent.KeywordCount] = leaf.Keywords[leaf.KeywordCount - 1];
						parent.ChildrenByteOffsets[parent.KeywordCount] = leaf.FileOffset;
						parent.KeywordCount++;

						// since the bulkSplitAsNeeded may possibly change the path (relocation) we re-get the grandparent and save
						BPlusTreeNode grandParent = getParentOfIndexNodeOnRightmostPath(parent.FileOffset);
						parent.Write(grandParent);
					
					}
				}


			}
			if (!isLastLeaf) {
				// remember the previous node
				prevLeaf = leaf;

				// make a brand new leaf node
				leaf = BPlusTreeNode.CreateNode(this);

				// make sure we're using the correct parent
				//						parentNode = GetRightmostIndexNode();
				leaf.Write(parent);
			}
		}

		/// <summary>
		/// Reader must be pointing at a stream which consists of a keyword (as a string) followed by a long of the file offset.  Binary format -- long takes exactly 8 bytes.
		/// </summary>
		/// <param name="stream"></param>
		private void bulkInsertData(BinaryReader rdr) {
			BPlusTreeNode prevLeaf = null;
			BPlusTreeNode leaf = BPlusTreeNode.CreateNode(this);
			BPlusTreeNode parent = this.Root;

			// on very first pass, we need to point root node @ first leaf (means we write a bogus, empty leaf node for the very first one)
			leaf.Write(null);
			parent.ChildrenByteOffsets[0] = leaf.FileOffset;

			int i=0;
			while (rdr.BaseStream.Position < rdr.BaseStream.Length) {
				string key =rdr.ReadString();
				long value = rdr.ReadInt64();


				int position = i % Fanout;
				if (position == 0 && i > 0) {
					// current leaf is done, save it and percolate changes as needed
					bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, false);

				}

				// fill current leaf with the data
				leaf.Keywords[position] = key;
				leaf.ChildrenByteOffsets[position] = value;
				leaf.KeywordCount++;

				i++;
			}

			// last leaf is done, save it and percolate changes as needed
			if (leaf.KeywordCount > 0) {
				bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, true);
			} else {
				// last leaf is empty (we wrote one too many)
				// abandon it.
				AbandonNode(leaf);
			}

			// make sure the last parent is written to file before we finish
			BPlusTreeNode grandParent = getParentOfIndexNodeOnRightmostPath(parent.FileOffset);
			parent.Write(grandParent);

		}

		private void bulkInsertData(SortedDictionary<string, long> data) {

			// ok, here's the overall flow:
			//   1. Create a new leaf node, fill it, write it, remember offset
			//   2. Create another new leaf node, fill it with all values except the first one, DO NOT WRITE IT YET
			//   3. Perform an insert with the 1 value we skipped in the second leaf

			BPlusTreeNode prevLeaf = null;
			BPlusTreeNode leaf = BPlusTreeNode.CreateNode(this);
			BPlusTreeNode parent = this.Root;
			string[] allKeys = data.Keys.ToArray();

			// on very first pass, we need to point root node @ first leaf (means we write a bogus, empty leaf node for the very first one)
			leaf.Write(null);
			parent.ChildrenByteOffsets[0] = leaf.FileOffset;

			for (int i=0; i < allKeys.Length; i++) {

				string key = allKeys[i];
				long value = data[key];

				int position = i % Fanout;
				if (position == 0 && i > 0) {
					// current leaf is done, save it and percolate changes as needed
					bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, false);

				}

				// fill current leaf with the data
				leaf.Keywords[position] = key;
				leaf.ChildrenByteOffsets[position] = value;
				leaf.KeywordCount++;
			}

			// last leaf is done, save it and percolate changes as needed
			if (leaf.KeywordCount > 0) {
				bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, true);
			} else {
				// last leaf is empty (we wrote one too many)
				// abandon it.
				AbandonNode(leaf);
			}

			// make sure the last parent is written to file before we finish
			BPlusTreeNode grandParent = getParentOfIndexNodeOnRightmostPath(parent.FileOffset);
			parent.Write(grandParent);

		}

		private void insertData(string keyword, long dataByteOffset) {
			if (!_root.IsFull) {
				// root is not full, recursively traverse down to appropriate leaf node and add it
				_root.Insert(keyword, dataByteOffset, null);
			} else {
				// root is full.  ReplaceRoot takes care of all the dirty work for us...
				_root = BPlusTreeNode.ReplaceRoot(this, _root, keyword, dataByteOffset);

			}

		}


		/// <summary>
		/// Inserts the given keyword with the given dataByteRange into the proper place into the B+ tree
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="dataByteRange"></param>
		public void Insert(string keyword, long dataByteOffset){

			// NOTE: the Root is a special case for insertion.

			// NOTE: we don't want any Search() or Insert() calls occurring while we're doing an Insert.
			//       so essentially we wait until pending requests are done then perform the Insert.


			// We use the _lockForWriting here because we want to serialize Insert() and Update() calls.  So one Insert() or Update() cannot 
			// run concurrently with another Insert() or Update(), as ensured by using the lock() statement.
			lock (_lockForFileAccess) {
				// Begin critical section for writing
				try {

					// flag us as writing
					// if a second write request comes in on a different thread, it won't get past the lock() above.
					// However, Search() does not use _lockForWriting, but instead does reference counting via calls to the Interlocked class (which is much faster)
					// Interlocked likes longs so we'll keep a count (which will always be 0 or 1)
					Interlocked.Increment(ref _currentWriteCount);

					// Now, by this point, we've ensured no other Insert() or Update() calls are taking place.
					// We must also wait for all existing Search() calls to complete. (recheck every 100 ms)
					Toolkit.BlockThread(okayToWrite, 100);

					// now that all the synchronization work is out of the way, perform the insert
					insertData(keyword, dataByteOffset);

					// this new data may affect results we've already cached.
					// so remove this keyword (and all its matchmode/case variants) from the keywordcache
					// so outdated results aren't returned on subsequent calls to Search()
					removeFromKeywordCache(keyword);


				} finally {

					// flag us as being done with the write
					Interlocked.Decrement(ref _currentWriteCount);

				}
				// End critical section for writing

			} // this releases the lock so a call to Update() or Insert() on another thread may enter its critical section
		}


//#if DEBUG
//        /// <summary>
//        /// NOTE: FOR DEBUGGING ONLY! DO NOT CALL THIS METHOD IN PRODUCTION! A Thread.Sleep is introduced after the start of the critical section but just before the actual insert is kicked off to allow for multithreaded troubleshooting
//        /// </summary>
//        /// <param name="keyword"></param>
//        /// <param name="dataByteOffset"></param>
//        /// <param name="delayInMilliseconds"></param>
//        public void Insert(string keyword, long dataByteOffset, int delayInMilliseconds) {

//            // NOTE:  Change this code to exactly match the Insert() code (but leave in the Thread.Sleep, obviously)

//            lock (_lockMonitor) {
//                try {
//                    Interlocked.Increment(ref _currentWriteCount);
//                    Toolkit.BlockThread(okayToWrite, 100);


//                    // Here's the only line that should be different than the 'real' production Insert() method...
//                    Thread.Sleep(delayInMilliseconds);

//                    insertData(keyword, dataByteOffset);

//                } finally {

//                    // flag us as being done with the write
//                    Interlocked.Decrement(ref _currentWriteCount);

//                }
//            }
//        }
//#endif

		private bool okayToWrite() {
			// NOTE: Writing requires the tree to be completely untouched -- no reads currently in progress
			return Interlocked.Read(ref _currentSearchCount) == 0;
		}

		internal void LogIf(bool expression, string text) {
//			Logger.LogTextIf(expression, text);
//			Debug.WriteLineIf(expression, text);
		}

		internal void Log(string text) {
//			Logger.LogText(text);
//			Debug.WriteLine(text);
		}

		/// <summary>
		/// Returns all index nodes starting at the root all the way down the right side of the tree.  Note only returns the index nodes -- excludes the rightmost leaf node.
		/// </summary>
		/// <returns></returns>
		internal List<BPlusTreeNode> GetRightmostNodePath(bool includeLeaf) {
			List<BPlusTreeNode> nodeList = new List<BPlusTreeNode>();
			BPlusTreeNode node = Root;
			while (node != null && (!node.IsLeaf || includeLeaf)) {
				nodeList.Add(node);
				node = BPlusTreeNode.Read(this, node.ChildrenByteOffsets[node.KeywordCount], false);
			}

			// we get here, previous node contains rightmost non-leaf node (aka index node)

			return nodeList;
		}

		internal BPlusTreeNode GetLeftmostLeafNode() {
			return getLeftmostLeafNodeInSubtree(this.Root);
		}

		internal IEnumerable<BPlusTreeNode> TraverseLeaves() {

			// since B+ trees store all values at the leaves and
			// contain pointers to sibling leaves,
			// we simply need to drill down to the leftmost node
			// and spin across the other leaves from there

			// first, drill down to leafmost leaf node
			BPlusTreeNode leaf = GetLeftmostLeafNode();

			while (leaf != null) {
				yield return leaf;
				leaf = BPlusTreeNode.Read(this, leaf.RightSiblingFileOffset, false);
			}

		}

		private IEnumerable<BPlusTreeNode> traverseBreadthFirst(BPlusTreeNode subtreeRoot) {

			if (subtreeRoot.IsLeaf){
				// leaves never have children -- we should get here only if we were incorrectly given a subtreeRoot as a leaf node
				yield return subtreeRoot;
			} else {
				// return all nodes at this level
				for (int i=0; i < subtreeRoot.KeywordCount+1; i++) {
					BPlusTreeNode node = BPlusTreeNode.Read(this, subtreeRoot.ChildrenByteOffsets[i], true);
					yield return node;
				}

				// return all nodes at level below this one
				for (int i=0; i < subtreeRoot.KeywordCount + 1; i++) {
					BPlusTreeNode node = BPlusTreeNode.Read(this, subtreeRoot.ChildrenByteOffsets[i], true);
					if (!node.IsLeaf) {
						foreach (BPlusTreeNode child in traverseBreadthFirst(node)) {
							yield return child;
						}
					}
				}
			}



		}

		internal IEnumerable<BPlusTreeNode> TraverseBreadthFirst() {

			// first return the root itself (special case)
			yield return Root;

			// start a subtree breadth first search at the root
			foreach (BPlusTreeNode node in traverseBreadthFirst(Root)) {
				yield return node;
			}

		}


		/// <summary>
		/// Returns all nodes that are invalid or point to invalid nodes.
		/// </summary>
		/// <param name="subtreeRoot"></param>
		/// <param name="inspectLeaves"></param>
		/// <returns></returns>
		internal IEnumerable<BPlusTreeNode> GetAllMalformedNodes(bool inspectLeaves) {

			foreach (BPlusTreeNode node in TraverseBreadthFirst()) {
				if (node.KeywordCount == 0) {
					yield return node;
				} else {
					if (node.IsLeaf) {
						if (!inspectLeaves) {
							// we're done, we were told to skip leaves (even if it's the root)
							break;
						} else {
							// make sure all keywords are non-empty
							for (int i=0; i < node.KeywordCount; i++) {
								// make sure all keywords are filled in
								if (String.IsNullOrEmpty(node.Keywords[i])) {
									// invalid keyword
									yield return node;
								}
							}
						}
					} else {
						for (int i=0; i < node.KeywordCount; i++) {
							// make sure all keywords are filled in
							if (String.IsNullOrEmpty(node.Keywords[i])) {
								// invalid keyword
								yield return node;
							}
							// check valid child pointers
							if (node.ChildrenByteOffsets[i] == 0) {
								// invalid child pointer. malformed.
								yield return node;
							}
						}
						// check rightmost child pointer
						if (node.ChildrenByteOffsets[node.KeywordCount] == 0) {
							// invalid rightmost child pointer. malformed.
							yield return node;
						}
					}
				}
			}
		}

		internal IEnumerable<string> TraverseKeywords() {
			// since B+ trees always copy up their keywords from the leaf (and don't push them up)
			// we can simply spin across the leaf nodes and return all the keywords
			foreach (BPlusTreeNode leaf in TraverseLeaves()) {
				for (int i=0; i < leaf.KeywordCount; i++) {
					// return current keyword
					yield return leaf.Keywords[i];
				}
			}
		}

		public IEnumerable<KeywordMatch> GetAllMatches() {
			foreach (BPlusTreeNode leaf in TraverseLeaves()) {
				// yield all the values in this node
				for (int i=0; i < leaf.KeywordCount; i++) {
					yield return new KeywordMatch { Keyword = leaf.Keywords[i], Value = leaf.ChildrenByteOffsets[i] };
				}
			}
		}

		public IEnumerable<string> GetAllKeywords() {
			return TraverseKeywords().Distinct();
		}

		/// <summary>
		/// Disposes this tree then deletes all associated files
		/// </summary>
		public void DeleteFiles() {
			Dispose();
			if (File.Exists(_treeFile)) {
				File.Delete(_treeFile);
			}
		}

		#region IDisposable Members

		public void Dispose() {
			if (_reader != null) {
				_reader.Close();
				_reader = null;
			}
			if (_writer != null) {
				_writer.Close();
				_writer = null;
			}
		}

		#endregion
	}
}
