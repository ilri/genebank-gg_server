using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
    internal class NodeLockManager {

        private Dictionary<long, NodeLocks> _nodeLocks;
        private object _key;
//        private bool _exclusiveLock;

        public NodeLockManager() {
            _nodeLocks = new Dictionary<long, NodeLocks>();
            _key = new object();
//            _exclusiveLock = false;
        }

        public void NodeIsMoving(long oldNodeLocation, long newNodeLocation) {
            NodeLocks nl = _nodeLocks[oldNodeLocation];
            if (nl != null) {
                _nodeLocks[newNodeLocation] = nl;
                _nodeLocks[oldNodeLocation] = null;
            }
        }

        /// <summary>
        /// Will block the calling thread until a lock (readable or writable) can be obtained for the node at the given location.
        /// </summary>
        /// <param name="nodeLocation"></param>
        /// <param name="writable"></param>
        public void Lock(long nodeLocation, bool writable) {
            NodeLocks nl = null;
            if (!_nodeLocks.TryGetValue(nodeLocation, out nl)) {
                nl = new NodeLocks();
                _nodeLocks.Add(nodeLocation, nl);
            }
            nl.AddLock(writable);
        }

        public void Unlock(long nodeLocation, bool writable) {
            NodeLocks nl = null;
            if (_nodeLocks.TryGetValue(nodeLocation, out nl)){
                nl.RemoveLock(writable);
            }
        }

        internal delegate object ExclusiveLockCallback(object[] args1, object[] args2);

        public object GetExclusiveLock(ExclusiveLockCallback callback, object[] args1, object[] args2) {
            lock (_key) {
                while (TotalLocks() > 0) {
                    Thread.Sleep(100);
                }
                return callback(args1, args2);
            }
        }

        public void ReleaseExclusiveLock() {
            lock (_key) {

            }
        }

        public int TotalLocks() {
            int total = 0;
            foreach (NodeLocks nl in _nodeLocks.Values) {
                total += nl.LockCount;
            }
            return total;
        }

    }
}
