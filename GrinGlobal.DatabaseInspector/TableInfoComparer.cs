using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector {

	/// <summary>
	/// When an instance is used as a comparer, sorts in alphabetic order by name.  To sort by dependency tree, call the static TableInfoComparer.SortByDependencies() method.
	/// </summary>
	public class TableInfoComparer : IComparer<TableInfo> {

		private static bool hasCircularReference(TableInfo t1, TableInfo t2) {
			// return true if there is one or more circular references between t1 and t2

			foreach (ConstraintInfo ci1 in t1.Constraints) {
				if (ci1.ReferencesTable == t2) {
					// t1 points at t2.
					// see if t2 points at t2
					foreach (ConstraintInfo ci2 in t2.Constraints) {
						if (ci2.ReferencesTable == t1) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public static List<TableInfo> SortByName(List<TableInfo> unsorted) {
			if (unsorted != null) {
				unsorted.Sort(new TableInfoComparer());
			}
			return unsorted;
		}

		public static List<TableInfo> SortByDependencies(List<TableInfo> unsorted) {
            unsorted = unsorted.ToList();
			List<TableInfo> sorted = new List<TableInfo>();
			List<TableInfo> alreadyChecked = new List<TableInfo>();

            const int maxLoops = 100000;
            var count = 0;
			while (unsorted.Count > 0) {
                if (count > maxLoops) {
                    throw new InvalidOperationException(getDisplayMember("SortByDependencies", "Possible infinite loop detected when sorting tables based on dependency.  Either [a] you have over 1,000 tables in your schema and you hit the absolute worst case scenario for table dependency sorting (a cyclic dependency exists that passes through all tables once) or [b] there's a bug in GrinGlobal.DatabaseInspector.TableInfoComparer.SortByDependencies(), probably related to constraint resolution.  Option [b] is much more likely."));
                }
                count++;
				for(int i=0;i<unsorted.Count;i++){
					TableInfo ti  = unsorted[i];
					bool canAdd = true;
					foreach (ConstraintInfo ci in ti.Constraints) {

						if (ci.ReferencesTable == ti) {
							// self-referential. ignore, we can always reference ourself :)
						} else if (sorted.Contains(ci.ReferencesTable)){
							// points to a table already in the sorted list.
							// we're still good, keep checking
						} else if (alreadyChecked.Contains(ci.ReferencesTable)) {

							// we've checked the referenced table once already, but it was pointing at 
							// a table not in the sorted list yet.

							if (!hasCircularReference(ti, ci.ReferencesTable)) {
								// there's no circular reference between these two tables.
								// we KNOW we can't add this guy yet.
								canAdd = false;
								break;
							} else {

								// this constraint creates a circular reference with the other table,
								// and the other table is NOT in the sorted list yet.
								// if all the other constraints on our table are pointing at 
								// tables already in the sorted list, we're good.  

								foreach (ConstraintInfo ci2 in ci.ReferencesTable.Constraints) {
									if (ci2.ReferencesTable == ti) {
										// this is the circular reference field.
										// ignore, keep checking
									} else if (sorted.Contains(ci2.ReferencesTable)) {
										// it's already in the sorted list. keep going
									} else if (alreadyChecked.Contains(ci.ReferencesTable)){
										// it's in the already checked list. keep going
									} else {
										// not in any list but unsorted. we can't add it yet.
										canAdd = false;
										break;
									}
								}

								if (!canAdd) {
									break;
								} else {
									// if all the non-self referential constraint fields are nullable,
									// we can add this guy!

									int nullableCount = 0;
									int nonCooperatorFieldCount = 0;
                                    if (ci.SourceFields.Count == 0) {
                                        // the constraint has not been fully filled yet.
                                        ci.Table = ti;
                                        foreach (var fn in ci.SourceFieldNames) {
                                            foreach (var ciField in ci.Table.Fields) {
                                                if (ciField.Name.ToLower() == fn.ToLower()) {
                                                    ci.SourceFields.Add(ciField);
                                                    break;
                                                }
                                            }
                                        }

                                        if (ci.SourceFields.Count == 0) {
                                            throw new InvalidOperationException(getDisplayMember("SortByDependencies{constraint}", "Constraint named {0} from {1} to {2}, and {3} object has no SourceFields for that constraint.", ci.ConstraintName, ti.TableName, ci.ReferencesTableName, ci.TableName));
                                        }
                                    }
									foreach (FieldInfo fi in ci.SourceFields) {
										if (!fi.IsCreatedBy && !fi.IsModifiedBy && !fi.IsOwnedBy) {
											if (fi.IsNullable) {
												nullableCount++;
											}
											nonCooperatorFieldCount++;
										}
									}

									if (nullableCount != nonCooperatorFieldCount || nullableCount == 0) {
										canAdd = false;
										break;
									}

								}


							}
						} else {
							// referenced table is not self-referential, in the sorted list, or in the already checked list.
							// means we have no possibility of adding it yet, no need to keep checking other constraints
							canAdd = false;
							break;
						}

						//if (!alreadyChecked.Contains(ci.ReferencesTable) && ci.ReferencesTable != ti) {

						//    // the already checked list does not contain our referenced table
						//    // (and the referenced table is not the current table (aka self-referential)

						//    if (alreadyChecked.Contains(ti) && alreadyChecked.Contains(ci.ReferencesTable)) {

						//        if (isCircularReference(ti, ci.ReferencesTable)) {
						//            // this is a circular reference.
						//            int nullableCount = 0;
						//            int nonCooperatorFieldCount = 0;
						//            foreach (FieldInfo fi in ci.SourceFields) {
						//                if (!fi.IsCreatedBy && !fi.IsModifiedBy && !fi.IsOwnedBy) {
						//                    if (fi.IsNullable) {
						//                        nullableCount++;
						//                    }
						//                } else {
						//                    nonCooperatorFieldCount++;
						//                }
						//            }

						//            if (nullableCount == nonCooperatorFieldCount) {
						//                // not all the non-cooperator fields are nullable.
						//                // do not add this one to the list
						//                canAdd = true;

						//            } else {
						//                canAdd = false;
						//            }
						//            break;

						//        }
						//    }
						//}
					}
					if (canAdd) {
						if (!sorted.Contains(ti)) {
							sorted.Add(ti);
							unsorted.Remove(ti);
							if (alreadyChecked.Contains(ti)) {
								alreadyChecked.Remove(ti);
							}
							// decrement i so we don't skip a table
							i--;
						}
					} else if (!alreadyChecked.Contains(ti)){
						alreadyChecked.Add(ti);
					}
				}
			}

			return sorted;
		}

		public int Compare(TableInfo x, TableInfo y) {
			if (x == null && y == null) {
				return 0;
			} else if (x == null) {
				return -1;
			} else if (y == null) {
				return 1;
			} else {
				return x.TableName.ToUpper().Trim().CompareTo(y.TableName.ToUpper().Trim());
			}
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "TableInfoComparer", resourceName, null, defaultValue, substitutes);
        }

	}
}