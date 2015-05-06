using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	internal struct ResolveKeywordMapping : IExternalSortable<ResolveKeywordMapping> {
		internal string Keyword;
		internal int ResolvedPrimaryKeyID;

		//internal static IOrderedEnumerable<ResolveIDMapping> FillAndSort(int primaryKeyFieldOffset, int resolvedPrimaryKeyFieldOffset, IDataReader idr) {

		//    List<ResolveIDMapping> queue = new List<ResolveIDMapping>();
		//    while (idr.Read()) {
		//        queue.Add(
		//            new ResolveIDMapping { 
		//                PrimaryKeyID = idr.GetInt32(primaryKeyFieldOffset), 
		//                ResolvedPrimaryKeyID = idr.GetInt32(resolvedPrimaryKeyFieldOffset) 
		//            });
		//    }

		//    IOrderedEnumerable<ResolveIDMapping> output = queue.OrderBy(rim => rim.PrimaryKeyID).ThenBy(rim2 => rim2.ResolvedPrimaryKeyID);

		//    return output;

		//}


		#region IExternalSortable<ResolveKeywordMapping> Members

		public void Read(BinaryReader rdr, bool includeSortKey) {
			if (includeSortKey) {
				Keyword = rdr.ReadString();
			}
			ResolvedPrimaryKeyID = rdr.ReadInt32();
		}

		public void Write(BinaryWriter wtr, bool includeSortKey) {
			if (includeSortKey) {
				wtr.Write(Keyword);
			}
			wtr.Write(ResolvedPrimaryKeyID);
		}

		public IEnumerable<ResolveKeywordMapping> Sort(IEnumerable<ResolveKeywordMapping> items) {
			return items.OrderBy(rim => rim.Keyword).ThenBy(rim2 => rim2.ResolvedPrimaryKeyID);
		}

		public int CompareTo(ResolveKeywordMapping other) {
            int compareKeywords = Keyword.CompareTo(other.Keyword);
            if (compareKeywords == 0){
                if (ResolvedPrimaryKeyID > other.ResolvedPrimaryKeyID){
                    return -1;
                } else if (ResolvedPrimaryKeyID < other.ResolvedPrimaryKeyID){
                    return 1;
                } else {
                    return 0;
                }
            } else {
                return compareKeywords;
            }
		}

		public bool IsInSameGroup(ResolveKeywordMapping other) {
			return this.Keyword == other.Keyword;
		}

		//public void ReadOnlySortKey(BinaryReader rdr) {
		//    PrimaryKeyID = rdr.ReadInt32();
		//}

		//public void WriteOnlySortKey(BinaryWriter wtr) {
		//    wtr.Write(PrimaryKeyID);
		//}

		#endregion
	}
}
