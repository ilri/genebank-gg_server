using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GrinGlobal.Search.Engine {
    public struct ResolvedHit : IComparable<ResolvedHit> {

        public ResolvedHit(Hit h, IEnumerable<int> resolvedIDs) : this() {
            Hit = h;
            ResolvedIDs = resolvedIDs;
        }


        public Hit Hit;

        public IEnumerable<int> ResolvedIDs { get; private set; }

        public override string ToString() {
            return Hit.ToString() + ", ResolvedIDs Count=" + ResolvedIDs.Count();
        }


        public static IEnumerable<int> GetAllResolvedIDs(IEnumerable<ResolvedHit> list) {
            var ret = new List<int>();
            if (list != null) {
                foreach (ResolvedHit rh in list) {
                    ret.AddRange(rh.ResolvedIDs);
                }
            }
            return ret;
        }

        public static Dictionary<short, List<ResolvedHit>> GroupResolvedHitsByIndex(IEnumerable<ResolvedHit> list) {
            var dic = new Dictionary<short, List<ResolvedHit>>();

            if (list != null && list.Count() > 0){
                short prevIndexID = short.MinValue;
                List<ResolvedHit> rhList = new List<ResolvedHit>();
                foreach (ResolvedHit rh in list) {
                    if (prevIndexID != rh.Hit.TemporaryIndexID){
                        if (prevIndexID > short.MinValue) {
                            dic.Add(prevIndexID, rhList);
                        }
                        rhList = new List<ResolvedHit>();
                        prevIndexID = rh.Hit.TemporaryIndexID;
                    }
                    rhList.Add(rh);
                }

                if (rhList.Count > 0) {
                    dic.Add(prevIndexID, rhList);
                }

            }

            return dic;

        }
        private static IEnumerable<ResolvedHit> sortByPrimaryKeyID(List<ResolvedHit> list) {
            return list.OrderBy(r => r.Hit.PrimaryKeyID);
        }


        private static bool alignListStarts(List<ResolvedHit> list1, List<ResolvedHit> list2, ref int max) {

            if (list1.Count == 0 || list2.Count == 0) {
                max = 0;
                return false;
            }

            max = Toolkit.Max(list1[0].Hit.PrimaryKeyID, list2[0].Hit.PrimaryKeyID);

            // spin through list1 until the matching pk id is found
            int i = 0;
            int count = list1.Count;
            while (i < count && list1[i].Hit.PrimaryKeyID < max){
                i++;
            }
            if (i > 0) {
                list1.RemoveRange(0, i);
            }


            i = 0;
            count = list2.Count;
            while (i < count && list2[i].Hit.PrimaryKeyID < max) {
                i++;
            }
            if (i > 0) {
                list2.RemoveRange(0, i);
            }

            return list1.Count > 0 && list2.Count > 0;

        }

        public static IEnumerable<ResolvedHit> Intersect(IEnumerable<ResolvedHit> list1, IEnumerable<ResolvedHit> list2, int keywordIndex, bool returnHitsWithNoResolvedIDs, bool trackResolvedHits) {

            // this method lets us strip out all ResolvedHit objects whose ResolvedIDs lists do not overlap at all.
            var ret = new List<ResolvedHit>();

            ResolvedHit lastResolvedHit = new ResolvedHit();


            if (keywordIndex == -1) {
                // ignore keyword index

                // grab all intersected resolved ids
                List<int> intersected = GetAllResolvedIDs(list1).Intersect(GetAllResolvedIDs(list2)).ToList();

                //var all = GetAllResolvedIDs(list1).Union(GetAllResolvedIDs(list2)).OrderByDescending(f => f).ToList();

                if (!trackResolvedHits) {
                    // they just want ID's.
                    ret.Add(new ResolvedHit(new Hit { ID = -1, FieldOrdinal = -1, KeywordIndex = -1, PrimaryKeyID = -1, TemporaryIndexID = -1 }, intersected));
                } else {
                    // they want to group resolved id's by hits...
                    if (list1 != null) {
                        // only add those hits from list1 that contain at least one resolved id in the list
                        var rhs1 = list1.ToList();
                        for (var i = 0; i < rhs1.Count; i++) {
                            var rh1 = rhs1[i];
                            rh1.ResolvedIDs = intersected.Intersect(rh1.ResolvedIDs);
                            if (returnHitsWithNoResolvedIDs || rh1.ResolvedIDs.Count() > 0) {
                                ret.Add(rh1);
                            }
                        }
                    }

                    if (list2 != null) {
                        var rhs2 = list2.ToList();
                        for (var i = 0; i < rhs2.Count; i++) {
                            var rh2 = rhs2[i];
                            rh2.ResolvedIDs = intersected.Intersect(rh2.ResolvedIDs);
                            if (returnHitsWithNoResolvedIDs || rh2.ResolvedIDs.Count() > 0) {
                                ret.Add(rh2);
                            }
                        }
                    }
                }

            } else if (keywordIndex > -1) {
                // TODO: exact match stuff here
            } else {

                // keyword index is derived from context (i.e. quoted string)

                // we need to remember the last resolved hit so the greatest Keyword Index value is used
                // think of "georgia cow corn" search:
                //   pass1: KeywordIndex = 0 for georgia
                //   pass2: KeywordIndex = 1 for cow
                //   pass3: KeywordIndex = 2 for corn



                // optimization: assume all hits are always returned in index order (always true due to how SearchCommand executes searches)
                var rhDic1 = GroupResolvedHitsByIndex(list1);
                var rhDic2 = GroupResolvedHitsByIndex(list2);

                foreach (short key1 in rhDic1.Keys) {

                    // optimization: sort by pk id so we can make several assumptions
                    var sortedList1 = sortByPrimaryKeyID(rhDic1[key1]).ToList();

                    if (sortedList1.Count == 0){
                        // optimization: nothing in first list, no need to even pull second list.
                        //               we know there's no intersection.
                        continue;
                    }


                    List<ResolvedHit> rhList2 = null;
                    if (rhDic2.TryGetValue(key1, out rhList2)){

                        // rhList2 contains all hits from same index as values in rhList1.

                        // optimization: do an up-front sort of the second list by primaryKeyID
                        //               this allows us to jump out of the inner loop early
                        //               as well as start later in the inner loop on subsequent iterations
                        var sortedList2 = sortByPrimaryKeyID(rhList2).ToList();

                        if (sortedList2.Count == 0) {

                            // optimization: no hits in second list to intersect with hits in first list.
                            continue;
                        }

                        if ((sortedList1[sortedList1.Count - 1].Hit.PrimaryKeyID < sortedList2[0].Hit.PrimaryKeyID)
                            || (sortedList2[sortedList2.Count - 1].Hit.PrimaryKeyID < sortedList1[0].Hit.PrimaryKeyID)) {

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
                            if (alignListStarts(sortedList1, sortedList2, ref max)) {

                                int i = 0;
                                int j = 0;
                                var allrh2IDs = new List<int>();
                                var rh1 = sortedList1[i];
                                var count2 = sortedList2.Count;
                                while (j < count2 && rh1.Hit.PrimaryKeyID == sortedList2[j].Hit.PrimaryKeyID) {
                                    var rh2 = sortedList2[j++];
                                    if (rh1.Hit.FieldOrdinal == rh2.Hit.FieldOrdinal) {

                                        // this means treat rh1 as the previous keyword, rh2 as the next keyword
                                        if (rh1.Hit.KeywordIndex == rh2.Hit.KeywordIndex - 1) {
                                            // rh1 contains first word, rh2 contains second. add as a valid hit.
                                            allrh2IDs.AddRange(rh2.ResolvedIDs);
                                            lastResolvedHit = rh2;

                                            // if we get here, it means the index, field, keyword, and pk id are all exactly right
                                            // this can never happen more than once for a given rh1.Hit, so we jump out
                                        } else {
                                            // wrong keyword offset, do not add
                                        }
                                    }
                                }

                                if (rh1.Hit.PrimaryKeyID == max) {
                                    sortedList1.RemoveAt(0);
                                } 
                                
                                if (j > 0){
                                    sortedList2.RemoveRange(0, j);
                                }

                                if (allrh2IDs.Count > 0) {
                                    ret.Add(new ResolvedHit(lastResolvedHit.Hit, allrh2IDs.Intersect(rh1.ResolvedIDs)));
                                }

                            }

                            done = sortedList1.Count == 0 || sortedList2.Count == 0;

                        }


                    } // end if rh2.TryGetValue
                } // end foreach rh1Dic.keys

            } // end else

            return ret.AsEnumerable();
        }

        public static IEnumerable<ResolvedHit> Intersect2(IEnumerable<ResolvedHit> list1, IEnumerable<ResolvedHit> list2, int keywordIndex) {

            // this method lets us strip out all ResolvedHit objects whose ResolvedIDs lists do not overlap at all.
            var ret = new List<ResolvedHit>();

            ResolvedHit lastResolvedHit = new ResolvedHit();


            if (keywordIndex == -1) {
                // ignore keyword index
                if (list1 == null) {
                    lastResolvedHit.Hit = list2.FirstOrDefault().Hit;
                } else if (list2 == null) {
                    lastResolvedHit.Hit = list1.FirstOrDefault().Hit;
                } else {
                    lastResolvedHit.Hit = list2.FirstOrDefault().Hit;
                }

                lastResolvedHit.ResolvedIDs = GetAllResolvedIDs(list1).Intersect(GetAllResolvedIDs(list2));

                ret.Add(lastResolvedHit);

            } else if (keywordIndex > -1) {
                // TODO: exact match stuff here
            } else {

                // keyword index is derived from context (i.e. quoted string)

                // we need to remember the last resolved hit so the greatest Keyword Index value is used
                // think of "georgia cow corn" search:
                //   pass1: KeywordIndex = 0 for georgia
                //   pass2: KeywordIndex = 1 for cow
                //   pass3: KeywordIndex = 2 for corn


                // we're using optimization that all hits are always returned in index order
                // so if 

                var rhDic1 = GroupResolvedHitsByIndex(list1);
                var rhDic2 = GroupResolvedHitsByIndex(list2);


                // for troubleshooting....
#if DEBUG_PERFORMANCE
using(var hptTotal = new HighPrecisionTimer("total", true)){
double totalMS = 0.0;
double list2MS = 0.0;
double intersectMS = 0.0;
#endif
                foreach (short key1 in rhDic1.Keys) {

                    var rhList1 = rhDic1[key1];

                    List<ResolvedHit> rhList2 = null;
                    if (rhDic2.TryGetValue(key1, out rhList2)) {

                        // rhList2 contains all hits from same index as values in rhList1.

                        foreach (var rh1 in rhList1) {

#if DEBUG_PERFORMANCE
            using (var hptList2 = new HighPrecisionTimer("list2", true)) {
#endif
                            var allrh2IDs = new List<int>();

                            foreach (var rh2 in rhList2) {
                                if (rh1.Hit.FieldOrdinal == rh2.Hit.FieldOrdinal && rh1.Hit.PrimaryKeyID == rh2.Hit.PrimaryKeyID) {

                                    // this means treat rh1 as the previous keyword, rh2 as the next keyword
                                    if (rh1.Hit.KeywordIndex == rh2.Hit.KeywordIndex - 1) {
                                        // rh1 contains first word, rh2 contains second. add as a valid hit.
                                        allrh2IDs.AddRange(rh2.ResolvedIDs);
                                        lastResolvedHit = rh2;

                                        // if we get here, it means the index, field, keyword, and pk id are all exactly right
                                        // this can never happen more than once for a given rh1.Hit, so we jump out
                                        break;
                                    } else {
                                        // wrong keyword offset, do not add
                                    }

                                }
                            }

#if DEBUG_PERFORMANCE
                list2MS += hptList2.ElapsedMilliseconds;
                using (var hptIntersect = new HighPrecisionTimer("intersect", true)) {
#endif

                            ret.Add(new ResolvedHit(lastResolvedHit.Hit, allrh2IDs.Intersect(rh1.ResolvedIDs)));

#if DEBUG_PERFORMANCE
                    intersectMS += hptIntersect.ElapsedMilliseconds;
                }
            }
#endif

                        } // end foreach rhList1
                    } // end if rh2.TryGetValue
                } // end foreach rh1Dic.keys

#if DEBUG_PERFORMANCE
    Debug.WriteLine("total elapsed ms:     " + totalMS);
    Debug.WriteLine("list2 elapsed ms:     " + list2MS);
    Debug.WriteLine("intersect elapsed ms: " + intersectMS);
    Debug.WriteLine("-------------------------------------");
}
#endif
            } // end else

            return ret.AsEnumerable();
        }


        public static IEnumerable<ResolvedHit> Concat(IEnumerable<ResolvedHit> list1, IEnumerable<ResolvedHit> list2) {

            if (list1 == null) {
                return list2;
            } else if (list2 == null) {
                return list1;
            } else {
                return list1.Concat(list2);
            }
        }

        public static IEnumerable<ResolvedHit> Except(IEnumerable<ResolvedHit> list1, IEnumerable<ResolvedHit> list2) {


            if (list1 == null) {
                return null;
            } else if (list2 == null) {
                return list1;
            } else {
                var ret = new List<ResolvedHit>();
                ret.Add(new ResolvedHit(list2.Last().Hit, GetAllResolvedIDs(list1).Except(GetAllResolvedIDs(list2))));
                return ret;
            }

        }



        #region IComparable<ResolvedHit> Members

        public int CompareTo(ResolvedHit other) {
            return this.Hit.CompareTo(other.Hit);
        }

        #endregion
    }
}
