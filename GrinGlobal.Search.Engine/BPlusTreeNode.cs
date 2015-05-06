using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using GrinGlobal.Core;
using System.Globalization;
using System.Diagnostics;

namespace GrinGlobal.Search.Engine {
	internal class BPlusTreeNode {

		int _nodeSizeOnDisk;
		public bool IsLeaf;
		public int KeywordCount;
		public long LeftSiblingFileOffset;
		public long RightSiblingFileOffset;
		public long[] ChildrenByteOffsets;
		public string[] Keywords;

		public long FileOffset;

		BPlusTree _tree;

		private delegate void SearchCallback(BPlusTreeNode node, int keywordIndex, object additionalData);

		public BPlusTreeNode() {
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("Root? ")
				.Append(IsRoot ? "Y" : "N")
				.Append(" Leaf? ")
				.Append(IsLeaf ? "Y" : "N")
				.Append(" Keys=")
				.Append(KeywordCount.ToString())
				//				.Append(", ChunkSize=")
				//				.Append(_chunkSize)
				//				.Append(", CurrentSize=")
				//				.Append(CurrentSize)
				.Append(" @ ")
				.Append(FileOffset.ToString().PadRight(5))
				.Append(" ; ")
				.Append(LeftSiblingFileOffset.ToString().PadRight(5))
				.Append(" < ")
				.Append(" > ")
				.Append(RightSiblingFileOffset.ToString().PadRight(5))
				.Append(" ; ");
			for (int i=0; i < KeywordCount; i++) {
				sb.Append(ChildrenByteOffsets[i].ToString())
					.Append(" | ")
					.Append(Keywords[i])
					.Append(" | ");
			}
			sb.Append(ChildrenByteOffsets[KeywordCount].ToString());
			return sb.ToString();
		}

		private int getHeaderSize() {
			// header
			return
				sizeof(bool) // IsLeaf
				+ sizeof(int) // KeywordCount
				+ sizeof(long) // LeftSiblingFileOffset
				+ sizeof(long) // RightSiblingFileOffset
				+ sizeof(long) * (_tree.Fanout + 1) // ChildNodeByteOffsets (we always write all of them)
			;
		}

		private static int __defaultNodeChunkSize;
		public static int GetDefaultNodeChunkSize(BPlusTree tree) {

			if (__defaultNodeChunkSize == 0) {
				__defaultNodeChunkSize = new BPlusTreeNode { _tree = tree }.adjustChunkSize(0);
			}
			return __defaultNodeChunkSize;

		}

		private int _chunkSize;
		internal int ChunkSize {
			get {
				return _chunkSize;
			}
		}
		private int adjustChunkSize(int minimumSize) {
			// header
			int len = getHeaderSize();

			int averageKeywordSizeOnDisk = calcStringSizeOnDisk("".PadRight(_tree.AverageKeywordSize));

			// assume a rough default for the total length of all keywords in a node
			do {
				len += _tree.Fanout * averageKeywordSizeOnDisk;
			} while (len < minimumSize);

			_chunkSize = len;

			return _chunkSize;

		}

		private int calcStringSizeOnDisk(string val) {
			// NOTE: the BinaryWriter/BinaryReader use the most significant bit of the length prefix bytes to determine when the string starts.
			//       so instead of always writing 4 bytes, it writes the length prefix as follows:
			//
			//  string length (in bytes, not chars)   prefix byte count
			//  ===================================   =================
			//  0 - 127 (2^7)                                 1
			//  128 - 16383 (2^14)                            2
			//  16384 - 2097151 (2^21)                        3
			//  2097152 - 268435455 (2^28)                    4
			//  268435456 - uint.MaxValue  (2^32)             5

			//byte num3;
			//int num = 0;
			//int num2 = 0;
			//do {
			//    num3 = this.ReadByte();
			//    num |= (num3 & 0x7f) << num2;
			//    num2 += 7;
			//}
			//while ((num3 & 0x80) != 0);


			// http://www.gamedev.net/community/forums/topic.asp?topic_id=438532
			// http://msdn.microsoft.com/en-us/library/system.io.binaryreader.readstring.aspx

			// TODO: calc prefix here.  the uint fudging is causing problems with abandoned nodes trying to relocate during creation...
			if (String.IsNullOrEmpty(val)) {
				// one byte, length is zero.
				return 1;
			} else {
				int byteCount = _tree.Encoding.GetByteCount(val);
				if (byteCount < 128) { // 2^(8-1)
					// 1 byte is used to denote prefix for length of string
					return 1 + byteCount;
				} else if (byteCount < 16384) { // 2^(16-2)
					// 2 bytes are used to denote prefix for length of string
					return 2 + byteCount;
				} else if (byteCount < 2097151) { // 2^(24-3)
					// 3 bytes are used to denote prefix for length of string
					return 3 + byteCount;
				} else if (byteCount < 268435456) { // 2^(32-4)
					// 4 bytes are used to denote prefix for length of string
					return 4 + byteCount;
				} else { // all others longer than 2^28
					// 5 bytes are used to denote prefix for length of string
					// this is needed since we ignore the most significant bit of each byte...leaves us 4 bits short to represent the maximum length of a string (2^32)
					// So this 5th byte is always <=15 (0x0F)
					return 5 + byteCount;
				}
			}
		}

		internal int CurrentSize {
			get {

				int len = getHeaderSize();

				// keywords
				for (int i=0; i < _tree.Fanout; i++) {
					// note the BinaryWriter precedes writing a string with a a set of 1-5 bytes representing its total length (determined by the calcStringSizeOnDisk() method)
					// also note we always write everything in the specified encoding (defaults to UTF8)
					len += calcStringSizeOnDisk(Keywords[i]);

				}

				return len;
			}

		}

		// here's the vernacular:
		// The _keywords array represents all the keywords in this node.
		// The _byteRanges array represents all the file offsets in this node for either (a) a keyword or (b) the data itself.
		//   a. If it is located in an index node (i.e. non-Leaf), the byte range represents the start/end offsets in the index file (used for traversing to other nodes)
		//   b. If it is located in a Leaf node, the byte range represents the start/end offsets in the data file (for looking up associated hit(s))


		// Think ByteRange = pointer to next node or data, Keyword = item we're searching by
		// So we always have 1 more pointer (or byterange, BR) than search item (or keyword, K):
		// 
		// |-------- Node ---------|
		// |      go       pi      |
		// |  /   |    |   |    \  |
		// | BR1  K1  BR2  K2  BR3 |
		// |-----------------------|


		/// <summary>
		/// Creates a new root node for the tree and makes the original root the first child of the new root.
		/// </summary>
		/// <param name="tree"></param>
		/// <returns></returns>
		internal static BPlusTreeNode ReplaceRoot(BPlusTree tree, BPlusTreeNode originalRoot, string keyword, long dataByteOffset) {


			// NOTE: This is a very special case.
			// replacing the root involves creating 2 new nodes:
			//  1. the new root node
			//  2. splitting the existing root node into 2 nodes, one of which is a new one

			BPlusTreeNode newRoot = null;

			// create a new root node, mark it as not a leaf, put it in exact same spot as original root
			newRoot = new BPlusTreeNode {
				IsLeaf = false,
				_tree = tree,
				Keywords = new string[tree.Fanout],
				ChildrenByteOffsets = new long[tree.Fanout + 1],
				KeywordCount = 0,
				FileOffset = -1,
				LeftSiblingFileOffset = -1,
				RightSiblingFileOffset = -1,
			};

			tree.Log("------------ BEGIN Replacing root ------------");
			tree.Log("BEGIN Replacing root. original node -> ".PadRight(50) + originalRoot.ToString());
			tree.Log("BEGIN Replacing root. new node -> ".PadRight(50) + newRoot.ToString());


			// add our new root to file, update the root node pointer
			BPlusAbandonedNode abandoned = tree.GetNextAvailableNodeLocation(originalRoot);
			newRoot.FileOffset = abandoned.FileOffset;
			newRoot.Write(null);
			tree.WriteRootNodeOffset(newRoot.FileOffset);


			// trickle-down insert the new keyword/data
			if (!String.IsNullOrEmpty(keyword)) {

				// split the original
				BPlusTreeNode newChild = newRoot.SplitChild(0, originalRoot, null);

				tree.Log("CONTINUE Replacing root. new child node -> ".PadRight(50) + originalRoot.ToString());


				if (newRoot.Insert(keyword, dataByteOffset, null)) {
					// the new root was relocated during insertion (outgrew its allotted space)
					// the insert should take care of it

					// re-read it in just in case
					newRoot = BPlusTreeNode.Read(tree, newRoot.FileOffset, true);

				}
			} else {
				newRoot = BPlusTreeNode.Read(tree, newRoot.FileOffset, true);
			}


			tree.Log("END Replacing root. original node -> ".PadRight(50) + originalRoot.ToString());
			//			_tree.Log("END Replacing root. new child node -> ".PadRight(50) + newChild.ToString());
			tree.Log("END Replacing root. new root -> ".PadRight(50) + newRoot.ToString());
			tree.Log("------------ END Replacing root ------------");

			return newRoot;
		}

		/// <summary>
		/// Creates a node for the tree.  Does not write to file, just initializes the Node with appropriate values
		/// </summary>
		/// <param name="tree"></param>
		/// <returns></returns>
		internal static BPlusTreeNode CreateNode(BPlusTree tree) {
			BPlusTreeNode newChild = new BPlusTreeNode {
				IsLeaf = true,
				_tree = tree,
				Keywords = new string[tree.Fanout],
				ChildrenByteOffsets = new long[tree.Fanout + 1],
				KeywordCount = 0,
				LeftSiblingFileOffset = -1,
				RightSiblingFileOffset = -1,
				FileOffset = -1
			};
			for (int i=0; i < tree.Fanout; i++) {
				newChild.Keywords[i] = string.Empty;
			}
			return newChild;
		}

		internal void ZeroFill() {
			byte[] bytes = new byte[_chunkSize];
			_tree.Writer.BaseStream.Position = FileOffset;
			_tree.Writer.BaseStream.Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Reads node from file at given byteOffset.  If byteOffset is &lt; 1, returns null.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="byteOffset"></param>
		/// <param name="throwExceptionOnNull">Throws an exception if the node is not located in the file</param>
		/// <returns></returns>
		internal static BPlusTreeNode Read(BPlusTree tree, long byteOffset, bool throwExceptionOnNull) {

			BPlusTreeNode node = null;

			if (byteOffset > 0) {

				// try to pull node from cache first
				if (!tree.NodeCache.TryGetValue(byteOffset, out node)) {

					// node wasn't cached. actually read it from the file.
					lock (tree.LockForFileAccess) {
						tree.Reader.BaseStream.Position = byteOffset;
						node = new BPlusTreeNode {
							FileOffset = tree.Reader.BaseStream.Position,
							_tree = tree,

							IsLeaf = tree.Reader.ReadBoolean(),
							KeywordCount = tree.Reader.ReadInt32(),
							LeftSiblingFileOffset = tree.Reader.ReadInt64(),
							RightSiblingFileOffset = tree.Reader.ReadInt64(),


							ChildrenByteOffsets = new long[tree.Fanout + 1],
							Keywords = new string[tree.Fanout]
						};


						// note we pull in all the byteoffsets first, then all the keywords
						for (int i=0; i <= tree.Fanout; i++) {
							node.ChildrenByteOffsets[i] = tree.Reader.ReadInt64();
						}
						// this is the only variable-length portion. luckily ReadString() does the nasty work for us.
						for (int i=0; i < tree.Fanout; i++) {
							node.Keywords[i] = tree.Reader.ReadString();
						}

						node._nodeSizeOnDisk = (int)(tree.Reader.BaseStream.Position - byteOffset);
						node.adjustChunkSize(node._nodeSizeOnDisk);
					}
				}
			}

			if (node == null && throwExceptionOnNull) {
				throw new InvalidOperationException("No node was found at fileoffset = " + byteOffset);
			}

			return node;
		}

		private bool IsRoot {
			get {
				return _tree.IsRoot(this);
			}
		}

		private void relocate(BPlusTreeNode parentNode) {

			// this node has outgrown its allotted space.
			// we need to move it to the end of the file and update everybody who points to it

			// so have to update the following:
			// 1. Parent node's pointer
			// 2. Left sibling's pointer
			// 3. Right sibling's pointer
			// 4. All children's parent pointer (which points at this node)

			_tree.Log("------------ BEGIN Relocating node ------------");
			_tree.Log("BEGIN Relocating node -> ".PadRight(50) + ToString());

			// get an abandoned node (or the end of the file, whichever comes first)
			BPlusAbandonedNode abandonedNode = _tree.GetNextAvailableNodeLocation(this);

			// mark this node as abandoned in the file (and remove from cache)
			_tree.AbandonNode(this);

			//if (this.IsRoot) {
			//    // we're relocating the root.
			//    _tree.Log("TODO: what to do here, if anything?");
			//}

			if (!IsLeaf) {
				// only leaf nodes should track left/right siblings.
				LeftSiblingFileOffset = -1;
				RightSiblingFileOffset = -1;
			}

			if (LeftSiblingFileOffset > -1) {
				BPlusTreeNode originalLeftSibling = BPlusTreeNode.Read(_tree, this.LeftSiblingFileOffset, true);
				originalLeftSibling.RightSiblingFileOffset = originalLeftSibling.IsLeaf ? abandonedNode.FileOffset : -1;
				_tree.Log(("Updating left sibling node @ " + originalLeftSibling.FileOffset + " -> ").PadRight(50) + originalLeftSibling.ToString());
				originalLeftSibling.Write(null);
			}
			if (RightSiblingFileOffset > -1) {
				BPlusTreeNode originalRightSibling = BPlusTreeNode.Read(_tree, this.RightSiblingFileOffset, true);
				originalRightSibling.LeftSiblingFileOffset = originalRightSibling.IsLeaf ? abandonedNode.FileOffset : -1;
				_tree.Log(("Updating right sibling node @ " + originalRightSibling.FileOffset + " -> ").PadRight(50) + originalRightSibling.ToString());
				originalRightSibling.Write(null);
			}

			if (this.IsRoot) {
				// special case!  no parent means this is the root node.
				// update the int at the very beginning of the file to point at the new root node
				_tree.WriteRootNodeOffset(abandonedNode.FileOffset);
			} else {

				if (parentNode != null) {
					for (int i = 0; i < parentNode.KeywordCount + 1; i++) {
						if (parentNode.ChildrenByteOffsets[i] == FileOffset) {
							parentNode.ChildrenByteOffsets[i] = abandonedNode.FileOffset;
							_tree.Log(("Updating parent node @ " + parentNode.FileOffset + " -> ").PadRight(50) + parentNode.ToString());
							parentNode.Write(null);
							// the Root node is cached in the tree object -- udpate it if need be
							if (parentNode.IsRoot) {
								_tree.Root = parentNode;
							}
							break;
						}
					}
				}
			}


			_tree.Writer.BaseStream.Position = abandonedNode.FileOffset;
			FileOffset = abandonedNode.FileOffset;
			//_chunkSize = abandonedNode.ByteCount;

			_tree.Log("END Relocating node (not written yet) -> ".PadRight(50) + ToString());
			_tree.Log("------------ END Relocating node ------------");

		}

		/// <summary>
		/// Returns true if the node was relocated during writing
		/// </summary>
		/// <returns></returns>
		internal bool Write(BPlusTreeNode parent) {

			bool relocated = false;

			// NOTE: BPlusTree handles all concurrency / multithreaded issues surrounding writing data -- so none of it is considered here
			//       Since it requires all other reads and writes to block until this write is done, we do not have to worry about
			//       the file pointer being repositioned in the middle of a write


			// remember the start byte offset for later (note 0 is not a valid position for a node in our particular file, so it's ok to use it as an invalid value)
			if (FileOffset < 1) {
				// new node, tack on end of file (or in an abandoned node)
				BPlusAbandonedNode abandonedNode = _tree.GetNextAvailableNodeLocation(this);
				FileOffset = abandonedNode.FileOffset;
				//				_chunkSize = abandonedNode.ByteCount;
			}


			// if a node has never been written it's size will be 0.
			// this is also true during a SplitChild write, but since by definition we're splitting a node, it must already have
			// at least enough space to store 1/2 of its previous keywords -- so it will never require to be relocated. 
			if (_nodeSizeOnDisk > 0) {
				// since keywords are dynamic in length and the node must be a specific
				// size in the file, we need to check if it will overflow the allotted node size
				int newSize = CurrentSize;
				if (newSize > _nodeSizeOnDisk) {
					// it grew since we read it. if it's over the default, we have to move it.
					if (newSize > _chunkSize && (FileOffset + _chunkSize) < _tree.Writer.BaseStream.Length) {
						// it won't fit -- we have to move it (and we're not at the end of the file already)
						relocate(parent);
						relocated = true;
					}
				}
			}

			_tree.Log("Writing node -> ".PadRight(50) + this.ToString());

			_tree.Writer.BaseStream.Position = FileOffset;
			_tree.Writer.Write(IsLeaf);
			_tree.Writer.Write(KeywordCount);
			_tree.Writer.Write(LeftSiblingFileOffset);
			_tree.Writer.Write(RightSiblingFileOffset);

			// write out all byte offsets first (we always do all regardless of KeywordCount)
			for (int i=0; i <= _tree.Fanout; i++) {
				_tree.Writer.Write(ChildrenByteOffsets[i]);
			}
			// the keyword portion is the only variable length area there is.
			// 
			for (int i=0; i < _tree.Fanout; i++) {
				if (String.IsNullOrEmpty(Keywords[i])) {
					_tree.Writer.Write("");
				} else {
					_tree.Writer.Write(Keywords[i]);
				}
			}

			// we also must write filler bytes to fill up to the chunksize
			int writtenByteCount = (int)(_tree.Writer.BaseStream.Position - FileOffset);
			adjustChunkSize(writtenByteCount);
			int fillerByteCount = _chunkSize - writtenByteCount;
			if (fillerByteCount > 0) {
				_tree.Writer.BaseStream.Write(new byte[fillerByteCount], 0, fillerByteCount);
			}

			_nodeSizeOnDisk = (int)(_tree.Writer.BaseStream.Position - FileOffset);



			// update the cache as needed
			_tree.UpdateNodeCache(this);


			return relocated;

		}

		public bool IsFull {
			get {
				return KeywordCount == _tree.Fanout;
			}
		}

		internal bool Insert(string keyword, long dataByteOffset, BPlusTreeNode parentNode) {
			//i <- n[x]
			//if leaf[x]
			//     then while i >= 1 and k < keyi[x]
			//            do keyi+1[x] <- keyi[x]
			//               i <- i - 1
			//          keyi+1[x] <- k
			//          n[x] <- n[x] + 1
			//          Disk-Write(x)
			//     else while i >= and k < keyi[x]
			//            do i <- i - 1
			//          i <- i + 1
			//          Disk-Read(ci[x])
			//          if n[ci[x]] = 2t - 1
			//               then B-Tree-Split-Child(x, i, ci[x])
			//                    if k > keyi[x]
			//                       then i <- i + 1
			//          B-Tree-Insert-Nonfull(ci[x], k)  

			int i = KeywordCount - 1;
			if (IsLeaf) {
				while (i > -1 && String.Compare(keyword, Keywords[i], false) < 0) {
					Keywords[i + 1] = Keywords[i];
					ChildrenByteOffsets[i + 1] = ChildrenByteOffsets[i];
					i--;
				}
				Keywords[i + 1] = keyword;
				ChildrenByteOffsets[i + 1] = dataByteOffset;
				KeywordCount++;
				return Write(parentNode);
			} else {
				while (i > -1 && String.Compare(keyword, Keywords[i], false) < 0) {
					i--;
				}
				i++;
				BPlusTreeNode child = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);

				_tree.Log(("Inspecting node @ " + child.FileOffset + " -> ").PadRight(50) + child.ToString());
				if (!child.IsFull) {
					return child.Insert(keyword, dataByteOffset, this);
				} else {
					// must first split the child
					BPlusTreeNode newChild = SplitChild(i, child, parentNode);
					if (String.Compare(keyword, newChild.Keywords[0], false) < 0) {
						// new keyword belongs somewhere below the original child
						return child.Insert(keyword, dataByteOffset, this);
					} else {
						// new keyword belongs somewhere below the new child
						return newChild.Insert(keyword, dataByteOffset, this);
					}
				}
			}


		}

		internal BPlusTreeNode SplitChild(int childOffset, BPlusTreeNode originalChild, BPlusTreeNode grandParent) {

			// Split the current node into two nodes:
			//   a. Left node contains all pointers/keywords to the left of the median
			//   b. Right node contains all pointers/keywords to the right of the median
			//   c. Parent node gets median keyword pushed into it
			//
			// Disk-wise: the originalChild is always written back to its original position.
			//            the newChild is placed into either an available abandoned node or tacked on the end of the file.
			//            the parent node is written back to its original position IF IT FITS.  Since we're coyping a keyword
			//               up into the parent node, it may cause it to outgrow its allocated size in the file.  If this occurs,
			//               the call to this.Write() implicitly calls relocate().  relocate() will then either write the parent to
			//               an abandoned node or tack it onto the end of the file.

			_tree.Log("------------ BEGIN Splitting node ------------");
			_tree.Log("BEGIN Splitting node parent -> ".PadRight(50) + ToString());
			_tree.Log("BEGIN Splitting node child -> ".PadRight(50) + originalChild.ToString());

			BPlusTreeNode newChild = BPlusTreeNode.CreateNode(_tree);
			newChild.IsLeaf = originalChild.IsLeaf;

			newChild.KeywordCount = _tree.Fanout - _tree.MinimumChildren;

			// assign items left of median
			for (int i = 0; i < newChild.KeywordCount; i++) {
				// Left half of original child already has appropriate values
				// However, left half of new child needs data from right half of original child
				newChild.ChildrenByteOffsets[i] = originalChild.ChildrenByteOffsets[i + _tree.Median + 1];
				newChild.Keywords[i] = originalChild.Keywords[i + _tree.Median + 1];
			}
			newChild.ChildrenByteOffsets[newChild.KeywordCount] = originalChild.ChildrenByteOffsets[_tree.Fanout];

			// adjust the new child's relative info as needed (we always add to the right)
			if (originalChild.IsLeaf) {
				// only leaves should track siblings
				newChild.LeftSiblingFileOffset = originalChild.FileOffset;
				newChild.RightSiblingFileOffset = originalChild.RightSiblingFileOffset;
			} else {
				newChild.LeftSiblingFileOffset = -1;
				newChild.RightSiblingFileOffset = -1;
			}


			// make room in parent for the median values
			// (copy all those above the childOffset over one)

			for (int i=KeywordCount; i > childOffset; i--) {
				ChildrenByteOffsets[i + 1] = ChildrenByteOffsets[i];
				Keywords[i] = Keywords[i - 1];
			}
			KeywordCount++;

			Keywords[childOffset] = originalChild.Keywords[_tree.Median];
			// just set byterange that points at our new child to the default for now.  we'll assign it the proper value later (once we know the true file offsets)
			ChildrenByteOffsets[childOffset + 1] = 0;

			// if we're splitting a leaf node, we must copy up the median value.
			// if we're splitting an index node, we must simply push it up (i.e. do not leave a copy in the original child)
			int startErasingAt = _tree.MinimumChildren;
			if (!originalChild.IsLeaf){
				// index node, erase the keyword (as we essentially want to push it up)
				startErasingAt--;
			}

			// assign items right of median (note this effectively means we copy up the median value, not move it up)
			for (int i=startErasingAt; i < _tree.Fanout; i++) {
				// Left half of newChild already has proper values (nulls/defaults)
				// However, right half of originalChild still has the values we just copied to the left half of newChild.
				// null/default those values.
				if (originalChild.IsLeaf || i > startErasingAt) {
					originalChild.ChildrenByteOffsets[i] = 0;
				}
				originalChild.Keywords[i] = String.Empty;
			}
			// there's always one more child than keyword...
			originalChild.ChildrenByteOffsets[_tree.Fanout] = 0;
			originalChild.KeywordCount = startErasingAt;

			// determine where to put this new child (in an abandoned node or tack onto end of file)
			BPlusAbandonedNode abandonedNode = _tree.GetNextAvailableNodeLocation(newChild);
			//newChild._chunkSize = abandonedNode.ByteCount;
			newChild.FileOffset = abandonedNode.FileOffset;

			newChild.Write(this);


			// the original child's right sibling must be updated to point at the new child instead
			if (originalChild.RightSiblingFileOffset > 0 && originalChild.IsLeaf) {
				BPlusTreeNode originalRightSibling = BPlusTreeNode.Read(_tree, originalChild.RightSiblingFileOffset, true);
				originalRightSibling.LeftSiblingFileOffset = newChild.FileOffset;
				originalRightSibling.Write(null);
			}



			// now that we've written the new child out, 
			// we know what to set the originalChild's right sibling to...
			if (originalChild.IsLeaf) {
				originalChild.RightSiblingFileOffset = newChild.FileOffset;
			}
			originalChild.Write(this);


			// we now know all the offsets we need to update in the parent...
			ChildrenByteOffsets[childOffset] = originalChild.FileOffset;
			ChildrenByteOffsets[childOffset + 1] = newChild.FileOffset;

			// this node may relocate because we're adding a keyword to it.
			// if it does, we have to be sure our direct parent knows about it (so it can update its child file offset that points to us)
			// hence the "grandParentNode".  Write() handles updating things correctly if we give it our parent (which is our children's grandparent) :)
			if (Write(grandParent)) {
				//Debug.WriteLine("would have missed this before");
			}

			_tree.Log("END Splitting node parent -> ".PadRight(50) + ToString());
			_tree.Log("END Splitting node. original child -> ".PadRight(50) + originalChild.ToString());
			_tree.Log("END Splitting node. new child -> ".PadRight(50) + newChild.ToString());
			_tree.Log("------------ END Splitting node ------------");

			return newChild;



			/*
			newChild <- Allocate-Node()
			leaf[newChild] <- leaf[parent]
			n[newChild] <- t - 1
			for j <- 1 to t - 1
				 do keyj[newChild] <- keyj+t[parent]
			if not leaf[parent]
				 then for j <- 1 to t
					  do cj[newChild] <- cj+t[parent]
			n[parent] <- t - 1
			for j <- n[originalChild] + 1 downto i + 1
				 do cj+1[originalChild] <- cj[originalChild]
			ci+1 <- newChild
			for j <- n[originalChild] downto i
				 do keyj+1[originalChild] <- keyj[originalChild]
			keyi[originalChild] <- keyt[parent]
			n[originalChild] <- n[originalChild] + 1
			Disk-Write(parent)
			Disk-Write(newChild)
			Disk-Write(originalChild)
			 * 
			*/
		}

		internal void Update(string keyword, long dataByteOffset, BPlusTreeNode parentNode) {
			// when the leaf node containing the exact keyword is found, updates the appropriate ChildrenByteOffset
			performSearch(keyword, KeywordMatchMode.ExactMatch, false, foundNodeToUpdate, dataByteOffset);
		}

		private void foundNodeToUpdate(BPlusTreeNode node, int keywordOffset, object additionalData) {
			node.ChildrenByteOffsets[keywordOffset] = (long)additionalData;
			// we know it won't split or relocate so we don't care about the parent...
			node.Write(null);
		}


		internal void Search(string keyword, KeywordMatchMode matchMode, bool ignoreCase, List<KeywordMatch> results) {
			// when a hit is found, calls searchHit passing the results so it may fill it
			performSearch(keyword, matchMode, ignoreCase, foundNodeInSearch, results);
		}

		private void foundNodeInSearch(BPlusTreeNode node, int keywordOffset, object additionalData) {
			List<KeywordMatch> results = (List<KeywordMatch>)additionalData;
			results.Add(new KeywordMatch { Keyword = node.Keywords[keywordOffset], Value = node.ChildrenByteOffsets[keywordOffset] });
		}


		/// <summary>
		/// Performs a search for the given keyword using the given matchMode/ignoreCase.  For each item that matches, the callback is called, passing the current node, keyword offset, and the given additionalData with it.  This allows us to reuse the somewhat complex search code for searching and updating.
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="matchMode"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="callback"></param>
		/// <param name="additionalData"></param>
		private void performSearch(string keyword, KeywordMatchMode matchMode, bool ignoreCase, SearchCallback callback, object additionalData) {

			int i=0;
			int compared = -1;

			// NOTE: We have the switch and as many of the if statements outside the loops as we can so we minimize branching within the loops.
			//       We'll be spinning through possibly millions of times so we're sacrificing cleaner, more intuitive code for performance here

			switch (matchMode) {
				case KeywordMatchMode.ExactMatch:
					// exact match.  using String.Compare instead of == allows us to jump out as soon as
					// we find a string "greater than" the search keyword.  If we just used ==, we'd have to always compare the entire
					// set of _keywords in this node if none was ever found.  Since once we find a match we stop, it's okay to put the IsLeaf check within the loop.

					// ExactMatch, regardless of Leafiness, regardless of case sensitivity
					while (i < KeywordCount && compared < 1) {
						compared = String.Compare(Keywords[i], keyword, ignoreCase, CultureInfo.CurrentUICulture);
						if (compared == 0) {
							// found our exact match
							if (IsLeaf) {
								callback(this, i, additionalData);

								// there may be multiple entries for a given keyword (unlikely, but possible)
								// so do NOT jump out.

							} else {
								// continue searching the associated child node...
								BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
								node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
								// since this node is not a leaf, this means a given keyword will not appear more than once.
								// so once we drill down, we're done.
								return;
							}

						} else if (compared > 0) {
							// this keyword is bigger than ours -- i.e. we know it's not in this node.
							// if we're not a leaf, drill down...
							if (!IsLeaf) {
								// continue searching the associated child node...
								BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
								node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
							}
							// we've already passed where it could be an exact match in this node. jump out.
							return;
						} else {
							// this keyword is "less than" our search keyword.  keep looking in this node.
						}
						i++;
					}
					if (!IsLeaf && i == KeywordCount) {
						// we ran out of keywords to compare against -- means
						// all keywords in this node are "less than" than our search keyword.
						// if there's a right child, inspect it.
						BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
						node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
					}
					break;
				case KeywordMatchMode.StartsWith:

					if (IsLeaf) {

						// StartsWith, IsLeaf, regardless of case sensitivity (case sensitivity is handled by comparisonType variable)

						while (i < KeywordCount) {
							if (Keywords[i].StartsWith(keyword, ignoreCase, CultureInfo.CurrentUICulture)) {
								callback(this, i, additionalData);
								// since this is a startswith (i.e. fuzzy on right)
								// we should just inspect the entire node (do not jump out)
							}
							i++;
						}
					} else {

						// StartsWith, Is NOT Leaf, regardless of case sensitivity (case sensitivity is handled by comparisonType variable)

						while (i < KeywordCount) {
							if (Keywords[i].StartsWith(keyword, ignoreCase, CultureInfo.CurrentUICulture)) {
								BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
								node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
								// since this is a startswith (i.e. fuzzy on right)
								// we should just inspect the entire node (do not jump out)
							} else if (String.Compare(Keywords[i], keyword, ignoreCase, CultureInfo.CurrentUICulture) > 0) {
								// this keyword doesn't start with our search keyword, but it is "greater than" it.
								// continue searching children
								BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
								node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
								return;
							}
							i++;
						}
						if (i == KeywordCount && ChildrenByteOffsets[i] > 0) {
							// we ran off the end. inspect rightmost child.
							BPlusTreeNode node = BPlusTreeNode.Read(_tree, ChildrenByteOffsets[i], true);
							node.performSearch(keyword, matchMode, ignoreCase, callback, additionalData);
							return;
						}
					}
					break;

				case KeywordMatchMode.EndsWith:

					// Since our index is built by the beginning of the word, we essentially have to do an index scan.
					// we'll use the tree's TraverseLeaves() method to do this efficiently.

					foreach (BPlusTreeNode node in _tree.TraverseLeaves()) {
						for (int j=0; j < node.KeywordCount; j++) {
							if (node.Keywords[j].EndsWith(keyword, ignoreCase, CultureInfo.CurrentUICulture)) {

								// this does end with what we're looking for...
								callback(node, j, additionalData);

								// since this is a endswith (i.e. fuzzy on left)
								// we should just inspect the entire node (do not jump out)
							}
						}
					}


					break;

				case KeywordMatchMode.Contains:

					// Since our index is built by the beginning of the word, we essentially have to do an index scan.
					// we'll use the tree's TraverseLeaves() method to do this efficiently.

					if (ignoreCase) {

						// Contains, Case Insensitive, regardless of leafiness

						keyword = keyword.ToLower();
						foreach (BPlusTreeNode node in _tree.TraverseLeaves()) {
							for (int j=0; j < node.KeywordCount; j++) {
								if (node.Keywords[j].ToLower().Contains(keyword)) {
									// this does contain what we're looking for...
									callback(node, j, additionalData);
									// since this is a endswith (i.e. fuzzy on left)
									// we should just inspect the entire node (do not jump out)
								}
							}
						}

					} else {

						// Contains, Case Sensitive, regardless of leafiness

						foreach (BPlusTreeNode node in _tree.TraverseLeaves()) {
							for (int j=0; j < node.KeywordCount; j++) {
								if (node.Keywords[j].Contains(keyword)) {
									// this does contain what we're looking for...
									callback(node, j, additionalData);
									// since this is a endswith (i.e. fuzzy on left)
									// we should just inspect the entire node (do not jump out)
								}
							}
						}
					}
					break;
			}

		}
	}
}
