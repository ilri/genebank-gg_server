using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Core {
    public class CachedObject {
        public object Value;
        public bool SlidingWindow;
        public DateTime ExpiresAt;
        public int MinutesToLive;
    }
}
