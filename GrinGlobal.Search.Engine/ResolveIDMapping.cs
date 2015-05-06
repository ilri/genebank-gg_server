using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace GrinGlobal.Search.Engine {
	internal struct ResolveIDMapping : IExternalSortable<ResolveIDMapping> {
		internal int PrimaryKeyID;
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


		#region IExternalSortable<ResolveIDMapping> Members

		public void Read(BinaryReader rdr, bool includeSortKey) {
			if (includeSortKey) {
				PrimaryKeyID = rdr.ReadInt32();
			}
			ResolvedPrimaryKeyID = rdr.ReadInt32();
		}

		public void Write(BinaryWriter wtr, bool includeSortKey) {
			if (includeSortKey) {
				wtr.Write(PrimaryKeyID);
			}
			wtr.Write(ResolvedPrimaryKeyID);
		}

		public IEnumerable<ResolveIDMapping> Sort(IEnumerable<ResolveIDMapping> items) {
			return items.OrderBy(rim => rim.PrimaryKeyID).ThenBy(rim2 => rim2.ResolvedPrimaryKeyID);
		}

		public int CompareTo(ResolveIDMapping other) {
			if (this.PrimaryKeyID > other.PrimaryKeyID) {
				return -1;
			} else if (this.PrimaryKeyID < other.PrimaryKeyID) {
				return 1;
			} else {
				if (this.ResolvedPrimaryKeyID > other.ResolvedPrimaryKeyID) {
					return -1;
				} else if (this.ResolvedPrimaryKeyID < other.ResolvedPrimaryKeyID) {
					return 1;
				} else {
					return 0;
				}
			}
		}

		public bool IsInSameGroup(ResolveIDMapping other) {
			return this.PrimaryKeyID == other.PrimaryKeyID;
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
