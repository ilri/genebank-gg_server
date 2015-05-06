using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {

    /// <summary>
    /// Base class for all nodes stored in a BPlusTree object.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
	public abstract class BPlusTreeNode<TKey, TValue> where TKey : ISearchable<TKey>, new() where TValue : IPersistable<TValue>, new() {

		#region Properties

		public List<TKey> Keywords { get; set; }
		public TKey FirstKeyword {
			get {
				if (Keywords.Count == 0) {
					return default(TKey);
				} else {
					return Keywords[0];
				}
			}
		}
		public TKey LastKeyword {
			get {
				if (Keywords.Count > 0) {
					return Keywords[Keywords.Count - 1];
				} else {
					return default(TKey);
				}
			}
		}
		public TKey MedianKeyword {
			get {
				if (Keywords.Count > this.Tree.Median) {
					return Keywords[this.Tree.Median - 1];
				} else {
					return default(TKey);
				}
			}
		}
		public bool IsLeaf { get; protected set; }

        private int _slotSize;
        internal int SlotSize {
            get {
                return _slotSize;
            }
            set {
                _slotSize = value;
            }
        }
       
		internal long Location { get; set; }
		public BPlusTree<TKey, TValue> Tree { get; private set; }
		public bool IsFull {
			get {
				return this.Keywords.Count == this.Tree.FanoutSize;
			}
		}

		protected internal BPlusTreeIndexNode<TKey, TValue> Parent;

		public bool IsRoot {
			get {
                if (this.Tree.Root == null) {
                    // only happens during intial startup (when we're first reading the root node from disk)
                    return true;
                } else {
                    return this.Tree.Root.Location == this.Location;
                }
			}
		}

		#endregion Properties

		#region Persistance
		private static BPlusTreeNode<TKey, TValue> processRead(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent, BinaryReader rdr, object[] args) {

			// stream has been positioned appropriately.
			// just read from it.

            BPlusTreeNode<TKey, TValue> node = null;
            tree.Lock(delegate() {
                long startLocation = rdr.BaseStream.Position;
                int slotSize = rdr.ReadInt32();
                bool leaf = rdr.ReadBoolean();
                short keywordCount = rdr.ReadInt16();

                if (leaf) {
                    node = new BPlusTreeLeafNode<TKey, TValue>(tree, parent, rdr);
                } else {
                    node = new BPlusTreeIndexNode<TKey, TValue>(tree, parent, rdr);
                }
                node.Location = startLocation;

                // now we must read in all the keywords (they're always at the end of the stream)
                for (short i = 0; i < keywordCount; i++) {
                    TKey keyManager = new TKey();
                    keyManager.Read(rdr);
                    node.Keywords.Add(keyManager);
                }

                node.SlotSize = slotSize;

                // orient the stream pointer to the end of the node (which may be past the current data in it)
                rdr.BaseStream.Position = startLocation + node.SlotSize;

            });

            return node;

		}

		/// <summary>
		/// Factory method for reading data from the tree's stream and returning either a BPlusTreeIndexNode or BPlusTreeLeafNode object
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="location"></param>
		/// <returns></returns>
        internal static BPlusTreeNode<TKey, TValue> Read(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent, long location) {

			if (location < 1) {
				return null;
			}

            BPlusTreeNode<TKey, TValue> node = null;
            
            if (!tree.NodeCache.TryGetValue(location, out node)){
                node = tree.ReadNodeFromDisk(parent, processRead, location, null);
            }
			return node;

		}


		internal void MoveTo(NodeSlot newSlot) {

            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin moving node from " + this.Location + " to " + newSlot.Location);
                Tree.Lock(delegate() {
                    // notify parent, if any, that we're moving
                    if (Parent != null) {
                        Parent.ChildIsMoving(this, newSlot.Location);
                    }

                    // let specific instance of this node know it should do its moving code
                    movingTo(newSlot.Location);

                    // finally move it
                    this.Location = newSlot.Location;
                });
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End moving node");
            }
		}

		protected virtual BPlusTreeNode<TKey, TValue> SplitChild(int childIndex, BPlusTreeNode<TKey, TValue> originalChild) {

			// create a new child
			// take 1/2 keywords and children from original child and give to new child
			// push (for index node) or copy (for leaf node) median value into parent

			return null;

		}

		protected virtual void movingTo(long newLocation) {
			// do nothing in default implementation
			// i.e. ignored by index nodes
		}

		protected virtual void ChildIsMoving(BPlusTreeNode<TKey, TValue> child, long newLocation) {
			// do nothing in default implementation
			// i.e. ignored by leaf nodes
		}


        /// <summary>
        /// Writes node to disk.  Returns true if node was written to a different location than it was read from (i.e. Location property changed during write)
        /// </summary>
        /// <returns></returns>
		internal bool Write() {

			// tell tree we want to write, give it the callback
            return Tree.WriteNodeToDisk(processWrite, null);

        }

        internal void WriteSlotSize(BinaryWriter wtr) {
            wtr.Write(SlotSize);
        }

		private BPlusTreeNode<TKey, TValue> processWrite(BinaryWriter wtr, object[] args) {

			// write header info
            WriteSlotSize(wtr);
			wtr.Write(IsLeaf);
			wtr.Write((short)Keywords.Count);

			// write instance-specific data (aka values and whatever metadata they need)
			write(wtr);

			// write keywords
			foreach (TKey keyManager in Keywords) {
				keyManager.Write(wtr);
			}
            wtr.Flush();
			return this;
        }




		protected internal abstract void write(BinaryWriter wtr);

		#endregion

		#region Instantiation
		internal BPlusTreeNode(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent) {
			Tree = tree;
			Keywords = new List<TKey>();
			Parent = parent;
			IsLeaf = false;
		}

		#endregion Instantiation



		#region Operations

		protected int GetIndexInParent(out int totalChildren) {
			if (Parent == null) {
				totalChildren = -1;
				return -1;
			} else {
				TKey ourKey = FirstKeyword;
				totalChildren = Parent.ChildLocations.Count;
				for (int i=0; i < Parent.Keywords.Count; i++) {
					TKey parentKeyword = Parent.Keywords[i];
					int compared = parentKeyword.CompareTo(ourKey, KeywordMatchMode.ExactMatch, false);
					if (compared == 0) {
						// found the index. since we store the first keyword of a node as the splitter keyword,
						// we return the 'next' childlocation.
						return i+1;
					} else if (compared > 0 && i == 0) {
						// must be the leftmost child, as the first parent keyword is > the child's first keyword.
						return 0;
					}
				}

				// we never found the index. assume it's the right most (as that's all that is remaining)
				return Parent.ChildLocations.Count;
			
			}


		}


		protected BPlusTreeNode<TKey, TValue> GetLeftSibling(bool mustHaveSameParent) {
			// look in parent for our smallest keyword, grab childlocation to the left of it

			int parentChildCount;
			int childLocationIndex = GetIndexInParent(out parentChildCount);

			if (childLocationIndex == -1) {
				// never found the index, or no parent (aka root). No sibling to return.
				return null;
			} else {
				if (childLocationIndex == 0) {
					if (mustHaveSameParent) {
						// caller wants the sibling only if it's from the same parent, and it's not.
						return null;
					} else {
						// we have to find the parent of our left sibling.
						// spin up until we find the first node whose child index is > 0
						// then drill down the rightmost path until we're at the same depth as we started at
						int i = 0;
						int nodeChildLocationIndex = -1;
						BPlusTreeNode<TKey, TValue> node = this;
						while (node.Parent != null) {

							i++;

							// keep bubbling up until we're at a node whose parent has at least one child to our left.
							nodeChildLocationIndex = node.GetIndexInParent(out parentChildCount);
							if (nodeChildLocationIndex > 0) {
								// this node is not the leftmost child of its parent.
								// we're done bubbling (node contains the ancestor node, nodeChildLocationIndex can tell us the subtree to search down)
								break;
							} else {
								node = node.Parent;
							}
						}

						if (nodeChildLocationIndex == 0) {
							// the only common ancestor has our node being the leftmost node in the tree (at this depth).
							return null;
						}

						// ok, node contains the ancestor somehwere up the tree that has the node we're looking for as its 
						// rightmost descendent at the same depth.
						// So we just keep drilling down until we're at the same depth.
						node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, node as BPlusTreeIndexNode<TKey, TValue>, nodeChildLocationIndex-1);
						while (i > 0) {
							BPlusTreeIndexNode<TKey, TValue> indexNode = node as BPlusTreeIndexNode<TKey, TValue>;
                            node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, indexNode, indexNode.LastChildLocation);
							i--;
						}

						// node now contains the left sibling!
						return node;

					}
				} else {
					// we know the left sibling is attached to the same parent.
                    BPlusTreeNode<TKey, TValue> node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, this.Parent, Parent.ChildLocations[childLocationIndex - 1]);
					return node;
				}
			}
		}

        protected BPlusTreeNode<TKey, TValue> GetRightSibling(bool mustHaveSameParent) {
			if (this.IsLeaf) {
				// use the rightsibling pointer
				BPlusTreeLeafNode<TKey, TValue> leaf = this as BPlusTreeLeafNode<TKey, TValue>;
                return BPlusTreeNode<TKey, TValue>.Read(this.Tree, this.Parent, leaf.RightSiblingLocation);
			} else {



				// look in parent for our smallest keyword, grab childlocation to the left of it
				int parentChildCount;
				int childLocationIndex = GetIndexInParent(out parentChildCount);

				if (childLocationIndex == -1) {
					// never found the index, or no parent (aka root). No sibling to return.
					return null;
				} else {
					if (childLocationIndex < parentChildCount-1) {
						// we know the right sibling is attached to the same parent.
						BPlusTreeNode<TKey, TValue> node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, this.Parent, Parent.ChildLocations[childLocationIndex + 1]);
						return node;
					} else {
						// we are the rightmost child, as we're at the location that's 1 past the fanout size
						if (mustHaveSameParent) {
							// caller wants the sibling only if it's from the same parent, and it's not.
							return null;
						} else {
							// we have to find the common ancestor between this and its right sibling.
							// spin up until we find the first node whose child index is <= Fanoutsize
							// then drill down the leftmost path until we're at the same depth as we started at
							int i = 0;
							int nodeChildLocationIndex = -1;
							BPlusTreeNode<TKey, TValue> node = this;
							while (node.Parent != null) {

								i++;

								// keep bubbling up until we're at a node whose parent has at least one child to our right.
								nodeChildLocationIndex = node.GetIndexInParent(out parentChildCount);
								if (nodeChildLocationIndex < parentChildCount-1) {
									// this node is not the rightmost child of its parent.
									// we're done bubbling (node contains the ancestor node, nodeChildLocationIndex can tell us the subtree to search down)
									break;
								} else {
									node = node.Parent;
								}
							}

							if (nodeChildLocationIndex == parentChildCount-1) {
								// the only common ancestor has our node being the rightmost node in the tree (at this depth).
								return null;
							}


							// ok, node contains the ancestor somehwere up the tree that has the node we're looking for as its 
							// leftmost descendent at the same depth.
							// So we just keep drilling down the leftmost path until we're at the same depth.
                            node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, node as BPlusTreeIndexNode<TKey, TValue>, nodeChildLocationIndex + 1);
							while (i > 0) {
								BPlusTreeIndexNode<TKey, TValue> indexNode = node as BPlusTreeIndexNode<TKey, TValue>;
                                node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, indexNode, indexNode.ChildLocations[0]);
								i--;
							}

							// node now contains the right sibling!
							return node;

						}
					}
				}




				//// bounce up to the parent
				//if (Parent != null) {
				//    TKey ourKey = FirstKeyword;
				//    for (int i=0; i < Parent.Keywords.Count; i++) {
				//        TKey parentKeyword = Parent.Keywords[i];
				//        if (parentKeyword.CompareTo(ourKey, KeywordMatchMode.ExactMatch, false) == 0) {
				//            // follow right child
				//            BPlusTreeNode<TKey, TValue> node = BPlusTreeNode<TKey, TValue>.Read(this.Tree, this.Parent, Parent.ChildLocations[i+1]);
				//            return node;
				//        }
				//    }
				//}
				//return null;
			}
		}


		public KeywordMatch<TKey, TValue> Insert(TKey keyword, TValue newValue) {

            var matches = new List<KeywordMatch<TKey, TValue>>();

            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin inserting new keyword '" + keyword.ToString() + "'");

                this.performSearch(keyword, KeywordMatchMode.ExactMatch, false, true, matches);
                // we should always get exactly one node back! (we passed true to recursive search to return a new leaf node if one isn't found that matches)
                // if we get null or 0 or multiple nodes, bomb
                if (matches.Count != 1) {
                    throw new InvalidOperationException(getDisplayMember("Insert", "Could not find a keyword match to insert data into"));
                } else {

                    // performSearch auto-splits children that are full when returnLeafIfNotFound is true.
                    // so we know the node we got back always has room in it.

                    // there's room in the node. just add the item.
                    matches[0].Node.Keywords.Insert(matches[0].Index, keyword);
                    matches[0].Node.Values.Insert(matches[0].Index, newValue);
                    matches[0].Node.Write();
                }
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End inserting new keyword '" + keyword.ToString() + "'");
            }

			return matches[0];
		}

        public List<KeywordMatch<TKey, TValue>> Update(TKey keyword, KeywordMatchMode matchMode, TValue newValue, UpdateMode updateMode) {

			List<KeywordMatch<TKey, TValue>> matches = new List<KeywordMatch<TKey, TValue>>();
            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin updating keyword '" + keyword.ToString() + "'");
                this.performSearch(keyword, matchMode, false, false, matches);
                foreach (KeywordMatch<TKey, TValue> match in matches) {
                    match.Node.Values[match.Index].Update(newValue, updateMode);
                    match.Node.Write();
                }
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End updating keyword '" + keyword.ToString() + "'");
            }
			return matches;
		}

		public List<KeywordMatch<TKey, TValue>> Search(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase) {

			List<KeywordMatch<TKey, TValue>> matches = new List<KeywordMatch<TKey, TValue>>();

			// Contains and EndsWith are special cases -- we index keywords from the leftmost data.
			// So to support those, we have essentially doa full leaf traversal.
			switch (matchMode) {
				case KeywordMatchMode.Contains:
                    if (keyword.ToString().Length < Toolkit.ToInt32("SearchEngineMinimumQueryLengthForContains", 3)) {
                        // don't let them do a 'contains' on something less than 2 chars, that will kill the server...
                        throw new InvalidOperationException(getDisplayMember("Search{contains}", "Invalid query.  Please lengthen your search criteria."));
                    } else {
                        // ignore the tree, just traverse all leaves and all keywords in each leaf
                        foreach (BPlusTreeLeafNode<TKey, TValue> node in Tree.TraverseLeaves()) {
                            node.performSearch(keyword, matchMode, ignoreCase, false, matches);
                        }
                    }
                    break;
				case KeywordMatchMode.EndsWith:
					// ignore the tree, just traverse all leaves and all keywords in each leaf
                    foreach (BPlusTreeLeafNode<TKey, TValue> node in Tree.TraverseLeaves()) {
                        node.performSearch(keyword, matchMode, ignoreCase, false, matches);
					}
					break;
				default:
					// follow the index tree
                    this.performSearch(keyword, matchMode, ignoreCase, false, matches);
					break;
			}

			return matches;

		}
		//List<BPlusTreeLeafNode<TKey, TValue>> foundNodes, 
		protected internal abstract void performSearch(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase, bool returnLeafIfNotFound, List<KeywordMatch<TKey, TValue>> matches);






		#endregion Operations

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "BPlusTreeNode", resourceName, null, defaultValue, substitutes);
        }

	}
}
