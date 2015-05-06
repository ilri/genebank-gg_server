using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
    internal class NodeLocks : IDisposable {

        private int _writeThreadId;
        private const int IMPOSSIBLE_THREAD_ID = -1;

        private List<int> _readThreadIds;

        private static int _maxWaitTime = Toolkit.GetSetting("NodeLockMaxWaitMS", 100);
        private object _key;


        public int LockCount {
            get {
                return _readThreadIds.Count + (_writeThreadId == IMPOSSIBLE_THREAD_ID ? 0 : 1);
            }
        }

        private bool threadCanWrite(int currentThreadId) {

            // a thread can write only if:
            // 1. There are no reads or the only read(s) that exist belong to the current thread
            // 2. There is no write happening, or the write belongs to the current thread


            // 1. There are no reads or the only read(s) that exist belong to the current thread
            foreach (int id in _readThreadIds) {
                if (id != currentThreadId) {
                    return false;
                }
            }

            // 2. There is no write happening, or the write belongs to the current thread
            if (_writeThreadId != IMPOSSIBLE_THREAD_ID) {
                // a write is occurring
                if (_writeThreadId != currentThreadId) {
                    // a write is occurring on another thread. 
                    return false;
                }
            }


            return true;


        }

        private bool threadCanRead(int currentThreadId) {

            // a thread can read only if:
            // 1. there is no write happening or the write taking place belongs to the current thread

            return _writeThreadId == IMPOSSIBLE_THREAD_ID || _writeThreadId == currentThreadId;
        }

        public NodeLocks() {
            _writeThreadId = IMPOSSIBLE_THREAD_ID;
            _key = new object();
            _readThreadIds = new List<int>();
        }

        /// <summary>
        /// Obtains a read or a write lock.  Will block calling thread until it is okay to continue with the action
        /// </summary>
        /// <param name="writable"></param>
        public void AddLock(bool writable) {
            lock (_key) {
                int currentThreadId = Thread.CurrentThread.ManagedThreadId;
                if (writable) {
                    while (!threadCanWrite(currentThreadId)) {
                        Monitor.Wait(_key, _maxWaitTime);
                    }
                    _writeThreadId = currentThreadId;
                } else {
                    while (!threadCanRead(currentThreadId)) {
                        Monitor.Wait(_key, _maxWaitTime);
                    }
                    _readThreadIds.Add(currentThreadId);
                }
            }
        }

        /// <summary>
        /// Removes the lock associated with the current thread.
        /// </summary>
        /// <param name="writable"></param>
        public void RemoveLock(bool writable) {
            lock (_key) {
                if (writable) {
                    _writeThreadId = IMPOSSIBLE_THREAD_ID;
                } else {
                    _readThreadIds.Remove(Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        public void Dispose() {
        }
    }
}
