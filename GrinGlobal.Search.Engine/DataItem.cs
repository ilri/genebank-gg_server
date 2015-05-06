using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
	internal struct DataItem<T> where T : new() {
		internal T Item;
		internal long DataFileOffset;

		internal DataItem(T item, long offset) {
			this.Item = item;
			this.DataFileOffset = offset;
		}
	}

}
