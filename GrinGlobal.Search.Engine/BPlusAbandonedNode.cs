using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	internal class BPlusAbandonedNode {
		public long FileOffset;
		public int ByteCount;

		public static BPlusAbandonedNode Read(BinaryReader rdr) {
			BPlusAbandonedNode node = new BPlusAbandonedNode();
			node.ByteCount = rdr.ReadInt32();
			node.FileOffset = rdr.ReadInt64();
			return node;
		}

		public void Write(BinaryWriter wtr) {
			wtr.Write(ByteCount);
			wtr.Write(FileOffset);
		}

		public override string ToString() {
			return "ByteCount=" + ByteCount + ", FileOffset=" + FileOffset;
		}
	}
}
