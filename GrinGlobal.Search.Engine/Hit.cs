using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;
using System.Runtime.Serialization;

namespace GrinGlobal.Search.Engine {
    [DataContract(Namespace = "http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    public struct Hit : IExternalSortable<Hit> {

        internal static IEnumerable<int> ReadAllPrimaryKeyIDs(BinaryReader rdr, long startPosition) {
            if (startPosition < rdr.BaseStream.Length) {

                rdr.BaseStream.Position = startPosition;

                Hit h = new Hit();

                // first, get the count of hits from the stream
                int hitCount = rdr.ReadInt32();
                int i = 0;
                while (rdr.BaseStream.Position < rdr.BaseStream.Length && i < hitCount) {
                    h.Read(rdr, false);
                    yield return h.PrimaryKeyID;
                    i++;
                }
            }

        }

        internal static IEnumerable<int> ReadAllPrimaryKeyIDs(BinaryReader rdr, long startPosition, int fieldOrdinal, int keywordIndex, short tempIndexID) {
            if (startPosition < rdr.BaseStream.Length) {

                rdr.BaseStream.Position = startPosition;

                Hit h = new Hit();

                // first, get the count of hits from the stream
                int hitCount = rdr.ReadInt32();
                int i = 0;
                while (rdr.BaseStream.Position < rdr.BaseStream.Length && i < hitCount) {
                    h.Read(rdr, false);
                    h.TemporaryIndexID = tempIndexID;
                    if (fieldOrdinal < 0 || h.FieldOrdinal == fieldOrdinal) {
                        if (keywordIndex < 0 || h.KeywordIndex == keywordIndex) {
                            // fieldordinal wasn't specified or its the same as given parameter.
                            // keywordindex wasn't specified or its the same as given parameter.
                            yield return h.PrimaryKeyID;
                        }
                    }
                    i++;
                }
            }

        }

        internal static List<Hit> ReadAll(BinaryReader rdr, long startPosition, int[] fieldOrdinals, int keywordIndex, short tempIndexID) {

            return ReadHits(rdr, startPosition, fieldOrdinals, keywordIndex, tempIndexID).ToList();

        }

        internal static IEnumerable<Hit> ReadHits(BinaryReader rdr, long startPosition, int[] fieldOrdinals, int keywordIndex, short tempIndexID) {
            if (startPosition < rdr.BaseStream.Length) {

                rdr.BaseStream.Position = startPosition;

                Hit h = new Hit();

                // first, get the count of hits from the stream
                int hitCount = rdr.ReadInt32();
                int i = 0;

                // since the following is the 'meat' of processing during a search query, we optimize the if checks out of the tight loop when we can...
                if ((fieldOrdinals == null || fieldOrdinals.Length == 0 || fieldOrdinals[0] < 0) && keywordIndex < 0) {
                    while (rdr.BaseStream.Position < rdr.BaseStream.Length && i < hitCount) {
                        h.Read(rdr, false);
                        h.TemporaryIndexID = tempIndexID;
                        yield return h;
                        i++;
                    }
                } else {
                    while (rdr.BaseStream.Position < rdr.BaseStream.Length && i < hitCount) {
                        h.Read(rdr, false);
                        h.TemporaryIndexID = tempIndexID;
                        if (fieldOrdinals == null || fieldOrdinals.Length == 0 || fieldOrdinals[0] < 0){
                            if (keywordIndex < 0 || h.KeywordIndex == keywordIndex) {
                                // fieldordinal is not given.
                                // keywordindex is not given or is the same as given parameter.
                                yield return h;
                            }
                        } else {
                            for (var j = 0; j < fieldOrdinals.Length; j++) {
                                if (h.FieldOrdinal == fieldOrdinals[j]) {
                                    if (keywordIndex < 0 || h.KeywordIndex == keywordIndex) {
                                        // fieldordinal is the same as given parameter.
                                        // keywordindex is the same as given parameter.
                                        yield return h;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
            }
        }

        internal static long WriteAll(BinaryWriter wtr, List<Hit> hits) {

            // hits must always be grouped, so ensure they're sorted properly!
            if (hits.Count > 0) {
                hits = hits[0].Sort(hits).ToList();
            }

            // orient to end of stream
            long startPosition = wtr.BaseStream.Position = wtr.BaseStream.Length;
            // remember how many we're going to write
            wtr.Write(hits.Count);
            // write all of them
            foreach (Hit h in hits) {
                h.Write(wtr, false);
            }
            // return where we started writing at
            return startPosition;
        }

        public void OnDeserialized(StreamingContext sc) {
        }

		/// <summary>
		/// The HitID used only for internal purposes
		/// </summary>
		internal int ID;

		/// <summary>
		/// The primary key value from the record in the database
		/// </summary>
        [DataMember]
		public int PrimaryKeyID;

		/// <summary>
		/// The ordinal of the field within the database row
		/// </summary>
		public int FieldOrdinal;

		/// <summary>
		/// The index of the keyword within the field
		/// </summary>
        [DataMember]
        public int KeywordIndex;

        /// <summary>
        /// Gets or sets the TemporaryID of the Index this Hit is associated with.
        /// </summary>
        internal short TemporaryIndexID;

		/// <summary>
		/// 
		/// </summary>
        [DataMember]
        public HitField[] Fields;

		internal Hit(int fieldCount) {
			ID = -1;
			PrimaryKeyID = -1;
			FieldOrdinal = -1;
			KeywordIndex = -1;
            TemporaryIndexID = -1;
			Fields = new HitField[fieldCount];
		}

		internal Hit(BinaryReader rdr, bool includeSortKey, int fieldCount) {
			ID = -1;
			PrimaryKeyID = -1;
			FieldOrdinal = -1;
			KeywordIndex = -1;
            TemporaryIndexID = -1;
            Fields = new HitField[fieldCount];
			Read(rdr, includeSortKey);
		}

		internal Hit CloneOnlyFields(int id, int pkID, int fieldOrdinal, int keywordIndex) {
			Hit h = new Hit { ID = id, PrimaryKeyID = pkID, FieldOrdinal = fieldOrdinal, KeywordIndex = keywordIndex };
			h.Fields = new HitField[this.Fields.Length];
			for(int i=0;i<this.Fields.Length;i++){
				HitField hf = Fields[i];
				h.Fields[i] = new HitField { FieldOrdinal = hf.FieldOrdinal, Value = hf.Value };
			}
			return h;
		}

		internal Hit(int id, int primaryKeyID, int fieldOrdinal, int keywordOffset, List<Field> indexedFields) {
			ID = id;
			PrimaryKeyID = primaryKeyID;
			FieldOrdinal = fieldOrdinal;
			KeywordIndex = keywordOffset;
            TemporaryIndexID = -1;

			if (indexedFields == null) {
				Fields = new HitField[0];
			} else {
				Fields = new HitField[indexedFields.Count];
				for (int i=0; i < Fields.Length; i++) {
					Fields[i] = new HitField { FieldOrdinal = indexedFields[i].Ordinal, Value = Toolkit.ToInt32(indexedFields[i].Value, -1) };
				}
			}
		}

        //[OnSerializing]
        //internal void serializing(StreamingContext sc) {
            
        //}

		public override string ToString() {
			return "ID=" + ID + ", PrimaryKeyID=" + PrimaryKeyID + ", FieldOrdinal=" + FieldOrdinal + ", KeywordIndex=" + KeywordIndex + ", Fields length=" + Fields.Length;
		}

		#region IPersistable<Hit> Members

		public void Read(BinaryReader rdr, bool includeSortKey) {
			if (rdr == null || rdr.BaseStream.Position == rdr.BaseStream.Length) {
				ID = int.MaxValue;
				PrimaryKeyID = int.MaxValue;
				FieldOrdinal = int.MaxValue;
				KeywordIndex = int.MaxValue;
				Fields = new HitField[0];
			} else {
                if (includeSortKey) {
                    ID = rdr.ReadInt32();
                } else {
                    ID = 0;
                }
				PrimaryKeyID = rdr.ReadInt32();
				FieldOrdinal = rdr.ReadInt32();
				KeywordIndex = rdr.ReadInt32();
				byte fieldCount = rdr.ReadByte();
				Fields = new HitField[fieldCount];
				for (int i=0; i < Fields.Length; i++) {
					Fields[i] = new HitField(rdr);
				}
			}
		}



		public void Write(BinaryWriter wtr, bool includeSortKey) {
			if (includeSortKey) {
				wtr.Write(ID);
			}
			wtr.Write(PrimaryKeyID);
			wtr.Write(FieldOrdinal);
			wtr.Write(KeywordIndex);
			wtr.Write((byte)Fields.Length);
			foreach (HitField hf in Fields) {
				hf.Write(wtr, includeSortKey);
			}
		}

        public bool IsDefault() {
            return ID == int.MaxValue && PrimaryKeyID == int.MaxValue && FieldOrdinal == int.MaxValue && KeywordIndex == int.MaxValue;
        }

		public int CompareTo(Hit other) {
			if (this.ID < other.ID) {
				return -1;
			} else if (this.ID > other.ID) {
				return 1;
			} else {
				// ID is same. compare primarykeyids.
				if (this.PrimaryKeyID < other.PrimaryKeyID) {
					return -1;
				} else if (this.PrimaryKeyID > other.PrimaryKeyID) {
					return 1;
				} else {
					return 0;
				}
			}
		}

		public IEnumerable<Hit> Sort(IEnumerable<Hit> items) {
			return items.OrderBy(h => h.ID).ThenBy(h2 => h2.PrimaryKeyID);
		}


		public bool IsInSameGroup(Hit other) {
			return this.ID == other.ID;
		}

		//public void ReadOnlySortKey(BinaryReader rdr) {
		//    ID = rdr.ReadInt32();
		//}

		//public void WriteOnlySortKey(BinaryWriter wtr) {
		//    wtr.Write(ID);
		//}

		#endregion






        public static Dictionary<short, List<Hit>> GroupHitsByIndex(IEnumerable<Hit> list) {
            var dic = new Dictionary<short, List<Hit>>();

            if (list != null && list.Count() > 0) {
                short prevIndexID = short.MinValue;
                List<Hit> hList = new List<Hit>();
                foreach (Hit h in list) {
                    if (prevIndexID != h.TemporaryIndexID) {
                        if (prevIndexID > short.MinValue) {
                            dic.Add(prevIndexID, hList);
                        }
                        hList = new List<Hit>();
                        prevIndexID = h.TemporaryIndexID;
                    }
                    hList.Add(h);
                }

                if (hList.Count > 0) {
                    dic.Add(prevIndexID, hList);
                }

            }

            return dic;

        }

        private static bool alignListStarts(List<Hit> list1, List<Hit> list2, ref int max) {

            if (list1.Count == 0 || list2.Count == 0) {
                max = 0;
                return false;
            }

            max = Toolkit.Max(list1[0].PrimaryKeyID, list2[0].PrimaryKeyID);

            // spin through list1 until the matching pk id is found
            int i = 0;
            int count = list1.Count;
            while (i < count && list1[i].PrimaryKeyID < max) {
                i++;
            }
            if (i > 0) {
                list1.RemoveRange(0, i);
            }


            i = 0;
            count = list2.Count;
            while (i < count && list2[i].PrimaryKeyID < max) {
                i++;
            }
            if (i > 0) {
                list2.RemoveRange(0, i);
            }

            return list1.Count > 0 && list2.Count > 0;

        }

        public static IEnumerable<Hit> Intersect(IEnumerable<Hit> list1, IEnumerable<Hit> list2, int keywordIndex) {


            // this method lets us strip out all ResolvedHit objects whose ResolvedIDs lists do not overlap at all.
            var ret = new List<Hit>();

            if (keywordIndex == SearchCommandConstants.IGNORE_KEYWORD_INDEX) {
                // ignore keyword index

                if (list1 == null || list2 == null) {
                    return null;
                } else {
                    return list1.Intersect(list2);
                }

            } else if (keywordIndex > SearchCommandConstants.IGNORE_KEYWORD_INDEX) {
                // TODO: exact match stuff here
                throw new NotImplementedException(getDisplayMember("Insertsect{badkeywordindex}", "Hit.Intersect() does not currently support keywordIndex = {0}", keywordIndex.ToString()));
            } else {

                // keyword index is derived from context (i.e. quoted string)

                // we need to remember the last resolved hit so the greatest Keyword Index value is used
                // think of "georgia cow corn" search:
                //   pass1: KeywordIndex = 0 for georgia
                //   pass2: KeywordIndex = 1 for cow
                //   pass3: KeywordIndex = 2 for corn



                // optimization: assume all hits are always returned in index order (always true due to how SearchCommand executes searches)
                var hDic1 = GroupHitsByIndex(list1);
                var hDic2 = GroupHitsByIndex(list2);

                foreach (short key1 in hDic1.Keys) {

                    // optimization: sort by pk id so we can make several assumptions
                    var sortedList1 = hDic1[key1];
                    sortedList1.Sort();

                    if (sortedList1.Count == 0) {
                        // optimization: nothing in first list, no need to even pull second list.
                        //               we know there's no intersection.
                        continue;
                    }


                    List<Hit> hList2 = null;
                    if (hDic2.TryGetValue(key1, out hList2)) {

                        // rhList2 contains all hits from same index as values in rhList1.

                        // optimization: do an up-front sort of the second list by primaryKeyID
                        //               this allows us to jump out of the inner loop early
                        //               as well as start later in the inner loop on subsequent iterations
                        hList2.Sort();

                        if (hList2.Count == 0) {

                            // optimization: no hits in second list to intersect with hits in first list.
                            continue;
                        }

                        if ((sortedList1[sortedList1.Count - 1].PrimaryKeyID < hList2[0].PrimaryKeyID)
                            || (hList2[hList2.Count - 1].PrimaryKeyID < sortedList1[0].PrimaryKeyID)) {

                            // optimization: if last pk in list1 < first pk in list2
                            //               OR
                            //                  last pk in list2 < first pk in list1
                            //               we don't even need to loop to check, as the lists are completely disjoint
                            continue;

                        }



                        bool done = false;

                        while (!done) {

                            // optimization:  skip over all items we know will never
                            //                intersect, such as pk id's that are not
                            //                in both lists
                            int max = 0;
                            if (alignListStarts(sortedList1, hList2, ref max)) {

                                int i = 0;
                                int j = 0;
                                var allh2s = new List<Hit>();
                                var h1 = sortedList1[i];
                                var count2 = hList2.Count;
                                while (j < count2 && h1.PrimaryKeyID == hList2[j].PrimaryKeyID) {
                                    var h2 = hList2[j++];
                                    if (h1.FieldOrdinal == h2.FieldOrdinal) {

                                        // this means treat rh1 as the previous keyword, rh2 as the next keyword
                                        if (h1.KeywordIndex == h2.KeywordIndex - 1) {
                                            // rh1 contains first word, rh2 contains second. add as a valid hit.
                                            ret.Add(h2);

                                            // if we get here, it means the index, field, keyword, and pk id are all exactly right
                                            // this can never happen more than once for a given rh1.Hit, so we jump out
                                        } else {
                                            // wrong keyword offset, do not add
                                        }
                                    }
                                }

                                if (h1.PrimaryKeyID == max) {
                                    sortedList1.RemoveAt(0);
                                }

                                if (j > 0) {
                                    hList2.RemoveRange(0, j);
                                }

                            }

                            done = sortedList1.Count == 0 || hList2.Count == 0;

                        }


                    } // end if rh2.TryGetValue
                } // end foreach rh1Dic.keys

            } // end else

            return ret.AsEnumerable();



            //if (keywordIndex < 0) {
            //    return list1.Intersect(list2);
            //} else {

            //    var retA = new List<Hit>();
            //    // need to make sure we return only hits whose keyword indexes are correct
            //    foreach (var a in list1) {
            //        if (a.KeywordIndex == keywordIndex) {
            //            retA.Add(a);
            //        }
            //    }

            //    var retB = new List<Hit>();
            //    var nextKeywordIndex = keywordIndex + 1;
            //    var listB = retA.Intersect(list2);
            //    foreach (var b in listB) {
            //        if (b.KeywordIndex == nextKeywordIndex) {
            //            retB.Add(b);
            //        }
            //    }

            //    return retA.Intersect(retB);

            //}
        }

        public static IEnumerable<Hit> Concat(IEnumerable<Hit> list1, IEnumerable<Hit> list2) {

            if (list1 == null) {
                return list2;
            } else if (list2 == null) {
                return list1;
            } else {
                return list1.Concat(list2);
            }
        }

        public static IEnumerable<Hit> Except(IEnumerable<Hit> list1, IEnumerable<Hit> list2) {


            if (list1 == null) {
                return null;
            } else if (list2 == null) {
                return list1;
            } else {
                return list1.Except(list2);
            }

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "Hit", resourceName, null, defaultValue, substitutes);
        }

    }
}
