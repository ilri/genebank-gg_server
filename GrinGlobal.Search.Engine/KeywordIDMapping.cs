using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	internal struct KeywordIDMapping {
		internal long HitID;
		internal string Keyword;
		internal long DataFileLocation;

		/// <summary>
		/// If true, reads/writes HitID first, then Keyword.  If false, reads/write Keyword first, then HitID.
		/// </summary>
		internal bool MapHitIDToKeyword;

		internal KeywordIDMapping(long hitID, string keyword, bool mapHitIDToKeyword, long fileLocation) {
			HitID = hitID;
			Keyword = keyword;
			DataFileLocation = fileLocation;
			MapHitIDToKeyword = mapHitIDToKeyword;
		}

		internal void Read(BinaryReader br) {
			HitID = 0;
			Keyword = null;
			if (MapHitIDToKeyword) {
				HitID = br.ReadInt64();
				Keyword = br.ReadString();
			} else {
				Keyword = br.ReadString();
				HitID = br.ReadInt64();
			}
			DataFileLocation = br.ReadInt64();
		}

		internal void Write(BinaryWriter bw) {
			if (MapHitIDToKeyword) {
				bw.Write(HitID);
				bw.Write(Keyword);
			} else {
				bw.Write(Keyword);
				bw.Write(HitID);
			}
			bw.Write(DataFileLocation);
		}
	}
}
