using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;
using System.Threading;
using System.Diagnostics;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {


    
    public class BPlusTree<TKey, TValue>
		: IDisposable
		where TKey : ISearchable<TKey>, new() 
		where TValue : IPersistable<TValue>, new() {

		#region Properties

        internal delegate object TreeLockCallback(WriteLockCallback callback, object[] arguments);
        internal delegate BPlusTreeNode<TKey, TValue> ReadLockCallback(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent, BinaryReader rdr, object[] args);
        internal delegate BPlusTreeNode<TKey, TValue> WriteLockCallback(BinaryWriter wtr, object[] args);

        //private NodeLockManager _nodeLockManager;
        //private object _treeLock;
        //internal object LockObject {
        //    get {
        //        return _treeLock;
        //    }
        //}

		private const short MINIMUM_FANOUT_SIZE = 3;
		private const short ENCODING_STORAGE_SIZE = 50;
		private long _rootLocation;
		private Stream _stm;
		private BinaryReader _reader;
		private BinaryWriter _writer;
        private int _firstNodeLocation;

        private int _nodeCacheSize;
        private int _keywordCacheSize;
		internal Dictionary<long, BPlusTreeNode<TKey, TValue>> NodeCache { get; private set; }
		internal Dictionary<string, List<KeywordMatch<TKey, TValue>>> KeywordCache { get; private set; }

		internal long RootLocation {
			get {
				return _rootLocation;
			}
		}

		internal int DefaultSlotSize {
			get {
                // TODO: make this more type-appropriate and possibly filled from config settings
                //   7 = header size
                //   8 = average size of a keyword (guesstimate)
                //  10 = average size of a value   (guesstimate)
				return 7 + (8 + 10) * FanoutSize;
            //    return 10; // debugging node relocation and splitting
            }
		}

        public short AverageKeywordSize { get; private set; }

		public short FanoutSize { get; private set; }
		/// <summary>
		/// Returns the Median based purely on the FanoutSize. FanoutSize=3 or FanoutSize=4 both return 2 because Ceiling(3/2) = Ceiling(4/2) = 2
		/// </summary>
		public int Median {
			get {
				return (int)(Math.Ceiling((decimal)FanoutSize / 2.0M));
			}
		}

		public Encoding Encoding { get; private set; }
		public BPlusTreeNode<TKey, TValue> Root { get; private set; }
		private List<NodeSlot> AvailableSlots;
		#endregion


		#region Persistance

        #region Concurrency Management


        private static object __lockObject = new object();
        internal void Lock(Toolkit.VoidCallback callback) {
            lock (__lockObject) {
                callback();
            }
        }
        /// <summary>
        /// Used internally to prepare a binary writer for writing, then it is passed back to caller via the callback
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        internal bool WriteNodeToDisk(WriteLockCallback callback, object[] args) {
            var moved = false;
            Lock(delegate() {
                moved = ((bool)processWriteCallback(callback, args));
            });
            return moved;
        }

        private object processWriteCallback(WriteLockCallback callback, object[] args){

            bool needsToBeMoved = false;

            try {
                LogInfo("Begin writing node");
                using (MemoryStream ms = new MemoryStream()) {
                    using (BinaryWriter bw = new BinaryWriter(ms)) {
                        // give them a memorystream to write to
                        BPlusTreeNode<TKey, TValue> node = callback(bw, args);

                        // the memorystream now has the node in it. only properties that may change are 
                        // the SlotSize and the Location, as those are the only ones related to how it is stored in the file.

                        // determine if that stream will fit in the node's current slot (if slot is at the end of the file,
                        // let it write w/o moving)

                        if (node.Location < _firstNodeLocation) {
                            // brand new node
                            needsToBeMoved = true;
                        } else if (ms.Length > node.SlotSize) {

                            // for concurrency sake, we relocate even if the current node is the last one in the file
                            // (in case another node relocation is taking place, meaning the 'end of the file' may change
                            //  in the middle of our write)
                            needsToBeMoved = true;


                            //// slot size is exceeded.  if it's the last node, we don't need to move it.
                            //if (node.Location + node.SlotSize != _writer.BaseStream.Length) {
                            //    // exceeded slot size and it's not the last one in the file.
                            //    needsToBeMoved = true;
                            //} else {
                            //    // this is currently the last node in the file.
                            //    // we don't need to relocate it, but the slotsize is too small.
                            //    // so grow the slotsize out
                            //}
                        }

                        int origSlotSize = node.SlotSize;
                        long origLocation = node.Location;

                        if (needsToBeMoved) {

                            // slot size exceeded or brand new node.
                            // if we're not the very last slot in the stream, we must move it.

                            // need to move this node and free up its previous slot
                            // doesn't fit in current slot.

                            node.SlotSize = calcRequiredSlotSize((int)ms.Length);
                            bw.BaseStream.Position = 0;
                            node.WriteSlotSize(bw);

                            // need to move it to a different slot (which notifies siblings, if any, and writes them as well)
                            if (node.IsRoot) {
                                node.MoveTo(GetAvailableSlot(node.SlotSize));
                                // make sure node is still seen as the root if it was previously the root
                                _rootLocation = node.Location;
                                writeHeader();
                            } else {
                                node.MoveTo(GetAvailableSlot(node.SlotSize));
                            }

                            if (origLocation >= _firstNodeLocation) {
                                // zero out the original location on disk so we know if we have a bad reference
                                Lock(delegate() {
                                    _writer.BaseStream.Position = origLocation;
                                    _writer.Write(new byte[origSlotSize], 0, origSlotSize);
                                });
                            }


                            //// since we've changed things within the node, call node.Write
                            //// (which is essentially a recursive call to this method)
                            //// we should never have more than one level of recursion for a given node!!!
                            //node.Write();


                            //return null;

                        }

                        // we get here, we have the proper slot

                        // init memorystream -- position @ 0, pad with empty bytes as needed
                        ms.Position = 0;
                        ms.SetLength(node.SlotSize);

                        // write memorystream bytes to filestream
                        Lock(delegate() {
                            _writer.BaseStream.Position = node.Location;
                            _writer.Write(ms.ToArray());
                            _writer.Flush();

                        });

                        // mark old slot as available if we moved it and it was a valid location to begin with
                        if (needsToBeMoved) {
                            if (origLocation > _firstNodeLocation) {
                                AvailableSlots.Add(new NodeSlot { Location = origLocation, Size = origSlotSize });
                            }
                        }

                        if (node.IsRoot) {
                            // update header info
                            writeHeader();
                        } else {
                            // update the node cache so subsequent requests don't return the stale node
                            // (note we never cache the root, it's already cached by the Root property)
                            updateNodeCache(node, origLocation);
                        }


                    }
                }

            } finally {
                LogInfo("End Writing Node");
            }
            return needsToBeMoved;

        }

        /// <summary>
        /// Used internally to atomically read a node from disk
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="callback"></param>
        /// <param name="location"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal BPlusTreeNode<TKey, TValue> ReadNodeFromDisk(BPlusTreeIndexNode<TKey, TValue> parent, ReadLockCallback callback, long location, object[] args) {
			if (callback == null){
				return null;
			}

            BPlusTreeNode<TKey, TValue> node = null;
            try {
                LogInfo("Begin reading node");
                this.Lock(delegate() {
                    _stm.Position = location;

                    try {

                        //_nodeLockManager.Lock(location, false);

                        // Critical section starts here
                        node = callback(this, parent, _reader, args);

                        // don't forget to cache it (root is never cached, Tree object contains a direct reference to it since 
                        // it is used so often and there is always exactly one root node.
                        if (!node.IsRoot && parent != null) {
                            // NOTE: parent will be null only when:
                            //       1) reading the root node
                            //       2) reading the leaf node from another leaf node via RightSiblingLocation (i.e. spinning across the bottom of the tree when doing non-exact searches)
                            updateNodeCache(node, node.Location);
                        }

                    } finally {
                        // _nodeLockManager.Unlock(location, false);
                    }

                });
            } finally {
                LogInfo("End reading node");
            }

            return node;

        }

        #endregion Concurrency Management

        #region Node and Keyword Caching


        private void fillNodeCacheBreadthFirst() {

            try {
                LogInfo("Begin filling node cache breadth first");
                // wipe any existing cached nodes
                NodeCache.Clear();

                foreach (var node in TraverseBreadthFirst(false)) {
                    if (NodeCache.Count >= _nodeCacheSize) {
                        return;
                    } else {
                        NodeCache[node.Location] = node;
                    }
                }
            } finally {
                LogInfo("End filling node cache breadth first");
            }
        }


        private void initCaches(bool fillNodeCache) {
            NodeCache = new Dictionary<long, BPlusTreeNode<TKey, TValue>>();
            KeywordCache = new Dictionary<string, List<KeywordMatch<TKey, TValue>>>();

            if (fillNodeCache) {
                // precache the given amount of nodes (skipping Root, as it's always cached)
                // to do this, we do a breadth-first search of the tree until we hit our cache limit.

                fillNodeCacheBreadthFirst();
            }

        }

        private void removeFromKeywordCache(string keyword) {

            // for now... we'll just clear all of them out when something has changed.
            if (KeywordCache != null) {
                KeywordCache.Clear();
            }

            return;


            //// invalidate the associated keywordcache
            //// we can't change a collection while iterating through it, so we queue up the keys to remove first
            //// then remove them outside the iteration
            //List<string> keysToRemove = new List<string>();
            //foreach (string key in KeywordCache.Keys) {
            //    if (key.Contains(generatedKeyword)) {
            //        keysToRemove.Add(key);
            //    }
            //}
            //foreach (string keyToRemove in keysToRemove) {
            //    if (!String.IsNullOrEmpty(keyToRemove)) {
            //        KeywordCache.Remove(keyToRemove);
            //    }
            //}
        }

        [Conditional("DEBUGBPLUSTREE")]
        internal static void LogInfo(string text) {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + DateTime.Now.ToString() + ": " + text);
        }

        /// <summary>
        /// If there is room in the cache, adds/updates the cache value for the given node
        /// </summary>
        /// <param name="node"></param>
        private void updateNodeCache(BPlusTreeNode<TKey, TValue> node, long previousLocationOnDisk) {

            try {
                LogInfo("Begin updating node cache");
                if (NodeCache == null) {
                    // cache is disabled (probably doing bulk insert during index creation)
                    LogInfo("Node cache is disabled");
                } else {
                    // we're either adding or updating it -- we don't care, Dictionary will handle that.
                    // special check for when we first load the index (root is null)
                    if (this.Root != null && !node.IsRoot) {

                        if (previousLocationOnDisk != node.Location) {
                            // if the node is moving, remove it from the cache by its previous Location value...
                            if (NodeCache.ContainsKey(previousLocationOnDisk)) {
                                LogInfo("Detected node cache contained a now-outdated reference, so that is being removed now...");
                                NodeCache.Remove(previousLocationOnDisk);
                            }
                        }


                        BPlusTreeNode<TKey, TValue> temp = null;
                        if (NodeCache.TryGetValue(node.Location, out temp)){

                            // cache contained a copy of the node.  replace it with the new, updated reference.

                            LogInfo("Replacing node cache entry: " + node);
                            NodeCache[node.Location] = node;

                        } else {

                            // cache has no node for given location.  If we haven't hit our max yet, add it.

                            if (NodeCache.Count < _nodeCacheSize) {
                                LogInfo("Adding node to cache: " + node);
                                NodeCache[node.Location] = node;
                            } else {
                                // cache is full, don't cache it. next read of this node will cause a disk access.
                                LogInfo("Node cache is full, so skipping: " + node);
                            }
                        }



                        // if the node is moving (and it is an index node, aka has children), 
                        // make sure all children know to point at the new object reference that is now in the cache...
                        if (previousLocationOnDisk != node.Location) {
                            if (node is BPlusTreeIndexNode<TKey, TValue>) {

                                LogInfo("Detected we are caching an index node that was moved.  Need to tell all cached children it has moved as well.");
                                var idxNode = node as BPlusTreeIndexNode<TKey, TValue>;
                                foreach (var childLoc in idxNode.ChildLocations) {
                                    BPlusTreeNode<TKey, TValue> tempChild = null;
                                    if (NodeCache.TryGetValue(childLoc, out tempChild)) {
                                        LogInfo("Updating parent pointer to parent node now in cache from child node=" + tempChild);
                                        tempChild.Parent = idxNode;
                                    }
                                }
                            }
                        }


                    }
                }
             } finally {
                LogInfo("End filling node cache");
            }
        }

        private void addKeywordToCache(string generatedKeyword, List<KeywordMatch<TKey, TValue>> results) {

            // first we construct a unique keyword by concatenating the 3 variables
            // (a search for 'joe' with a matchmode of ExactMatch ignoring case could return different results
            //  than a search for 'joe' with a matchmode of Contains and being case-sensitive)

            try {
                LogInfo("Begin adding keyword to cache");
                if (KeywordCache == null || _keywordCacheSize <= 0) {
                    // keyword cache is disabled
                    LogInfo("Keyword cache is disabled");
                    return;
                }



                if (KeywordCache.ContainsKey(generatedKeyword)) {
                    // is already in cache. refresh it.
                    KeywordCache[generatedKeyword] = results;
                } else {
                    if (KeywordCache.Count >= _keywordCacheSize) {
                        // cache is full. remove an item and add this one.
                        // HACK: it's impossible to tell the the order keys were added in a Dictionary since it is unordered,
                        // but we can grab the first one returned by the Keys collection.  we'll just remove it and hope for the best. :)
                        string existingKey = null;
                        foreach (string key in KeywordCache.Keys) {
                            existingKey = key;
                            break;
                        }
                        if (!String.IsNullOrEmpty(existingKey)) {
                            KeywordCache.Remove(existingKey);
                        }
                    }

                    // add the new keyword
                    KeywordCache[generatedKeyword] = results;

                }
            } finally {
                LogInfo("End adding keyword to cache");
            }
        }


   		private string generateUniqueKeywordCacheKey(string keyword, KeywordMatchMode matchMode, bool ignoreCase) {
			StringBuilder sb = new StringBuilder(keyword);
			sb.Append("|")
				.Append(((int)matchMode).ToString())
				.Append("|")
				.Append((ignoreCase ? "Y" : "N"));
			return sb.ToString();

		}

        #endregion Node and Keyword Caching

        private NodeSlot GetAvailableSlot(int minimumSlotSize) {

            // since this method deals with the end of the file, we want only 1 thead at a time
            // to deal with it.  Reentrant calls on the same thread are fine, but concurrent access
            // to the length of the file is not good.  Since this is the only place where the length of the file may
            // be changed, by controlling it with a tree-wide lock we can make sure we do not have race conditions or deadlocks.

            NodeSlot slot = null;
            Lock(delegate() {
                long newSlotLocation = _writer.BaseStream.Length;

                // disabled for now ... debugging...
                //for (int i = 0; i < AvailableSlots.Count; i++) {
                //    NodeSlot ns = AvailableSlots[i];
                //    if (minimumSlotSize < ns.Size) {
                //        // this one can be used. remember its location and remove it from the available list
                //        AvailableSlots.RemoveAt(i);
                //        return ns;
                //    }
                //}

                // reserve the slot in the file by zeroing it out
                // this requires an additional write but we are guaranteed to reserve the space.
                int newSlotSize = calcRequiredSlotSize(minimumSlotSize);
                _writer.BaseStream.Position = newSlotLocation;
                _writer.BaseStream.Write(new byte[newSlotSize], 0, newSlotSize);


                slot = new NodeSlot { Location = newSlotLocation, Size = newSlotSize };
            });

            return slot;
		}

		private int calcRequiredSlotSize(int minimumSlotSize) {
			// determine the next chunk to stop at

            // jump to the nearest multiple of default slot size that is at least as long as minimum slot size
            int defaultSlotCount = (minimumSlotSize / DefaultSlotSize) + 1;
            return defaultSlotCount * DefaultSlotSize;


            //if (minimumSlotSize > DefaultSlotSize) {
            //    newSlotSize = defaultSlotCount * DefaultSlotSize;
            //} else {
            //    // default slot size is enough.
            //    newSlotSize = DefaultSlotSize;
            //}




            //int oneShy = minimumSlotSize / DefaultSlotSize;
            //int remainder = minimumSlotSize % DefaultSlotSize;

            //int minSlotSize = (oneShy * DefaultSlotSize);

            //if (remainder == 0) {
            //    // node exactly fits in this slot.
            //    return minSlotSize;
            //} else {
            //    // node overlaps at least a little. bump up one.
            //    return minSlotSize + DefaultSlotSize;
            //}

		}

		#endregion Persistance


		#region Instantiation

        public static BPlusTree<TKey, TValue> Create(string fileName, Encoding encoding, short fanoutSize, short averageKeywordSize, int nodeCacheSize, int keywordCacheSize) {
			FileStream fs = new FileStream(Toolkit.ResolveFilePath(fileName, true), FileMode.Create, FileAccess.ReadWrite);
			return new BPlusTree<TKey, TValue>(fs, encoding, fanoutSize, averageKeywordSize, nodeCacheSize, keywordCacheSize);
		}

		public static BPlusTree<TKey, TValue> Create(Stream stream, Encoding encoding, short fanoutSize, short averageKeywordSize, int nodeCacheSize, int keywordCacheSize) {
			return new BPlusTree<TKey, TValue>(stream, encoding, fanoutSize, averageKeywordSize, nodeCacheSize, keywordCacheSize);
		}

        public static BPlusTree<TKey, TValue> Load(string fileName, bool readOnly) {
            return Load(fileName, readOnly, -1, -1);
        }

		public static BPlusTree<TKey, TValue> Load(string fileName, bool readOnly, int nodeCacheSize, int keywordCacheSize) {
            FileStream fs = null;
            try {
                fs = new FileStream(Toolkit.ResolveFilePath(fileName, true), FileMode.Open, (readOnly ? FileAccess.Read : FileAccess.ReadWrite));
                return Load(fs, nodeCacheSize, keywordCacheSize);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                // clean up if something bombs!
                if (fs != null) {
                    fs.Dispose();
                }
                throw;
            }
		}

		public static BPlusTree<TKey, TValue> Load(Stream stream, int nodeCacheSize, int keywordCacheSize) {
			return new BPlusTree<TKey, TValue>(stream, nodeCacheSize, keywordCacheSize);
		}

		private BPlusTree(Stream stream, int nodeCacheSize, int keywordCacheSize) {

            //_nodeLockManager = new NodeLockManager();
            //_treeLock = new object();

            Lock(delegate() {

                _stm = stream;

                // read in encoding first, using the default ASCII reader
                byte[] bytes = new byte[ENCODING_STORAGE_SIZE];
                _stm.Read(bytes, 0, bytes.Length);
                string encodingClass = ASCIIEncoding.ASCII.GetString(bytes).Trim();
                Encoding = (Encoding)Toolkit.LoadClass(Type.GetType(encodingClass));


                _reader = new BinaryReader(_stm, Encoding);
                if (_stm.CanWrite) {
                    _writer = new BinaryWriter(_stm, Encoding);
                }

                // read in fanoutsize
                FanoutSize = _reader.ReadInt16();

                // read in average keyword size
                AverageKeywordSize = _reader.ReadInt16();

                // read in cache sizes
                _nodeCacheSize = nodeCacheSize;
                _keywordCacheSize = keywordCacheSize;

                AvailableSlots = new List<NodeSlot>();
                // read in 3 AvailableSlots
                for (int i = 0; i < 3; i++) {
                    NodeSlot slot = new NodeSlot();
                    slot.Read(_reader);
                    if (slot.Location > 0) {
                        AvailableSlots.Add(slot);
                    }
                }


                // read in root location
                long rootLocation = _reader.ReadInt64();

                // for slot management, remember the first available node position 
                // (i.e. we've read in all the header info, first byte possibly belonging to a valid node is at the current position)
                _firstNodeLocation = (int)_reader.BaseStream.Position;

                NodeCache = new Dictionary<long, BPlusTreeNode<TKey, TValue>>();
                KeywordCache = new Dictionary<string, List<KeywordMatch<TKey, TValue>>>();

                // read in root
                Root = BPlusTreeNode<TKey, TValue>.Read(this, null, rootLocation);

                // precache as many index nodes as possible
                // initCaches(true);

            });
		}


		private BPlusTree(Stream stream, Encoding encoding, short fanoutSize, short averageKeywordSize, int nodeCacheSize, int keywordCacheSize){

            Lock(delegate() {

                _stm = stream;

                if (encoding == null) {
                    encoding = UTF8Encoding.UTF8;
                }

                Encoding = encoding;

                _reader = new BinaryReader(_stm, encoding);
                if (_stm.CanWrite) {
                    _writer = new BinaryWriter(_stm, encoding);
                }

                FanoutSize = fanoutSize;

                AverageKeywordSize = averageKeywordSize;

                if (nodeCacheSize < 0) {
                    // default to adding in first two non-root levels of the B+ tree
                    _nodeCacheSize = FanoutSize + (FanoutSize * FanoutSize);
                } else {
                    _nodeCacheSize = nodeCacheSize;
                }

                if (keywordCacheSize < 0) {
                    _keywordCacheSize = 200;
                } else {
                    _keywordCacheSize = keywordCacheSize;
                }

                AvailableSlots = new List<NodeSlot>();

                writeHeader();
            });

		}

		private void writeHeader(){

            try {
                LogInfo("Begin writing Header");
                Lock(delegate() {
                    // jump to beginning of file
                    _stm.Position = 0;

                    // write out string encoding class
                    byte[] encodingClassBytes = ASCIIEncoding.ASCII.GetBytes(Encoding.GetType().ToString().PadRight(ENCODING_STORAGE_SIZE));
                    _stm.Write(encodingClassBytes, 0, ENCODING_STORAGE_SIZE);

                    // write out fanout size
                    _writer.Write(FanoutSize);

                    // write out average keyword size
                    _writer.Write(AverageKeywordSize);

                    // reserve 3 places to store available slot information
                    for (int i = 0; i < 3; i++) {
                        if (i < AvailableSlots.Count) {
                            AvailableSlots[i].Write(_writer);
                        } else {
                            new NodeSlot().Write(_writer);
                        }
                    }


                    // first time through Root will be null (or have an invalid Location)
                    if (Root == null || Root.Location < 1) {
                        // write root location as the immediately following bytes
                        _rootLocation = _stm.Length + sizeof(long);
                        _writer.Write(_rootLocation);
                        _firstNodeLocation = (int)_writer.BaseStream.Position;

                        // create a root and write it out
                        Root = new BPlusTreeLeafNode<TKey, TValue>(this, null, null);
                        Root.Write();
                    } else {

                        _rootLocation = Root.Location;
                        _writer.Write(_rootLocation);
                        _firstNodeLocation = (int)_writer.BaseStream.Position;


                    }
                });
            } finally {
                LogInfo("End Writing Header");
            }

		}


		#endregion Instantiation


		#region Traversals

        private BPlusTreeLeafNode<TKey, TValue> getRightmostLeafNode(ref int depth) {
            if (Root.IsLeaf) {
                return Root as BPlusTreeLeafNode<TKey, TValue>;
            }

            BPlusTreeNode<TKey, TValue> node = this.Root;
            while (!node.IsLeaf) {
                var idxNode = node as BPlusTreeIndexNode<TKey, TValue>;
                node = BPlusTreeNode<TKey, TValue>.Read(this, idxNode, idxNode.LastChildLocation);
                depth++;
            }
            return node as BPlusTreeLeafNode<TKey, TValue>;
        }

        internal int GetKeywordCount(bool approximate) {
            // first, drill down to leafmost leaf node
            var keywordCount = 0;
            if (Root.IsLeaf) {
                return Root.Keywords.Count;
            } else {

                if (approximate) {
                    var depth = 1;

                    BPlusTreeNode<TKey, TValue> node = this.Root;
                    keywordCount = node.Keywords.Count;
                    var fanSize = node.Keywords.Count;
                    while (!node.IsLeaf) {
                        var idxNode = node as BPlusTreeIndexNode<TKey, TValue>;
                        node = BPlusTreeNode<TKey, TValue>.Read(this, idxNode, idxNode.LastChildLocation);
                        depth++;

                        // approximate, just count all nodes but rightmost one at current depth as being full
                        keywordCount += (int)Math.Pow((double)(fanSize - 1), (double)depth);
                        // ... and get halfway close on the rightmost node since bulk insert usually doesn't fill this guy completely full
                        keywordCount += node.Keywords.Count;
                        fanSize = node.Keywords.Count;
                    }
                    return keywordCount;
                } else {
                    keywordCount = 0;
                    foreach (var leaf in TraverseLeaves()) {
                        keywordCount += leaf.Keywords.Count;
                    }

                    return keywordCount;
                }
            }
        }

        private BPlusTreeLeafNode<TKey, TValue> getLeftmostLeafNode() {
            if (Root.IsLeaf) {
                return Root as BPlusTreeLeafNode<TKey, TValue>;
            }

            BPlusTreeNode<TKey, TValue> node = this.Root;
            while (!node.IsLeaf) {
                node = BPlusTreeNode<TKey, TValue>.Read(this, node as BPlusTreeIndexNode<TKey, TValue>, ((BPlusTreeIndexNode<TKey, TValue>)node).ChildLocations[0]);
            }
            return node as BPlusTreeLeafNode<TKey, TValue>;
        }

		internal IEnumerable<BPlusTreeLeafNode<TKey, TValue>> TraverseLeaves() {

			// since B+ trees store all values at the leaves and
			// contain pointers to sibling leaves,
			// we simply need to drill down to the leftmost node
			// and spin across the other leaves from there

			// first, drill down to leafmost leaf node
			BPlusTreeLeafNode<TKey, TValue> leaf = getLeftmostLeafNode();

			while (leaf != null) {
				yield return leaf;
                leaf = BPlusTreeNode<TKey, TValue>.Read(this, null, leaf.RightSiblingLocation) as BPlusTreeLeafNode<TKey, TValue>;
			}

		}

		private IEnumerable<BPlusTreeNode<TKey, TValue>> traverseBreadthFirst(BPlusTreeNode<TKey, TValue> subtreeRoot) {

			if (subtreeRoot.IsLeaf) {
				// leaves never have children -- we should get here only if we were incorrectly given a subtreeRoot as a leaf node
				yield return subtreeRoot;
			} else {

				BPlusTreeIndexNode<TKey, TValue> indexNode = subtreeRoot as BPlusTreeIndexNode<TKey, TValue>;
				// return all nodes at this level
				for (int i=0; i < indexNode.ChildLocations.Count; i++) {
                    BPlusTreeNode<TKey, TValue> node = BPlusTreeNode<TKey, TValue>.Read(this, indexNode, indexNode.ChildLocations[i]);
					yield return node;
				}

				// return all nodes at level below this one
				for (int i=0; i < indexNode.ChildLocations.Count; i++) {
					BPlusTreeNode<TKey, TValue> node = BPlusTreeNode<TKey, TValue>.Read(this, indexNode, indexNode.ChildLocations[i]);
					if (!node.IsLeaf) {
						foreach (BPlusTreeNode<TKey, TValue> child in traverseBreadthFirst(node)) {
							yield return child;
						}
					}
				}
			}
		}

		public IEnumerable<BPlusTreeNode<TKey, TValue>> TraverseBreadthFirst(bool returnRoot) {

			// first return the root itself (special case)
            if (returnRoot){
    			yield return Root;
            }

			// start a subtree breadth first search at the root
            foreach (BPlusTreeNode<TKey, TValue> node in traverseBreadthFirst(Root)) {
				yield return node;
			}

		}

		/// <summary>
		/// Returns all nodes that are invalid or point to invalid nodes.
		/// </summary>
		/// <param name="subtreeRoot"></param>
		/// <param name="inspectLeaves"></param>
		/// <returns></returns>
		internal IEnumerable<BPlusTreeNode<TKey, TValue>> GetAllMalformedNodes(bool inspectLeaves) {

			foreach (BPlusTreeNode<TKey, TValue> node in TraverseBreadthFirst(true)) {
				if (node.Keywords.Count == 0) {
                    if (node.IsRoot && node.IsLeaf) {
                        // only the root node can have 0 keywords -- but then only if there is no other data.
                        var root = node as BPlusTreeLeafNode<TKey, TValue>;
                        if (root.Values.Count > 0) {
                            // node claims to be a leaf node with no keywords, yet has at least one value.
                            // invalid node.
                            yield return node;
                        } else {
                            // is a valid, empty root which is the only leaf node in the entire tree.
                        }
                    } else {
                        // node is either not the root or is the root and is not a leaf.
                        // invalid node.
                        yield return node;
                    }
				} else {
					if (node.IsLeaf) {
						if (!inspectLeaves) {
							// we're done, we were told to skip leaves (even if it's the root)
							break;
						} else {
							// make sure all keywords are non-empty
							for (int i=0; i < node.Keywords.Count; i++) {
								// make sure all keywords are filled in
								if (node.Keywords[i] == null) {
									// invalid keyword
									yield return node;
								}
							}
						}
					} else {
						for (int i=0; i < node.Keywords.Count; i++) {
							// make sure all keywords are filled in
							if (node.Keywords[i] == null) {
								// invalid keyword
								yield return node;
							}
							// check valid child pointers
							if (((BPlusTreeIndexNode<TKey, TValue>)node).ChildLocations[i] < 1) {
								// invalid child pointer. malformed.
								yield return node;
							}
						}
						// check rightmost child pointer
						if (((BPlusTreeIndexNode<TKey, TValue>)node).LastChildLocation < 1) {
							// invalid rightmost child pointer. malformed.
							yield return node;
						}
					}
				}
			}
		}

        internal IEnumerable<TKey> TraverseKeywords() {
			// since B+ trees always copy up their keywords from the leaf (and don't push them up)
			// we can simply spin across the leaf nodes and return all the keywords
            foreach (BPlusTreeNode<TKey, TValue> leaf in TraverseLeaves()) {
				foreach(TKey key in leaf.Keywords){
					// return current keyword
					yield return key;
				}
			}
		}

        public IEnumerable<TKey> GetAllKeywords() {
            return TraverseKeywords();
		}

        public IEnumerable<BPlusTreeLeafNode<TKey, TValue>> GetAllLeaves() {
            return TraverseLeaves();
        }

		#endregion Traversals

		#region Operations

		/// <summary>
		/// String is used so often for B+ tree keywords, this overload accepts a string (and creates a BPlusString out of it) and calls the 'normal' method
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="matchMode"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public List<KeywordMatch<TKey, TValue>> Search(string keyword, KeywordMatchMode matchMode, bool ignoreCase) {
			var key = new TKey().Parse(keyword);
			return Search(key, matchMode, ignoreCase);
		}

		/// <summary>
		/// Searches the tree for the given keyword and based on matchMode and ignoreCase, returns 0 to many keyword matches.
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="matchMode"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public List<KeywordMatch<TKey, TValue>> Search(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase) {

            
            var generatedKeyword = generateUniqueKeywordCacheKey(keyword.ToString(), matchMode, ignoreCase);

            List<KeywordMatch<TKey, TValue>> results = null;

            if (!KeywordCache.TryGetValue(generatedKeyword, out results)) {
                results = Root.Search(keyword, matchMode, ignoreCase);
                addKeywordToCache(generatedKeyword, results);
            }

            return results;
		}


		/// <summary>
		//// String is used so often for B+ tree keywords, this overload accepts a string (and creates a BPlusString out of it) and calls the 'normal' method
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="matchMode"></param>
		/// <param name="newValue"></param>
		/// <param name="updateMode"></param>
		/// <returns>List of all items this update affected</returns>
		public List<KeywordMatch<TKey, TValue>> Update(string keyword, KeywordMatchMode matchMode, TValue newValue, UpdateMode updateMode) {
            return Update(new TKey().Parse(keyword), matchMode, newValue, updateMode);
		}

		/// <summary>
		/// Finds keywords in the tree that match the given one (based on matchMode) and updates the value according to the update mode
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="matchMode"></param>
		/// <param name="newValue"></param>
		/// <param name="updateMode">Append means add the given newValue to the existing value.  Replace means completely replace the existing value with the newValue.  Remove means remove occurrences of the newValue from the existing value.</param>
		/// <returns>List of all items this update affected</returns>
		public List<KeywordMatch<TKey, TValue>> Update(TKey keyword, KeywordMatchMode matchMode, TValue newValue, UpdateMode updateMode) {
            removeFromKeywordCache(keyword.ToString());
            return Root.Update(keyword, matchMode, newValue, updateMode);
        }

		/// <summary>
		//// String is used so often for B+ tree keywords, this overload accepts a string (and creates a BPlusString out of it) and calls the 'normal' method
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="newValue"></param>
		public KeywordMatch<TKey, TValue> Insert(string keyword, TValue newValue) {
			return Insert(new TKey().Parse(keyword), newValue);
		}

		/// <summary>
		/// Inserts a new keyword and its value into the tree.
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="newValue"></param>
		public KeywordMatch<TKey, TValue> Insert(TKey keyword, TValue newValue) {
            if (Root.IsFull) {
                // we must first split the root
                BPlusTreeNode<TKey, TValue> newChild;
                ReplaceRoot(out newChild);

                foreach (var node in TraverseBreadthFirst(true)) {
                    Debug.WriteLine(node.ToString());
                }


            }

            removeFromKeywordCache(keyword.ToString());

            // root is not full, recursively traverse down to appropriate leaf node and add it
            return Root.Insert(keyword, newValue);
		}








		/// <summary>
		/// Replaces the full root with a new root (sets the Root property) and returns it.  Newly created child node is returned in the out parameter.
		/// </summary>
		/// <param name="newChild"></param>
		/// <returns></returns>
		internal BPlusTreeIndexNode<TKey, TValue> ReplaceRoot(out BPlusTreeNode<TKey, TValue> newChild) {

			// NOTE: This is a very special case.
			// replacing the root involves creating 2 new nodes:
			//  1. the new root node
			//  2. splitting the existing root node into 2 nodes, one of which is a new one

			// we use this "newRoot" variable just so we don't have to constantly cast Root to an IndexNode.
			BPlusTreeIndexNode<TKey, TValue> newRoot = null;


			BPlusTreeNode<TKey, TValue> origRoot = Root;

            try {
                LogInfo("Begin replacing root node");
                // create a new, empty root node and assign our root to it
                Root = newRoot = new BPlusTreeIndexNode<TKey, TValue>(this, null, null);

                // make original root first child of new root so SplitChild works properly (don't forget the parent pointer! important for SplitChild...)
                BPlusTreeNode<TKey, TValue> childNode = null;

                Lock(delegate() {
                    newRoot.ChildLocations.Add(origRoot.Location);
                    newRoot.Write();
                    origRoot.Parent = newRoot;

                    // SplitChild will write out all 3 nodes (original root, new root, new child)
                    childNode = newRoot.SplitChild(origRoot);
                });

                newChild = childNode;
                return newRoot;
            } finally {
                LogInfo("End replacing root node");
            }
		}




















		/// <summary>
		/// Bulk inserts the given dataStream into a new tree.  Cannot be used on existing trees!  Returns the total number of leaf nodes.
		/// </summary>
		/// <param name="dataStream"></param>
		/// <param name="fillFactor">Percentage of how full each leaf node should be.  If value is not somewhere between 0.0 and 1.0, 1.0 is assumed.  An absolute minimum of 3 keywords per node is also enforced.</param>
		/// <returns></returns>
		public int BulkInsert(Stream dataStream, decimal fillFactor) {

			BPlusTreeLeafNode<TKey, TValue> prevLeaf = null;
			BPlusTreeIndexNode<TKey, TValue> parent = null;
			BPlusTreeLeafNode<TKey, TValue> leaf = new BPlusTreeLeafNode<TKey, TValue>(this, parent, null);

			// fillFactor tells us how full we want leaves to be.
			// when we hit that amount, we should start creating a new leaf.
			// logic is all the same, just instead of stopping @ FanoutSize, we stop at the calculated fillFactor.

			int stopFillingAt = (int)(Math.Ceiling((fillFactor <= 0.0M || fillFactor >= 1.0M ? FanoutSize : fillFactor * FanoutSize)));

			// our implementation requires a minimum of 3 per node...
			if (stopFillingAt < MINIMUM_FANOUT_SIZE) {
				stopFillingAt = MINIMUM_FANOUT_SIZE;
			}

			int i = 0;
			int leafCount = 0;
			int position = 0;

			using(BinaryReader dataReader = new BinaryReader(dataStream)){
				while (dataStream.Position < dataStream.Length){

					position = (i % stopFillingAt);
					if (position == 0 && i > 0) {
						// parent / prevLeaf / leaf may all change, so we pass references to them
						bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, dataStream.Position == dataStream.Length, stopFillingAt);
						leafCount++;
					}

					// data is grouped in the queue file.
					// 

					// read key / value from stream
					TKey key = new TKey();
					key.Read(dataReader);

					TValue val = new TValue();
					val.Read(dataReader);

					leaf.Keywords.Add(key);
					leaf.Values.Add(val);


					i++;


				}


				if (leaf.Keywords.Count > 0){
					// haven't written out the very last one yet.
					bulkSaveNodes(ref parent, ref prevLeaf, ref leaf, true, stopFillingAt );
					leafCount++;
				} else {
					// we added one too many leaves, drop the last one
					for(int j=0;j<AvailableSlots.Count;j++){
						if (AvailableSlots[j].Location == leaf.Location){
							AvailableSlots.RemoveAt(j);
							break;
						}
					}
				}

				return leafCount;
			}

		}

		private BPlusTreeIndexNode<TKey, TValue> bulkSplitAsNeeded(BPlusTreeIndexNode<TKey, TValue> indexNode) {
			BPlusTreeNode<TKey, TValue> newParent = null;
			if (!indexNode.IsFull){
				// this guy isn't full, we're done looking. just return it.
				return indexNode;
			} else {
				if (indexNode.IsRoot){
					// splitting root...
					 ReplaceRoot(out newParent);

				} else {
					// splitting non-root index node...
					if (indexNode.Parent.IsFull) {
						indexNode.Parent = bulkSplitAsNeeded(indexNode.Parent);
					}

                    indexNode.Parent.Write();
                    newParent = indexNode.Parent.SplitChild(indexNode) as BPlusTreeIndexNode<TKey, TValue>;

					if (indexNode.Keywords.Count == 0 || newParent.Keywords.Count == 0) {
						throw new InvalidOperationException(getDisplayMember("bulkSplitAsNeeded", "Bad keyword count"));
					}

				}
			}

			// newParent has already been written to disk (and therefore relocated if needed)
			// so callers to this method (including itself) can trust its Location property to be correct until another keyword is added
			// (which may cause a relocation)
			return newParent as BPlusTreeIndexNode<TKey, TValue>;
		}


		private void bulkSaveNodes(ref BPlusTreeIndexNode<TKey, TValue> parent, ref BPlusTreeLeafNode<TKey, TValue> prevLeaf, ref BPlusTreeLeafNode<TKey, TValue> leaf, bool isLastLeaf, int maximumKeywordCount) {

			if (prevLeaf == null) {

				// special case -- adding first leaf to the completely empty tree (root has no keywords / childlocations / values)

				if (isLastLeaf) {

					// there's only one leaf total.
					// means the root is also a leaf (no index nodes whatsoever)
                    leaf.Location = Root.Location;
                    Root = (BPlusTreeNode<TKey, TValue>)leaf;
                    leaf.Write();

				} else {

					// Root defaults to being a leaf node (even when completely empty).
					// need to convert it to an index node and attach our leaf node.

					// write out the leaf
                    leaf.Write();

					// create a new root node, attach the leaf to it.  Don't set a keyword (we don't know what it is yet, it's in the next leaf)
					BPlusTreeIndexNode<TKey, TValue> newRoot = null;
					if (Root.IsLeaf) {
						// root is a leaf, needs to be promoted to an index node
						newRoot = ((BPlusTreeLeafNode<TKey, TValue>)Root).PromoteToIndexNode();
					} else {
						newRoot = (BPlusTreeIndexNode<TKey, TValue>)Root;
					}
					newRoot.ChildLocations.Add(leaf.Location);
					leaf.Parent = newRoot;
					parent = newRoot;
					Root = newRoot;
                    Root.Write();

				}

			} else {

				// need to save the current leaf node's data
				// (and the previous leaf node, since its sibling's offset (current leaf node) is changing

				if (prevLeaf != null) {

					// write the leaf so we get a valid Location 
					leaf.Parent = parent;
                    leaf.Write();



					// point previous's right sibling @ current
					if (prevLeaf.RightSiblingLocation != leaf.Location) {
						prevLeaf.RightSiblingLocation = leaf.Location;
                        prevLeaf.Write();
					}

					// remove previous leaf from the cache, as we're not going to touch it anymore
//					NodeCache.Remove(prevLeaf.FileOffset);

				}


                // we don't use the IsFull property here because we have to respect the given fill factor.
                // therefore we don't compare against the FanoutSize (as the node.IsFull property does), we compare against the upper limit imposed by fill factor.
				// if (!parent.IsFull) {
                if (parent.Keywords.Count < maximumKeywordCount){

                    // 2010-02-24 brock@circaware.com
                    // implementation changed from using last keyword of left child as the median keyword to using
                    // first keyword of right child as the median keyword to reduce the number of special cases encountered.
                    // New implementation:
                    parent.Keywords.Add(leaf.FirstKeyword);
                    parent.ChildLocations.Add(leaf.Location);
                    parent.Write();

                    // cont'd from above -- So the following comments and code are no longer valid.

                    //if (isLastLeaf) {
                    //    // last leaf does NOT copy its keyword up on a non-full node -- it's the rightmost child in the entire tree
                    //    // so this parent's last child pointer should point at it, but not contain the keyword.
                    //    //if (parent.Keywords.Count == 0) {
                    //    //    parent.Keywords.Add(leaf.FirstKeyword);
                    //    //} else {
                    //    //    // make sure final keyword points at first word of last leaf (instead of first word of the last leaf's left sibling)
                    //    //    parent.Keywords.Add(leaf.FirstKeyword);
                    //    //    // parent.Keywords[parent.Keywords.Count - 1] = leaf.FirstKeyword;
                    //    //}
                        

                    //    // make sure final keyword points at first word of last leaf (instead of first word of the last leaf's left sibling)
                    //    parent.Keywords.Add(leaf.FirstKeyword);


                    //    parent.ChildLocations.Add(leaf.Location);
                    //    parent.Write();

                    //} else {
                    //    // simply copy up the first keyword in the leaf (and the fileoffset to the leaf)
                    //    parent.Keywords.Add(leaf.FirstKeyword);
                    //    parent.ChildLocations.Add(leaf.Location);


                    //}

				} else {

					// need to get a new parent
                    parent = bulkSplitAsNeeded(parent);
					leaf.Parent = parent;

					// add first keyword and left/right child pointers
					parent.Keywords.Add(leaf.FirstKeyword);
					parent.ChildLocations.Add(leaf.Location);

                    parent.Write();

				}


			}

			prevLeaf = leaf;

			leaf = new BPlusTreeLeafNode<TKey, TValue>(this, parent, null);

		}

		#endregion


		#region IDisposable Members

		public void Dispose() {
            Lock(delegate() {
                if (_writer != null) {

                    // do a final write of header info to make sure
                    // root node, availableslots, etc are persisted
                    writeHeader();

                    _writer.Close();
                    _writer = null;
                }

                if (_reader != null) {
                    _reader.Close();
                    _reader = null;
                }
            });
		}

		#endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "BPlusTree", resourceName, null, defaultValue, substitutes);
        }

	}
}
