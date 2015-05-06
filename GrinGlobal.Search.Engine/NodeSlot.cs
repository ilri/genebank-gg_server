using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {
	internal class NodeSlot : IPersistable<NodeSlot> {
		public long Location;
		public int Size;

		#region IPersistable<object> Members

		public void Read(BinaryReader rdr) {
			Location = rdr.ReadInt64();
			Size = rdr.ReadInt32();
		}

		public void Write(BinaryWriter wtr) {
			wtr.Write(Location);
			wtr.Write(Size);
		}

        public void Update(NodeSlot newValue, GrinGlobal.Interface.UpdateMode updateMode) {
			throw new NotImplementedException();
		}

		#endregion
	}

}
