using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Data;

namespace GrinGlobal.Core {
//	[DebuggerStepThrough()]
	public class CacheManager {

		private CacheManager() {
			_dict = new Dictionary<string,CachedObject>();
			_lock = new object();
		}

		private static CacheManager __mgr;

		private Dictionary<string, CachedObject> _dict;
		private object _lock;

        public string Name { get; private set; }

		/// <summary>
		/// Returns the default CacheManager.  Only to be used internally by CacheManager.
		/// </summary>
		private static CacheManager Master {
			get {

				bool disabled = Toolkit.GetSetting("DisableCacheManager", (bool)false);
				if (disabled || __mgr == null){
					// either caching is disabled or there isn't a default one.
                    // create a new one
					__mgr = new CacheManager();
				}

				return __mgr;

			}
		}

        public static IEnumerable<KeyValuePair<string, object>> AllCacheEntries {
            get {
                var master = CacheManager.Master;
                foreach (var cacheName in master.Keys) {
                    var cache = master[cacheName] as CacheManager;
                    foreach (var cacheKey in cache.Keys) {
                        var kvp = new KeyValuePair<string, object>(cacheName + "." + cacheKey, cache[cacheKey]);
                        yield return kvp;
                    }
                }
            }
        }

        public static IEnumerable<CacheManager> AllCaches {
            get {
                var master = CacheManager.Master;
                foreach (var cacheName in master.Keys) {
                    yield return master[cacheName] as CacheManager;
                }
            }
        }

		/// <summary>
		/// Returns the specified CacheManager.  If one doesn't exist with the given name, it will be created and returned.
		/// </summary>
		/// <param name="cacheName"></param>
		/// <returns></returns>
		public static CacheManager Get(string cacheName) {
			CacheManager cm = (CacheManager)CacheManager.Master[cacheName];
			if (cm == null) {
                cm = (CacheManager)(CacheManager.Master[cacheName] = new CacheManager { Name = cacheName });
			}
			return cm;
		}

		/// <summary>
		/// Clears all items from the cache
		/// </summary>
		public virtual void Clear() {
			lock (_lock) {
				_dict.Clear();
			}
		}


		/// <summary>
		/// Clears all items from all caches represented CacheManager. Equivalent to calling CacheManager.Default.Clear().
		/// </summary>
		public static void ClearAll() {

            var master = CacheManager.Master;
            // first spin through all caches and clear them...
            if (master.Keys.Count > 0) {
                var keys = new string[master.Keys.Count];
                master.Keys.CopyTo(keys, 0);
                foreach (var cacheName in keys) {
                    var cache = master[cacheName] as CacheManager;
                    if (cache != null) {
                        cache.Clear();
                    }
                }
            }
            // then clear the master cache (which held all the other caches)
            master.Clear();
		}

		/// <summary>
		/// Gets or sets an item in the cache.  Defaults to expiring in 10 minutes with a sliding window (as it is accessed, the expiry time will be bumped out by 10 more minutes)
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object this[string key] {
			get {
				CachedObject co = null;
				if (_dict.TryGetValue(key, out co)){
                    if (co.ExpiresAt < DateTime.UtcNow) {
                        _dict.Remove(key);
                        return null;
                    } else {
                        co.ExpiresAt = DateTime.UtcNow.AddMinutes(co.MinutesToLive);
                        return co.Value;
                    }
				} else {
					return null;
				}
			}
			set {
                Insert(key, value, 10, true);
			}
		}

        public void Insert(string key, object newValue, int minutesToLive, bool slidingWindow){
            lock (_lock) {
                _dict[key] = new CachedObject { Value = newValue, MinutesToLive = minutesToLive, ExpiresAt = DateTime.UtcNow.AddMinutes(minutesToLive), SlidingWindow = slidingWindow };
            }
        }

        public void Remove(string key) {
            if (_dict.ContainsKey(key)) {
                lock (_lock) {
                    _dict.Remove(key);
                }
            }
        }

        public Dictionary<string, CachedObject>.KeyCollection Keys {
            get {
                return _dict.Keys;
            }
        }
	}
}
