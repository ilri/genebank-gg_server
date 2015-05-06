using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.Threading;
using GrinGlobal.Core;
using GrinGlobal.Search.Engine;
using System.Diagnostics;
using System.Data;
using System.Security;
using System.IO;

using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine.Hosting {
	// NOTE: If you change the class name "SearchHost" here, you must also update the reference to "SearchHost" in App.config.

    /// <summary>
    /// This is the WCF entry point for the search engine
    /// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, MaxItemsInObjectGraph = 2147483647)]
	[KnownType(typeof(Exception))]
	[KnownType(typeof(NoNullAllowedException))]
	[KnownType(typeof(InvalidOperationException))]
	[KnownType(typeof(ArgumentException))]
	[KnownType(typeof(NotImplementedException))]
	[KnownType(typeof(ApplicationException))]
	[KnownType(typeof(InvalidCastException))]
	[KnownType(typeof(SecurityException))]
    [KnownType(typeof(DBNull))]
	public class SearchHost : ISearchHost, IDisposable {

        private class QueuedCallback {
            public ThreadQueuerCallback Callback;
            public object[] Arguments;
            public int WorkItemCount;
            public bool IsCancelled;
            public string IndexName;
        }
        private class ThreadQueuer : IDisposable {
            private Thread _thread;
            private Queue<QueuedCallback> _queue;
            private object _locker = new object();
            private string _title;
            private SearchHost _host;

            private int _pendingWorkItems;

            public ThreadQueuer(SearchHost host, ThreadQueuerCallback firstTask, object[] args, string title) {
                _host = host;
                _title = title;
                _queue = new Queue<QueuedCallback>();
                _thread = new Thread(doWork);
                _thread.Start();
                _pendingWorkItems = 0;
                if (firstTask != null) {
                    AddWork(firstTask, null, args, 0);
                }
            }


            /*
             * Multithreading code is tricky at best.  I followed the advice from the following website for our simple producer/consumer queue:
             * http://www.albahari.com/threading/part4.aspx#_UsingWaitPulse
             */

            public void AddWork(ThreadQueuerCallback callback, string indexName, object[] args, int numberOfWorkItemsRepresented) {
                // request to do something, add it to the queue
                // then pulse all waiting threads so they know they can 'do stuff' to the queue
                lock (_locker) {
                    SearchHost.debug("Enqueuing task in search engine");
                    _queue.Enqueue(new QueuedCallback { Callback = callback, IndexName = indexName, IsCancelled = false, Arguments = args, WorkItemCount = numberOfWorkItemsRepresented });
                    Interlocked.Add(ref _pendingWorkItems, numberOfWorkItemsRepresented);
                    Monitor.PulseAll(_locker);
                }
            }

            public void CancelWorkByIndex(string indexName) {
                // since we're spinning on the collection, we need to throw a lock on before we do it.
                // this lock only prevents others from affecting the queue itself, not the work currently being
                // performed on a particular queue item.
                try {


                    try {
                        // cancel all those that are queued but not in the processing list yet
                        lock (_locker) {
                            foreach (var qc in _queue) {
                                try {
                                    if (!String.IsNullOrEmpty(qc.IndexName) && qc.IndexName.ToLower() == indexName.ToLower()) {
                                        qc.IsCancelled = true;
                                        SearchHost.debug("Cancelling queued callback for index " + qc.IndexName + " containing " + qc.WorkItemCount + " items to process");
                                    }
                                } catch {
                                }
                            }
                            Monitor.PulseAll(_locker);
                        }
                    } catch {
                        // something happens here, ignore...
                    }


                    // cancel all those we've already dequeued and may or may not have started processing yet
                    for (var i = 0; i < _processingCallbacks.Count; i++) {
                        try {
                            if (!String.IsNullOrEmpty(_processingCallbacks[i].IndexName) && _processingCallbacks[i].IndexName.ToLower() == indexName.ToLower()) {
                                _processingCallbacks[i].IsCancelled = true;
                                SearchHost.debug("Cancelling in process callback for index " + _processingCallbacks[i].IndexName + " containing " + _processingCallbacks[i].WorkItemCount + " items to process");
                            }
                        } catch {
                            // an exception happens here if another thread altered the _processingCallbacks list while
                            // we spin on it.
                            // just ignore this because it means the item was removed from the list and therefore won't be processed anyway.
                        }
                    }

                } catch {
                    // we bomb here it means there are no callbacks currently in process because the list is null.  ignore.
                }
                SearchHost.debug("Done cancelling items for index " + indexName);
            }

            public int PendingWorkItemCount {
                get {
                    return _pendingWorkItems;
                }
            }

            public void DecrementPendingWorkItemCount() {
                Interlocked.Add(ref _pendingWorkItems, -1);
            }

            private bool _isDone;
            public bool IsDone {
                get { return _isDone; }
            }

            private List<QueuedCallback> _processingCallbacks;

            private void doWork() {

                SearchHost.debug("Thread id for processing " + _title);
                // since we do all work in a single thread, we can use an object-level list to hold all the
                // ones we are currently processing.  Clients may add to the queue, but we pop those off into 
                // a list at controlled times so we don't get any off-by-one errors.  This also lets us
                // cancel pending callbacks
                _processingCallbacks = new List<QueuedCallback>();
                while (!_isDone) {
                    lock (_locker) {
                        while (_queue.Count == 0 && !_isDone) {
                            // nothing in the queue and nothing in process, just set pending work items to 0.
                            Interlocked.Exchange(ref _pendingWorkItems, 0);
                            Monitor.Wait(_locker, 5000);
                        }

                        // add them to the 'todo' list within the lock
                        while (_queue.Count > 0) {
                            var qc = _queue.Dequeue();
                            if (!qc.IsCancelled) {
                                SearchHost.debug("Adding task to todo list in search engine");
                                _processingCallbacks.Add(qc);
                            } else {
                                // remember to yank off the workitemcount as well since we're not going to process it anymore...
                                SearchHost.debug("Task is cancelling, removing " + qc.WorkItemCount + " items from the todo count");
                                Interlocked.Add(ref _pendingWorkItems, -1 * qc.WorkItemCount);
                            }
                        }
                    }

                    // process the 'todo' list outside the lock
                    if (_processingCallbacks.Count > 0) {
                        for (var i = 0; i < _processingCallbacks.Count; i++) {
                            // doWork is always run in exactly 1 thread.
                            // this means we can rely on _countWasDecremented to hold its value we assign it
                            // while we are processing.  If the caller manually decremented the count itself (via Decrement())
                            // we should not alter _pendignWorkItems, as it may have changed from another thread.  But if they
                            // didn't, we want to adjust the _pendingWorkItems count by the appropriate amount.

                            SearchHost.debug("Procesing task in search engine");
                            QueuedCallback qc = _processingCallbacks[i];

                            var countBefore = _pendingWorkItems; // say 99
                            var countAfter = countBefore - qc.WorkItemCount; // would be 99 - 50 = 49

                            if (qc != null) {
                                try {
                                    if (!qc.IsCancelled) {
                                        qc.Callback(this, qc);
                                    } else {
                                        SearchHost.debug("Task in todo list is cancelling, removing " + qc.WorkItemCount + " items from todo count");
                                    }
                                } catch (Exception ex) {
                                    _host.logInfo(new ProgressEventArgs(getDisplayMember("doWork{processing}", "Failed during processing in {0}: {1}",_title, ex.Message), EventLogEntryType.Error, false));
                                } finally {

                                    // during processing, suppose callers added 15 pending work items.
                                    // and callback did not alter workitemcount.
                                    // pending work items should = 64.

                                    var diff = _pendingWorkItems - countBefore; // 114 - 99 = 15
                                    Interlocked.Exchange(ref _pendingWorkItems, countAfter + diff);
                                    //if (count) {
                                    //    // caller did not decrement the pending work item count, so we do them all at once for them.
                                    //    Interlocked.Add(ref _pendingWorkItems, -1 * qc.WorkItemCount);
                                    //}
                                }
                            }
                            SearchHost.debug("Done procesing task in search engine");
                            if (_isDone) {
                                break;
                            }
                        }
                    }
                    _processingCallbacks.Clear();
                }

                // we get here, search engine will probably be shutting down. set pending items to 0 just in case we counted
                // wrong within the loop and never got back to checking it at the top of the while loop.
                Interlocked.Exchange(ref _pendingWorkItems, 0);

            }

            public void Dispose() {
                try {

                    // make sure we're not blocking any other threads
                    _isDone = true;
                    debug(_title + " thread flagged so it should exit");

                    lock (_locker) {
                        Monitor.PulseAll(_locker);
                    }

                    debug(_title + " thread now joining caller thread...");

                    _thread.Join();

                    debug(_title + " thread now exiting.");

                } catch { }
            }

            private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
                return ResourceHelper.GetDisplayMember(null, "SearchEngine", "ThreadQueuer", resourceName, null, defaultValue, substitutes);
            }
        }

        private class ThreadMonitor : IDisposable {
            private Thread _thread;
            private object _locker = new object();
            private Toolkit.VoidCallback _callback;
            private int _timeout;
            private string _title;

            public ThreadMonitor(Toolkit.VoidCallback callback, string title, int intervalInMS) {
                _title = title;
                _callback = callback;
                _timeout = intervalInMS;
                _thread = new Thread(doWork);
                _thread.Start();
            }


            /*
             * Multithreading code is tricky at best.  I followed the advice from the following website for our simple producer/consumer queue:
             * http://www.albahari.com/threading/part4.aspx#_UsingWaitPulse
             */

            private bool _isDone;
            private void doWork() {
                SearchHost.debug("Thread id for processing " + _title);
                _isDone = false;
                while (!_isDone) {
                    lock (_locker) {
                        Monitor.Wait(_locker, _timeout);
                        _callback();
                    }
                }
            }

            public void Dispose() {
                try {
                    _isDone = true;
                    debug(_title + " thread flagged so it should exit");
                    lock (_locker) {
                        Monitor.PulseAll(_locker);
                    }
                    debug(_title + " thread now joining caller thread...");
                    _thread.Join();
                    debug(_title + " thread now exiting.");
                } catch { }
            }
        }

		Indexer<BPlusString, BPlusListLong> _indexer;

        /// <summary>
        /// Will validate the search engine's config file is valid xml and contains all required values.  Throws an exception on failure.
        /// </summary>
		public static void ValidateConfig() {
			// null means lookup file name from app.config.  if that doesn't exist, uses a default of ~/gringlobal.search.config
            var dbDown = false;
            List<string> loadedIndexes = null;
			Indexer<BPlusString, BPlusListLong> indexer = Indexer<BPlusString, BPlusListLong>.InitializeFromConfig(null, ref dbDown, out loadedIndexes, null);
		}

        private string eventLogSource = "GRIN-Global Search Engine";
		private string eventLogFile;

		public delegate void ProgressEventHandler(object sender, ProgressEventArgs pea);
		public event ProgressEventHandler OnProgress;


        private ThreadQueuer _indexManipulationQueuer;
        private ThreadMonitor _clientDisconnectMonitor;
        private ThreadQueuer _realtimeUpdateQueuer;

		private void startIndexManipulationThread(ThreadQueuer queuer, QueuedCallback qc) {

            // precache items by searching for them...
            // initCaches();

            // check for an init file
            try {
                var initFile = Toolkit.ResolveFilePath(_indexer.FolderName + @"\recreate.init", false);
                if (File.Exists(initFile)) {
                    var indexNames = new List<string>();
                    indexNames.AddRange(File.ReadAllText(initFile).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                    File.Delete(initFile);
                    RebuildIndexes(indexNames, true, true);
                }
            } catch {
                // eat all errors around init stuff, as we want to guarantee the engine itself runs even if init stuff fails
            }

		}

        private void clientDisconnectMonitorCallback() {

            // this thread does nothing but monitor client connections.
            // if a client is disconnected, abort its query thread.
            // So if a client fires off a really long-running query, it will
            // be killed off if they disconnect

            if (__wcfClients != null) {
                var clients = __wcfClients.Keys.ToList();
                foreach(var client in clients){
                    if (client != null) {
                        if (client.State == CommunicationState.Faulted || client.State == CommunicationState.Closed) {
                            // client is disconnected or failed in some way.
                            // kill off the corresponding thread
                            ClientMonitor cm;
                            if (__wcfClients.TryGetValue(client, out cm)){
                                if (client.State == CommunicationState.Faulted) {
                                    if (cm.Thread.IsAlive && cm.Thread.IsBackground) {
                                        logInfo("Forcefully aborting long running query '" + cm.SearchString + "'.");
                                        cm.Thread.Abort();
                                    }
                                }
                                __wcfClients.Remove(client);
                            }
                        }
                    }
                }
            }

        }

        private static object __indexerLock = new object();

		private void initIndexer() {

            if (__wcfClients == null) {
                __wcfClients = new Dictionary<IContextChannel, ClientMonitor>();
            }

            // we want to avoid the locking expense on every call, so we use
            // a double-nested if check.  This makes sure we don't doubly-init the indexer
            // if it is hit by multiple requests while it is initializing.
			if (_indexer == null) {
                lock (__indexerLock) {
                    if (_indexer == null) {

                        //if (Debugger.IsAttached) {
                        //    eventLogFile = Toolkit.ResolveFilePath(Toolkit.GetSetting("SearchEngineLogFile", "~/logs/GrinGlobalSearchService.log"), true);
                        //} else {
                        eventLogFile = Toolkit.ResolveFilePath(Toolkit.GetSetting("SearchEngineLogFile", "*COMMONAPPLICATIONDATA*/GRIN-Global/GRIN-Global Search Engine/logs/GrinGlobalSearchService.log"), true);
                        //}
                        if (File.Exists(eventLogFile)) {
                            logFileSize = new FileInfo(eventLogFile).Length;
                        } else {
                            File.Create(eventLogFile).Close();
                            logFileSize = 0;
                        }

                        var isDatabaseDown = false;
                        List<string> loadedIndexNames = null;
                        _indexer = Indexer<BPlusString, BPlusListLong>.InitializeFromConfig(null, ref isDatabaseDown, out loadedIndexNames, _indexer_OnProgress);
                        //_indexer.OnProgress += new Indexer<BPlusString, BPlusListLong>.ProgressEventHandler(_indexer_OnProgress);

                        logInfo(new ProgressEventArgs(getDisplayMember("initIndexer{startup}", "{0} database backend, root index file folder: {1}", _indexer.DataConnectionSpec.ProviderName, Toolkit.ResolveDirectoryPath(_indexer.FolderName, false)), EventLogEntryType.Information, true));


                        // start background threads for serving queries, recreating indexes, performing updates, etc.
                        if (_indexManipulationQueuer != null) {
                            _indexManipulationQueuer.Dispose();
                        }
                        if (_clientDisconnectMonitor != null) {
                            _clientDisconnectMonitor.Dispose();
                        }
                        if (_realtimeUpdateQueuer != null) {
                            _realtimeUpdateQueuer.Dispose();
                        }

                        // there might be a file signifying we need to create index(es), so we want to do that immediately
                        // when the thread starts (queue it up immediately)
                        _indexManipulationQueuer = new ThreadQueuer(this, startIndexManipulationThread, null, "Index Manipulation");

                        // real time updater has no special startup code
                        _realtimeUpdateQueuer = new ThreadQueuer(this, null, null, "Realtime Updates");

                        // client disconnect monitor is special in the fact that it polls all clients to see if any have disconnected
                        // if they have, it forcefully ends the associated thread.  The other governors are reactive only, meaning
                        // they do nothing until something is put into their queues.
                        _clientDisconnectMonitor = new ThreadMonitor(clientDisconnectMonitorCallback, "Client Disconnects", 5000);

                        var msg = getDisplayMember("initIndexer{ready}", "Ready to answer search queries on the following indexes ({0}):\r\n", loadedIndexNames.Count.ToString(), String.Join(", ", loadedIndexNames.ToArray()));
                        logInfo(new ProgressEventArgs(msg, EventLogEntryType.Information, true));
                    }
                }
			}
		}

		void _indexer_OnProgress(object sender, ProgressEventArgs pea) {
			logInfo(pea);
		}

        private void initCaches() {

            // simply perform a search for each configured precache keyword so the results are auto-cached at startup
            initIndexer();

            if (Toolkit.GetSetting("DisablePrecaching", false)) {
                logInfo("Precache disabled via config setting 'DisableCaching' = true");
            } else {
                int count = _indexer.PrecacheKeywords.Count;
                if (count == 0) {
                    logInfo("No keywords configured for caching (no nodes existed in config file at xpath /Indexes/PrecacheKeywords/Keyword)");
                } else {
                    foreach (var precache in _indexer.PrecacheKeywords) {
                        using (HighPrecisionTimer hpt = new HighPrecisionTimer("precache", true)) {
                            logInfo("Precaching keyword '" + precache.Keyword + "' for resolver " + precache.ResolverName + "...");
                            var ret = this.Search(precache.Keyword, precache.IgnoreCase, true, null, precache.ResolverName.ToLower(), 0, 0, null, null);
                            var ds = Toolkit.DataSetFromXml(ret);
                            logInfo("Precached " + ds.Tables["SearchResult"].Rows.Count + " IDs for '" + precache.Keyword + "' in resolver " + precache.ResolverName + " in " + Toolkit.FormatTimeElapsed(hpt.ElapsedMilliseconds));
                        }
                    }
                    logInfo("Precached " + count + " keyword(s)");
                }
            }

        }

		/// <summary>
		/// Writes an informational message to the log file but not the event log
		/// </summary>
		/// <param name="message"></param>
		private void logInfo(string message) {
			logInfo(new ProgressEventArgs(message, EventLogEntryType.Information, false));
		}

		private void logInfo(ProgressEventArgs pea) {

			if (pea == null) {
				return;
			}


            if (OnProgress != null) {
                OnProgress(this, pea);
            }

            try {
			    if (pea.WriteToEventLog || pea.LogType == EventLogEntryType.Error || pea.LogType == EventLogEntryType.Warning) {
				    EventLog.WriteEntry(eventLogSource, pea.Message, pea.LogType);
			    }
            } catch {
                // always eat errors writing to event log
            }

			// we always always always write to the log file
            logToFile(pea);

		}

		// 16 MB log file is the largest we allow
		private const int MAX_LOG_FILE_SIZE = 16777216;

		private long logFileSize;

        private object _logLockObject = new object();

		private void logToFile(ProgressEventArgs pea) {

			if (pea == null) {
				return;
			}

            lock (_logLockObject) {
                if (logFileSize > MAX_LOG_FILE_SIZE) {
                    // rotate out the log file, it's too big
                    File.Move(eventLogFile, eventLogFile.Replace(".log", "." + DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss") + ".log"));
                    logFileSize = 0;
                }

                using (StreamWriter sw = File.AppendText(eventLogFile)) {
                    logFileSize += Toolkit.OutputTabbedData(new object[] { DateTime.Now.ToString(), pea.LogType.ToString(), pea.Message }, sw, false);
                    //logFileSize += Toolkit.OutputCSV(new object[] { DateTime.Now.ToString(), pea.LogType.ToString(), pea.Message }, sw, false);
                }
            }
		}

		public SearchHost(ProgressEventHandler onProgress) {
			if (onProgress != null) {
				this.OnProgress += onProgress;
			}
			try {
				initIndexer();
			} catch {
				if (_indexer != null) {
					_indexer.Dispose();
					_indexer = null;
				}
				throw;
			}
		}

		public SearchHost() : this(null) {
		}

        /// <summary>
        /// Returns a list of name of the indexes in the search engine.  Can optionally return only those that are enabled (i.e. currently searchable)
        /// </summary>
        /// <param name="enabledOnly"></param>
        /// <returns></returns>
		public List<string> ListIndexes(bool enabledOnly) {
			initIndexer();
			var output = new List<string>();
			foreach (string key in _indexer.GetIndexNames()) {
				Index<BPlusString, BPlusListLong> idx = _indexer.GetIndex(key);
				if (idx.Enabled || !enabledOnly) {
					output.Add(idx.Name);
				}
			}
			return output;
		}

        /// <summary>
        /// Returns information about the search engine, such as index information, status counts (# of realtime updates pending, index creations pending, etc), etc.  Optionally only those that are currently enabled.  DataTable names are: indexer, index, index_field, resolver, and status.
        /// </summary>
        /// <param name="enabledIndexesOnly"></param>
        /// <returns></returns>
        public string GetInfo(bool enabledIndexesOnly) {
            initIndexer();

            var ds = _indexer.FillDataSetFromConfig(null, enabledIndexesOnly, null, null);

            var dtStatus = ds.Tables["status"];
            var drStatus = dtStatus.NewRow();
            drStatus["pending_realtime_updates"] = _realtimeUpdateQueuer.PendingWorkItemCount;
            drStatus["processing_indexes"] = _indexManipulationQueuer.PendingWorkItemCount;
            drStatus["connected_clients"] = __wcfClients.Keys.Count;
            dtStatus.Rows.Add(drStatus);

            return Toolkit.DataSetToXml(ds);
        }

        /// <summary>
        /// Returns information about the search engine, such as index information, status counts (# of realtime updates pending, index creations pending, etc), etc.  Optionally only those that are currently enabled.  DataTable names are: indexer, index, index_field, resolver, and status.
        /// </summary>
        /// <param name="enabledIndexesOnly"></param>
        /// <param name="onlyThisIndex"></param>
        /// <param name="onlyThisResolver"></param>
        /// <returns></returns>
        public string GetInfoEx(bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver) {
            initIndexer();

            var ds = _indexer.FillDataSetFromConfig(null, enabledIndexesOnly, onlyThisIndex, onlyThisResolver);

            var dtStatus = ds.Tables["status"];
            var drStatus = dtStatus.NewRow();
            drStatus["pending_realtime_updates"] = _realtimeUpdateQueuer.PendingWorkItemCount;
            drStatus["processing_indexes"] = _indexManipulationQueuer.PendingWorkItemCount;
            drStatus["connected_clients"] = __wcfClients.Keys.Count;
            dtStatus.Rows.Add(drStatus);

            return Toolkit.DataSetToXml(ds);
        }

        /// <summary>
        /// Returns status information about the search engine
        /// </summary>
        /// <returns></returns>
        public string GetStatus() {
            initIndexer();

            var ds = Indexer<BPlusString, BPlusListLong>.CreateDefaultDataSet();

            var dtStatus = ds.Tables["status"];
            var drStatus = dtStatus.NewRow();
            drStatus["pending_realtime_updates"] = _realtimeUpdateQueuer.PendingWorkItemCount;
            drStatus["processing_indexes"] = _indexManipulationQueuer.PendingWorkItemCount;
            drStatus["connected_clients"] = __wcfClients.Keys.Count;
            dtStatus.Rows.Add(drStatus);

            return Toolkit.DataSetToXml(ds);
        }

        /// <summary>
        /// Returns a chronologically descending comma separated value list of actions that were logged to the search engine's log file.
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <returns></returns>
		public string GetLatestMessages(int maxMessages) {
			initIndexer();
			IEnumerable<List<string>> allLines = null;
			lock (_indexer) {
				//allLines = Toolkit.ParseCSV(eventLogFile, true);
                allLines = Toolkit.ParseTabDelimited(eventLogFile, true);
            }
			if (allLines == null) {
				return string.Empty;
			} else {
				// first, reverse them
				allLines = allLines.Reverse();
				int count = 0;

				var sw = new StringWriter();
                foreach (List<string> line in allLines) {
					//Toolkit.OutputCSV(line.ToArray(), sw, false);
                    var arr = line.ToArray();
                    if (arr.Length < 2) {
                        // obviously didn't work properly, assume 'old' csv format and ignore.
                    } else {
                        Toolkit.OutputTabbedData(line.ToArray(), sw);
                        count++;
                    }
					if (count >= maxMessages){
						break;
					}
				}
				return sw.ToString();
			}
		}

        /// <summary>
        /// Returns a list of Hit objects matching the given criterion.
        /// </summary>
        /// <param name="searchString">The term(s) to search by</param>
        /// <param name="ignoreCase">False to perform a case-sensitive search, True otherwise</param>
        /// <param name="autoAndConsecutiveLiterals">True to 'and' all terms together (reducing list of hits), False to 'or' all terms together (lengthening list of hits)</param>
        /// <param name="indexNames">Name(s) of index(es) to search</param>
        /// <param name="resolverName">The name of the resolver to retrieve primary key id's from.  Must be defined in the search engine's config file.</param>
        /// <param name="returnHitsWithNoResolvedIDs"></param>
        /// <param name="offset">Number of Hits to skip (i.e. do not return first 25 hits)</param>
        /// <param name="limit">Maximum number of Hits to return</param>
        /// <returns></returns>
        public List<ResolvedHitData> SearchForHits(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, bool returnHitsWithNoResolvedIDs, int offset, int limit) {
            throw new NotImplementedException();
            //initIndexer();
            //try {
            //    return _indexer.SearchForHits(searchString, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, returnHitsWithNoResolvedIDs, offset, limit).ToList();
            //} catch (Exception ex) {
            //    // don't bring down the service, just log and continue
            //    StringBuilder sb = new StringBuilder();
            //    if (indexNames != null) {
            //        foreach (string name in indexNames) {
            //            sb.Append(name).Append(", ");
            //        }
            //    }
            //    if (sb.Length > 2) {
            //        sb.Remove(sb.Length - 2, 2);
            //    }
            //    logInfo(new ProgressEventArgs("Error ocurred while searching for '" + searchString + ", ignoreCase=" + ignoreCase + "; autoAndConsecutiveLiterals=" + autoAndConsecutiveLiterals + "; indexNames=" + sb.ToString() + ".  Error: " + ex.Message + ".\r\nAdditional info: " + ex.ToString(), EventLogEntryType.Error, true));
            //    throw;
            //}
        }

        private class ClientMonitor {
            internal Thread Thread;
            internal string SearchString;
        }

        private Dictionary<IContextChannel, ClientMonitor> __wcfClients;



        /// <summary>
        /// Returns a list of integers (the primary key id's from the table given by resolverName parameter) matching the given criterion
        /// </summary>
        /// <param name="searchString">The term(s) to search by</param>
        /// <param name="ignoreCase">False to perform a case-sensitive search, True otherwise</param>
        /// <param name="autoAndConsecutiveLiterals">True to 'and' all terms together (reducing list of hits), False to 'or' all terms together (lengthening list of hits)</param>
        /// <param name="indexNames">Name(s) of index(es) to search</param>
        /// <param name="resolverName">The name of the resolver to retrieve primary key id's from.  Must be defined in the search engine's config file.</param>
        /// <param name="offset">Number of Hits to skip (i.e. do not return first 25 hits)</param>
        /// <param name="limit">Maximum number of Hits to return</param>
        /// <param name="options">Equal/semi-colon delimited keyword pairs of options for query.  e.g.: passthru=NonIndexed;debug=true;</param>
        /// <returns></returns>
        public string Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, int offset, int limit, string dataSetAsXml, string options) {
			initIndexer();
			try {

                monitorRequest(searchString);

                var dsInput = Toolkit.DataSetFromXml(dataSetAsXml);

				var ret = _indexer.Search(searchString, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, offset, limit, dsInput, options);

                // Debugging cancelling long-running search when client disconnects in the middle of processing
                //if (Debugger.IsAttached) {
                //    Thread.Sleep(10000);
                //}
                Console.WriteLine("search returned " + (ret == null || ret.Tables["SearchResult"] == null ? -1 : ret.Tables["SearchResult"].Rows.Count) + " rows, parsed query=" + (ret == null || ret.Tables["SearchQuery"] == null || ret.Tables["SearchQuery"].Rows.Count == 0 ? " -- unknown -- " : ret.Tables["SearchQuery"].Rows[0]["parsedSearchString"].ToString()));
                var output = Toolkit.DataSetToXml(ret);
                Console.WriteLine(output);
                return output;

			} catch (Exception ex) {
				// don't bring down the service, just log and continue
				StringBuilder sb = new StringBuilder();
				if (indexNames != null) {
					foreach (string name in indexNames) {
						sb.Append(name).Append(", ");
					}
				}
				if (sb.Length > 2) {
					sb.Remove(sb.Length - 2, 2);
				}
				logInfo(new ProgressEventArgs(getDisplayMember("Search{error}", "Error occurred while searching for '{0}, ignoreCase={1}; autoAndConsecutiveLiterals={2}; indexNames={3}; resovler={4}.  Error: {5}.\r\nAdditional info: {6}"
                    , searchString, ignoreCase.ToString(), autoAndConsecutiveLiterals.ToString(), sb.ToString(), 
                    resolverName, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
				throw;
			}
		}

        private void monitorRequest(string searchString) {
            var ctx = OperationContext.Current;
            if (ctx != null) {
                IContextChannel client = ctx.Channel;
                if (client != null) {
                    // if client disconnects / dies / times out / whatever, stop processing their request
                    __wcfClients[client] = new ClientMonitor { Thread = Thread.CurrentThread, SearchString = searchString };
                    client.Faulted += new EventHandler(client_Faulted);
                    client.Closed += new EventHandler(client_Closed);
                }
            }
        }

        void client_Closed(object sender, EventArgs e) {
            killClient((IContextChannel)sender);
        }

        private void killClient(IContextChannel client){
            if (__wcfClients != null) {
                try {
                    if (client.State == CommunicationState.Faulted) {
                        ClientMonitor cm;
                        if (__wcfClients.TryGetValue(client, out cm)) {
                            try {
                                if (cm.Thread.IsAlive && cm.Thread.IsBackground) {
                                    logInfo("Forcefully aborting long running query '" + cm.SearchString + "'.");
                                    cm.Thread.Abort();
                                }
                            } catch { }
                        }
                    }
                } finally {
                __wcfClients.Remove(client);
                }
            }
        }

        void client_Faulted(object sender, EventArgs e) {
            // client disconnected, abort processing their query if we can
            killClient((IContextChannel)sender);
        }

		private Index<BPlusString, BPlusListLong> getIndex(string indexName) {
			initIndexer();
            return _indexer.GetIndex(indexName);
		}


		private List<Index<BPlusString, BPlusListLong>> getIndexes(List<string> indexNames) {
			List<Index<BPlusString, BPlusListLong>> indexes = new List<Index<BPlusString, BPlusListLong>>();

			initIndexer();

			if (indexNames == null || indexNames.Count == 0) {
				// get all of them
                foreach (string name in _indexer.GetIndexNames()) {
                    indexes.Add(_indexer.GetIndex(name));
                }
			} else {
				// get only those they specified (if they gave us bogus index names, ignore them)
				foreach (string name in indexNames) {
                    Index<BPlusString, BPlusListLong> idx = _indexer.GetIndex(name);
                    if (idx != null){
						indexes.Add(idx);
					}
				}
			}

			return indexes;
		}

        /// <summary>
        /// Moves files for the given index(es) from the rotation folder to the actively searched folder.  Since index generation is a lengthy process, they are created in a rotation folder so searches may continue during generation.  Only during rotation are searches queued up until rotation is completed.
        /// </summary>
        /// <param name="indexNames"></param>
		public void RotateIndexes(List<string> indexNames) {
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (idx.Enabled) {
					try {
						idx.Rotate(false);
					} catch (Exception ex) {
						logInfo(new ProgressEventArgs(getDisplayMember("RotateIndexes", "Rotating index '{0}' failed: {1}. {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				}
			}
		}

        /// <summary>
        /// Always returns true.  Used simply to verify connection between a client and the search engine (for troubleshooting configuration issues)
        /// </summary>
        /// <returns></returns>
		public bool Ping() {
			return true;
		}


		#region IDisposable Members

		public void Dispose() {
			logInfo(new ProgressEventArgs(getDisplayMember("Dispose", "Attempting to gracefully shut down search engine..."), EventLogEntryType.Information, true));

            if (_realtimeUpdateQueuer != null) {
                _realtimeUpdateQueuer.Dispose();
            }
            if (_indexManipulationQueuer != null) {
                _indexManipulationQueuer.Dispose();
            }
            if (_clientDisconnectMonitor != null) {
                _clientDisconnectMonitor.Dispose();
            }

            if (_indexer != null) {
				_indexer.Dispose();
			}

            debug("All background threads have now exited.");
            logInfo(new ProgressEventArgs(getDisplayMember("Dispose{done}", "Search engine is now shut down."), EventLogEntryType.Information, true));
        }

		#endregion

		#region ISearchHost Members

        private void removeKeywords(string[] keywordsToRemove, Index<BPlusString, BPlusListLong> idx, int rowID, SearchOptions opts) {

            if (keywordsToRemove != null && keywordsToRemove.Length > 0) {
                BPlusListLong newList = new BPlusListLong();

                for (int i = 0; i < keywordsToRemove.Length; i++) {
                    string keyword = keywordsToRemove[i];

                    List<KeywordMatch<BPlusString, BPlusListLong>> matches = idx.Search(keyword, KeywordMatchMode.ExactMatch, false);

                    if (matches == null) {
                        // something is wrong, idx.Search should always return a collection (even if it's null).
                        // cancel processing.
                        return;
                    }

                    if (matches.Count == 0) {
                        // keyword does not exist. nothing to do. ignore / continue.
                    } else {
                        // keyword exists.

                        // first remove from search command cache
                        SearchCommand<BPlusString, BPlusListLong>.ClearCache(idx, keyword);

                        // zero out all hits that match the rowID
                        // update the entry in the tree (if needed)
                        foreach (KeywordMatch<BPlusString, BPlusListLong> m in matches) {
                            BPlusListLong bpll = m.Value as BPlusListLong;
                            if (bpll == null || bpll.Value == null || bpll.Value.Count == 0) {
                                // no list of data items to point at (should never happen). nothing to do.
                            } else {
                                // for each long in our list, go to that position in the data file.
                                // for all hits in that portion of the data file that match 
                                // our rowID, overwrite rowID with a 0 (effectively deleting them from the mix)

                                foreach (long startPosition in bpll.Value) {
                                    int totalForThisID = 0;
                                    int hitsForThisID = idx.ZeroOutHit(startPosition, rowID, out totalForThisID);
                                    if (hitsForThisID > 0 && totalForThisID == hitsForThisID) {
                                        // all hits for this keyword were removed.
                                        // mark that entire portion of the data file as being bogus
                                        newList.Value.Add(startPosition);
                                    }
                                }
                            }
                            // only update the tree if we need to 
                            // (i.e. if ALL hits for a given start position were removed, we should
                            //  remove that start position from the tree's list for this keyword)
                            // note this orphans the data in the file -- but it's been zero'ed out anyway. shouldn't matter.
                            if (newList.Value.Count > 0) {
                                idx.UpdateTree(m, newList, UpdateMode.Subtract);
                            }
                        }

                    }
                }



                // now update the resolver information.
                // resolvers come in two flavors:
                // 1. PK ID from Index as its keyword
                // 2. Keyword from Index as its keyword
                foreach (var key in idx.Resolvers.Keys) {
                    var r = idx.Resolvers[key];

                    // we never update resolvers that aren't enabled
                    if (r.Enabled) {

                        // never update resolvers that have caching turned off
                        // Also, IdAndKeyword updates are prohibitively expensive to do realtime updates with.
                        // So, this only applies to CacheMode = 'Id'.
                        if (r.AllowRealTimeUpdates && r.CacheMode == ResolverCacheMode.Id) {

                            // we already updated the index to include the new keyword.
                            // grab all the IDs as reported by the index
                            // then using that list grab all resolved ids.
                            // If our new rowID does not appear in that list,
                            // we need to add it to the resolver that skips the index lookup
                            // during search requests.
                            foreach (string keyword in keywordsToRemove) {

                                // We need to update the id-based resolver tree...

                                // pull all resolved id's from the index for the current keyword and rowID
                                var pkBasedResolvedIDs = idx.getResolvedIDs(keyword, rowID, r.Name, null, opts);

                                // pull all resolved ids for the current row id from the database
                                var dbBasedResolvedIDs = new List<int>();
                                var cmd = r.createDataCommand(rowID);
                                if (cmd != null) {
                                    try {
                                        using (var dm = DataManager.Create()) {
                                            using (var idr = dm.Stream(cmd)) {
                                                int ordinal = -1;
                                                while (idr.Read()) {
                                                    if (ordinal < 0) {
                                                        ordinal = idr.GetOrdinal(r.ResolvedPrimaryKeyField);
                                                    }
                                                    if (!idr.IsDBNull(ordinal)) {
                                                        dbBasedResolvedIDs.Add(idr.GetInt32(ordinal));
                                                    }
                                                }
                                            }
                                        }
                                    } catch (Exception ex) {
                                        this.logInfo(new ProgressEventArgs(getDisplayMember("removeKeywords{failedlookup}", "In SearchHost.removeKeywords, failed looking up keyword '{0}' from database where primary key id={1} for resolver '{2}' for index '{3}': {4}. SQL: {5}", keyword, rowID.ToString(), r.Name, idx.Name, ex.Message, cmd.ProcOrSql), EventLogEntryType.Error, false));
                                    }
                                }

                                // determine the disparity between the database and the resolved ids reported from the pk-based resolver tree
                                if (pkBasedResolvedIDs.Count > 0) {
                                    // nothing in the index for this resolver / keyword / row.
                                    // add all the db resolved ids
                                    if (dbBasedResolvedIDs.Count > 0) {
                                        var extras = pkBasedResolvedIDs.Except(dbBasedResolvedIDs).ToList();
                                        if (extras.Count > 0) {
                                            r.RemoveResolvedIDs(rowID, extras);
                                        }
                                    }
                                }





                                if (r.CacheMode == ResolverCacheMode.IdAndKeyword) {
                                    // there is also a keyword-based resolver tree to update as well.

                                    // Get a list of all ids already in the pk-based resolver tree (the one we just updated)
                                    var idsResolvedByPKID = idx.getResolvedIDs(keyword, r.Name, null, opts);

                                    // Get a list of all ids already in the keyword-based resolver tree (we don't care about rowID since that isn't applicable to this kind of resolver tree)
                                    var idsResolvedByKeyword = r.GetResolvedIDsViaIdAndKeyword(keyword, KeywordMatchMode.ExactMatch, false, opts);

                                    if (idsResolvedByKeyword.Count > 0) {
                                        if (idsResolvedByPKID.Count > 0) {
                                            // pk-based resolver tree may have a smaller list than the keyword-based resolver tree.
                                            var extras2 = idsResolvedByKeyword.Except(idsResolvedByPKID).ToList();
                                            if (extras2.Count > 0) {
                                                r.RemoveResolvedIDs(keyword, extras2);
                                            }
                                        } else {
                                            // pk-based resolve tree contains none.
                                            r.RemoveResolvedIDs(keyword, idsResolvedByKeyword);
                                        }
                                    }

                                }
                            }
                        }
                    }

                    // update the resolver that is by keyword (if any)
                    // update any resolver cache(s)
                    //foreach (var key in idx.Resolvers.Keys) {
                    //    var r = idx.Resolvers[key];
                    //    if (r.EnableByKeyword) {
                    //        var idList = idLists[key];
                    //        if (idList.Count > 0) {
                    //            // the resolver cache knows to create a node / update data file as needed,
                    //            // we just need to tell it to add those IDs.
                    //            r.AddResolvedIDs(keyword, idList);
                    //        }
                    //    }
                    //}
                }


                //// now update the resolver information.
                //// resolvers come in two flavors:
                //// 1. PK ID from Index as its keyword
                //// 2. Keyword from Index as its keyword
                //foreach (var key in idx.Resolvers.Keys) {
                //    var r = idx.Resolvers[key];

                //    // we already updated the index to include the new keyword.
                //    // grab all the IDs as reported by the index
                //    // then using that list grab all resolved ids.
                //    // If our new rowID does not appear in that list,
                //    // we need to add it to the resolver that skips the index lookup
                //    // during search requests.
                //    foreach (string keyword in keywordsToRemove) {

                //        // row id is the 'new' row in the index
                //        // pull all resolved ids for that id from the database


                //        if (r.Method == ResolutionMethod.Sql) {
                //            // there is a id-based resolver tree we need to update.
                //            // lookup data from database for current id (rowID)
                //            var dbIDResolvedIDs = new List<int>();
                //            using (var dm = DataManager.Create()) {
                //                var cmd = r.createDataCommand(rowID);
                //                using (var idr = dm.Stream(cmd)) {
                //                    int ordinal = -1;
                //                    while (idr.Read()) {
                //                        if (ordinal < 0) {
                //                            ordinal = idr.GetOrdinal(r.ResolvedPrimaryKeyField);
                //                        }
                //                        dbIDResolvedIDs.Add(idr.GetInt32(ordinal));
                //                    }
                //                }
                //            }

                //            // get list of resolved ids as reported by our index
                //            var indexResolvedIDs = idx.getResolvedIDs(keyword, rowID, r.Name);

                //            // determine the disparity between the database and the resolved ids reported from the index
                //            var extras = indexResolvedIDs.Except(dbIDResolvedIDs).ToList();
                //            if (extras.Count > 0) {
                //                // and remove as needed from the id-based resolver tree
                //                r.RemoveResolvedIDs(rowID, extras.ToList());
                //            }

                //        }




                //        if (r.EnableByKeyword) {
                //            // there is also a keyword-based resolver tree to update as well.

                //            // list of resolved id's from the database
                //            var dbKeywordResolvedIDs = new List<int>();

                //            // list of id's to query the db with
                //            var indexIDs = idx.getPrimaryKeyIDs(keyword);

                //            // query db to get resolved id's for the given list of ids from the index (which was just updated with the proper values)
                //            var cmd = r.createDataCommand(indexIDs);
                //            if (cmd != null) {
                //                using (var dm = DataManager.Create()) {
                //                    using (var idr = dm.Stream(cmd)) {
                //                        int ordinal = -1;
                //                        while (idr.Read()) {
                //                            if (ordinal < 0) {
                //                                ordinal = idr.GetOrdinal(r.ResolvedPrimaryKeyField);
                //                            }
                //                            dbKeywordResolvedIDs.Add(idr.GetInt32(ordinal));
                //                        }
                //                    }
                //                }
                //            }

                //            // get list of resolved id's as currently recorded in the keyword-based resolver tree
                //            var keywordResolvedIDs = r.GetResolvedIDs(keyword, KeywordMatchMode.ExactMatch, false);

                //            // determine the disparity between the database and the resolved ids reported from the index
                //            var extras = keywordResolvedIDs.Except(dbKeywordResolvedIDs).ToList();
                //            if (extras.Count > 0) {
                //                // and remove as needed from the keyword-based resolver tree.
                //                r.RemoveResolvedIDs(keyword, extras.ToList());
                //            }
                //        }
                //    }
                //}
            }
        }

        private void addKeywords(string[] keywordsToAdd, string fieldName, Index<BPlusString, BPlusListLong> idx, int rowID, SearchOptions opts) {

            if (keywordsToAdd != null && keywordsToAdd.Length > 0) {
                BPlusListLong newList = new BPlusListLong();


                // update the index and the resolver that is by PKID
                for (int i = 0; i < keywordsToAdd.Length; i++) {
                    string keyword = keywordsToAdd[i];

                    // first remove from search command cache as it is now outdated
                    SearchCommand<BPlusString, BPlusListLong>.ClearCache(idx, keyword);

                    List<KeywordMatch<BPlusString, BPlusListLong>> matches = idx.Search(keyword, KeywordMatchMode.ExactMatch, false);

                    if (matches == null) {
                        // something is wrong, idx.Search should always return a collection (even if it's null).
                        // cancel processing.
                        return;
                    }

                    // add the row.ID to the end of the index's data file (if we haven't already)
                    if (newList.Value.Count == 0) {
                        Hit h = idx.CreateNewHit(fieldName, rowID, i);
                        long pos = idx.AppendHitToData(h);
                        newList.Value.Add(pos);
                    }

                    if (matches.Count == 0) {
                        // keyword does not yet exist.
                        // add it to the index tree along with the pointer to the new datafile item
                        var nm = idx.InsertIntoTree(keyword, newList);
                        matches.Add(nm);
                    } else {
                        // keyword exists.
                        // update the entry in the index tree (add items in newList to the existing list in the proper Value index of the node)
                        foreach (var m in matches) {
                            idx.UpdateTree(m, newList, UpdateMode.Add);
                        }
                    }
                }

                // now update the resolver information.
                // resolvers come in two flavors:
                // 1. PK ID from Index as its keyword
                // 2. Keyword from Index as its keyword
                foreach (var key in idx.Resolvers.Keys) {
                    var r = idx.Resolvers[key];

                    // we never update resolvers that aren't enabled
                    if (r.Enabled) {

                        // never update resolvers that have caching turned off
                        // Also, IdAndKeyword updates are prohibitively expensive to do realtime updates with.
                        // So, this only applies to CacheMode = 'Id'.
                        if (r.AllowRealTimeUpdates && r.CacheMode == ResolverCacheMode.Id) {

                            // we already updated the index to include the new keyword.
                            // grab all the IDs as reported by the index
                            // then using that list grab all resolved ids.
                            // If our new rowID does not appear in that list,
                            // we need to add it to the resolver that skips the index lookup
                            // during search requests.
                            foreach (string keyword in keywordsToAdd) {

                                // We need to update the id-based resolver tree...

                                // pull all resolved id's from the index for the current keyword and rowID
                                var pkBasedResolvedIDs = idx.getResolvedIDs(keyword, rowID, r.Name, null, opts);

                                // pull all resolved ids for the current row id from the database
                                var dbBasedResolvedIDs = new List<int>();
                                var cmd = r.createDataCommand(rowID);
                                try {
                                    if (cmd != null) {
                                        using (var dm = DataManager.Create()) {
                                            using (var idr = dm.Stream(cmd)) {
                                                int ordinal = -1;
                                                while (idr.Read()) {
                                                    if (ordinal < 0) {
                                                        ordinal = idr.GetOrdinal(r.ResolvedPrimaryKeyField);
                                                    }
                                                    if (!idr.IsDBNull(ordinal)) {
                                                        dbBasedResolvedIDs.Add(idr.GetInt32(ordinal));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                } catch (Exception ex) {
                                    this.logInfo(new ProgressEventArgs(getDisplayMember("addKeywords{lookupfailed}", "In SearchHost.addKeywords, failed looking up keyword '{0}' from database where primary key id={1} for resolver '{2}' for index '{3}': {4}. SQL: {5}", keyword, rowID.ToString(), r.Name, idx.Name, ex.Message, cmd.ProcOrSql), EventLogEntryType.Error, false));
                                }

                                // determine the disparity between the database and the resolved ids reported from the pk-based resolver tree
                                if (pkBasedResolvedIDs.Count == 0) {
                                    // nothing in the index for this resolver / keyword / row.
                                    // add all the db resolved ids
                                    if (dbBasedResolvedIDs.Count > 0) {
                                        r.AddResolvedIDs(rowID, dbBasedResolvedIDs);
                                    }
                                } else if (dbBasedResolvedIDs.Count > 0) {

                                    // pk-based resolver tree contains resolved ids already, and so does the database.
                                    // only add those that don't exist in the pk-based resolver tree already.
                                    var missing = dbBasedResolvedIDs.Except(pkBasedResolvedIDs).ToList();
                                    if (missing.Count > 0) {
                                        // and add it to the id-based resolver tree
                                        r.AddResolvedIDs(rowID, missing);
                                    }
                                }





                                if (r.CacheMode == ResolverCacheMode.IdAndKeyword) {
                                    // there is also a keyword-based resolver tree to update as well.

                                    // Get a list of all ids already in the pk-based resolver tree (the one we just updated)
                                    var idsResolvedByPKID = idx.getResolvedIDs(keyword, r.Name, null, opts);

                                    // Get a list of all ids already in the keyword-based resolver tree (we don't care about rowID since that isn't applicable to this kind of resolver tree)
                                    var idsResolvedByKeyword = r.GetResolvedIDsViaIdAndKeyword(keyword, KeywordMatchMode.ExactMatch, false, opts);

                                    if (idsResolvedByKeyword.Count == 0) {
                                        // add all the ones we found via the pk-based resolver tree as there are none currently in the keyword-based resolver tree
                                        if (idsResolvedByPKID.Count > 0) {
                                            r.AddResolvedIDs(keyword, idsResolvedByPKID);
                                        }
                                    } else if (idsResolvedByPKID.Count > 0) {
                                        // add all those that exist in the pk-based resolver tree but not already in the keyword-based resolver tree
                                        var except = idsResolvedByPKID.Except(idsResolvedByKeyword).ToList();
                                        if (except.Count > 0) {
                                            r.AddResolvedIDs(keyword, except);
                                        }
                                    }

                                }
                            }
                        }
                    }

                    // update the resolver that is by keyword (if any)
                    // update any resolver cache(s)
                    //foreach (var key in idx.Resolvers.Keys) {
                    //    var r = idx.Resolvers[key];
                    //    if (r.EnableByKeyword) {
                    //        var idList = idLists[key];
                    //        if (idList.Count > 0) {
                    //            // the resolver cache knows to create a node / update data file as needed,
                    //            // we just need to tell it to add those IDs.
                    //            r.AddResolvedIDs(keyword, idList);
                    //        }
                    //    }
                    //}
                }
            }
        }

        private void processReplaceKeywords(Index<BPlusString, BPlusListLong> idx, FieldValue val, int searchableFieldsIndex, int rowID, SearchOptions opts) {
            // throw new NotImplementedException("processReplaceKeywords() is not implemented yet.  call processAddKeywords instead.");
            throw new NotImplementedException();
        }

        private void processAddKeywords(Index<BPlusString, BPlusListLong> idx, FieldValue val, int searchableFieldsIndex, int rowID, SearchOptions opts) {

            if (val == null) {
                return;
            }

            // first, tokenize all words from the original value and the new value
            IEnumerable<string> origKeywords = null;
            IEnumerable<string> newKeywords = null;
            try {
                origKeywords = idx.Tokenize((val.OriginalValue == null ? "" : val.OriginalValue.ToString()), idx.SearchableFields[searchableFieldsIndex]).AsEnumerable();
                newKeywords = idx.Tokenize((val.NewValue == null ? "" : val.NewValue.ToString()), idx.SearchableFields[searchableFieldsIndex]).AsEnumerable();
            } catch (Exception ex) {
                throw new InvalidOperationException(getDisplayMember("processAddKeywords{tokenize}", "Error tokenizing keywords in processAddKeywords: {0}", ex.Message), ex);
            }

            // figure out keywords to remove
            string[] keywordsToRemove = null;
            string[] keywordsToAdd = null;
            try {
                keywordsToRemove = origKeywords.Except(newKeywords).ToArray();
                keywordsToAdd = newKeywords.Except(origKeywords).ToArray();
            } catch (Exception ex2) {
                throw new InvalidOperationException(getDisplayMember("processAddKeywords{compare}", "Error comparing keyword lists in processAddKeywords: {0}", ex2.Message), ex2);
            }


            try {
                removeKeywords(keywordsToRemove, idx, rowID, opts);
            } catch (Exception ex3) {
                throw new InvalidOperationException(getDisplayMember("processAddKeywords{remove}", "Error removing keywords: {0}", ex3.Message), ex3);
            }

            // figure out keywords to add
            try {
                addKeywords(keywordsToAdd, val.FieldName, idx, rowID, opts);
            } catch (Exception ex4) {
                throw new InvalidOperationException(getDisplayMember("processAddKeywords{add}", "Error adding keywords: {0}", ex4.Message), ex4);
            }

        }

        private void processSubtractKeywords(Index<BPlusString, BPlusListLong> idx, FieldValue val, int searchableFieldsIndex, int rowID, SearchOptions opts) {

            BPlusListLong newList = new BPlusListLong();

            IEnumerable<string> origKeywords = null;
            IEnumerable<string> newKeywords = null;
            try {
                // first, tokenize all words from the original value and the new value
                origKeywords = idx.Tokenize((val.OriginalValue == null ? "" : val.OriginalValue.ToString()), idx.SearchableFields[searchableFieldsIndex]).AsEnumerable();
                newKeywords = idx.Tokenize((val.NewValue == null ? "" : val.NewValue.ToString()), idx.SearchableFields[searchableFieldsIndex]).AsEnumerable();
            } catch (Exception ex) {
                throw new InvalidOperationException(getDisplayMember("processSubtractKeywords{tokenize}", "Error tokenizing keywords in processSubtractKeywords: {0}", ex.Message), ex);
            }

            // remove all the unique keywords
            string[] toRemove = null;

            try {
                toRemove = origKeywords.Union(newKeywords).ToArray();
            } catch (Exception ex2) {
                throw new InvalidOperationException(getDisplayMember("processSubtractKeywords{union}", "Error unioning keywords in processSubtractKeywords: {0}", ex2.Message), ex2);
            }

            try {
                removeKeywords(toRemove, idx, rowID, opts);
            } catch (Exception ex3) {
                throw new InvalidOperationException(getDisplayMember("processSubtractKeywords{remove}", "Error removing keywords in processSubtractKeywords: {0}", ex3.Message), ex3);
            }

        }

        private bool processDefrag(BPlusTreeLeafNode<BPlusString, BPlusListLong> leaf, BinaryReader reader, BinaryWriter writer) {

            bool updatedAtLeastOne = false;
            for(int i=0;i<leaf.Values.Count;i++){
                BPlusListLong longList = leaf.Values[i];
                if (longList != null && longList.Value != null) {
                    // we only need to defrag this one if it has more than one value in its list of pointers to the data file.
                    List<Hit> allHits = new List<Hit>();
                    foreach (long position in longList.Value) {
                        // get all hits from the datafile for this position
                        allHits.AddRange(Hit.ReadAll(reader, position, null, -1, -1));
                    }

                    // ok we've pulled all the data from the tree and data file we need to.

                    // write all the hits out (WriteAll takes care of hit ordering issues)
                    long writePosition = Hit.WriteAll(writer, allHits);

                    // update the tree with the pointer to the list of hits we wrote out
                    leaf.Values[i] = new BPlusListLong(writePosition);

                    updatedAtLeastOne = true;
                }
            }

            return updatedAtLeastOne;
        }

        /// <summary>
        /// Updates the given indexName for the given UpdateRow object
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="row"></param>
        public void UpdateIndexRow(string indexName, UpdateRow row) {
            List<UpdateRow> rows = new List<UpdateRow>();
            rows.Add(row);
            UpdateIndex(indexName, rows);
        }

        private FieldValue calculateFieldValues(Field fld, UpdateRow row, Index<BPlusString, BPlusListLong> idx) {

            FieldValue fv = new FieldValue { FieldName = fld.Name };

            // 2009-07-27 Brock Weaver brock@circaware.com
            //
            // currently we only support concatentation of subfields, as that's all we need
            // Ideally there should be a calculation parser to determine what to do.
            string calculatedValue = fld.Calculation;
            string calculatedOriginalValue = fld.Calculation;
            foreach (FieldValue fvCalc in row.Values) {
                calculatedValue = calculatedValue.Replace("{#" + fvCalc.FieldName + "}", (fvCalc.NewValue + ""));
                calculatedOriginalValue = calculatedOriginalValue.Replace("{#" + fvCalc.FieldName + "}", (fvCalc.OriginalValue + ""));
            }

            if (calculatedValue.ToLower().StartsWith("sql:")) {

                if (row.Mode != UpdateMode.Subtract) {

                    // we need to go to the database and pull the actual value
                    using (DataManager dm = DataManager.Create(idx.Indexer.DataConnectionSpec)) {
                        string sql = calculatedValue.Substring(4);
                        try {
                            fv.NewValue = dm.ReadValue(sql);
                            fv.OriginalValue = null;
                        } catch (Exception ex) {
                            SearchHost.debug(ex.Message);
                            throw new InvalidOperationException(getDisplayMember("calculateFieldValues{updating}", "calculating value for updating index={0}, field={1}, OriginalValue={2}, NewValue={3}, Mode={4}, ID={5}, failed with error: {6}. SQL: {7}", idx.Name, fv.FieldName, fv.OriginalValue.ToString(), fv.NewValue.ToString(), row.Mode.ToString(), row.ID.ToString(), ex.Message, sql), ex);
                        }
                    }
                }
            } else {
                fv.NewValue = calculatedValue;
                fv.OriginalValue = calculatedOriginalValue;
            }


            return fv;
        }

        private void performIndexUpdate(ThreadQueuer queuer, QueuedCallback qc) {

            string indexName = qc.IndexName;
            List<UpdateRow> rows = qc.Arguments[0] as List<UpdateRow>;
            SearchOptions opts = null;
            if (qc.Arguments.Length > 1) {
                opts = qc.Arguments[1] as SearchOptions;
            }

            debug("Performing index update in search engine for " + rows.Count + " rows");

            //debug("sleeping 3 seconds..."); Thread.Sleep(3000);

            var idx = getIndex(indexName);
            if (idx != null && idx.Enabled) {
                foreach (UpdateRow row in rows) {

                    if (queuer.IsDone) {
                        // process is ending, be sure not to kill it off in the middle of an update (the still-pending ones are lost, but the tree / data file will still work properly)
                        return;
                    }

                    if (qc.IsCancelled) {
                        // an action took place after this processing began, but before it finished which
                        // nullifies the processing this is doing.  i.e. lots of pending rows to realtime update
                        // but user issued a full rebuild index.  this realtime update should then bail
                        // because its processing will be ignored at the end anyway.
                        return;
                    }

                    try {
                        BPlusListLong bpll = new BPlusListLong();

                        // update the index itself as needed

                        switch (row.Mode) {
                            case UpdateMode.Add:

                                for (int i = 0; i < idx.SearchableFields.Count; i++) {
                                    Field fld = idx.SearchableFields[i];
                                    if (fld.IsCalculated) {
                                        // this is a multi-part field (i.e. 1 field in the search index, but multiple in the db table)

                                        FieldValue fv = calculateFieldValues(fld, row, idx);
                                        fv.OriginalValue = null;

                                        processAddKeywords(idx, fv, i, row.ID, opts);
                                    } else {
                                        for (int j = 0; j < row.Values.Count; j++) {
                                            if (row.Values[j].FieldName.ToLower() == fld.Name.ToLower()) {
                                                // it exists in the database definition.
                                                // if it is a searchable field, we need to update it.
                                                processAddKeywords(idx, row.Values[j], i, row.ID, opts);
                                                break;
                                            }
                                        }
                                    }
                                }



                                break;
                            case UpdateMode.Replace:
                                for (int i = 0; i < idx.SearchableFields.Count; i++) {
                                    Field fld = idx.SearchableFields[i];
                                    if (fld.IsCalculated) {
                                        // this is a multi-part field (i.e. 1 field in the search index, but multiple in the db table)

                                        FieldValue fv = calculateFieldValues(fld, row, idx);

                                        //processReplaceKeywords(idx, fv, i, row.ID);
                                        processAddKeywords(idx, fv, i, row.ID, opts);
                                    } else {
                                        for (int j = 0; j < row.Values.Count; j++) {
                                            if (row.Values[j].FieldName.ToLower() == fld.Name.ToLower()) {
                                                // it exists in the database definition.
                                                // if it is a searchable field, we need to update it.
                                                //processReplaceKeywords(idx, row.Values[j], i, row.ID);
                                                processAddKeywords(idx, row.Values[j], i, row.ID, opts);
                                                break;
                                            }
                                        }
                                    }
                                }

                                break;
                            case UpdateMode.Subtract:
                                for (int i = 0; i < idx.SearchableFields.Count; i++) {
                                    Field fld = idx.SearchableFields[i];
                                    if (fld.IsCalculated) {
                                        // this is a multi-part field (i.e. 1 field in the search index, but multiple in the db table)

                                        FieldValue fv = calculateFieldValues(fld, row, idx);

                                        processSubtractKeywords(idx, fv, i, row.ID, opts);

                                    } else {
                                        for (int j = 0; j < row.Values.Count; j++) {
                                            if (row.Values[j].FieldName.ToLower() == fld.Name.ToLower()) {
                                                // it exists in the database definition.
                                                // if it is a searchable field, we need to update it.
                                                processSubtractKeywords(idx, row.Values[j], i, row.ID, opts);
                                                break;
                                            }
                                        }
                                    }
                                }

                                break;
                            default:
                                throw new NotImplementedException(getDisplayMember("performIndeUpdate", "SearchHost.UpdateIndex row.UpdateMode={0} is not yet implemented (row.ID={1})", row.Mode.ToString(), row.ID.ToString()));
                        }


                        // update the appropriate FK resolver as needed

                        for (int i = 0; i < idx.ForeignKeyFields.Count; i++) {

                            var fld = idx.ForeignKeyFields[i];
                            // get the index for this field
                            var fkIdx = this.getIndex(fld.ForeignKeyTable);
                            if (fkIdx != null) {

                                foreach (string resolverName in fkIdx.Resolvers.Keys) {

                                    //// HACK: WOW this should not be hardcoded here... how to determine it though???
                                    //if (resolverName.ToLower() == "accessions" && idx.Name.ToLower() == "accession"
                                    //    || resolverName.ToLower() == "inventory" && idx.Name.ToLower() == "inventory"
                                    //    || resolverName.ToLower() == "orders" && idx.Name.ToLower() == "order_request") {

                                    // only resolvers that have method = sql store data in a separate tree/datafile
                                    // others store resolver data directly in the index itself, so nothing to update
                                    // (as it was already updated in the index earlier)
                                    if (resolverName.ToLower() == idx.Name.ToLower()) {

                                        // we may need to do an update to the fk index
                                        for (int j = 0; j < row.Values.Count; j++) {
                                            var fv = row.Values[j];
                                            if (fv.FieldName.ToLower() == fld.ForeignKeyField.ToLower()) {

                                                // update fkID stored in appropriate resolver

                                                if (fv.OriginalValue == null) {
                                                    // must be adding
                                                    fkIdx.Resolvers[resolverName].AddResolvedID(Toolkit.ToInt32(fv.NewValue, 0), row.ID);
                                                } else if (fv.NewValue == null) {
                                                    // must be removing
                                                    fkIdx.Resolvers[resolverName].RemoveResolvedID(Toolkit.ToInt32(fv.OriginalValue, 0), row.ID);
                                                } else if (Toolkit.ToInt32(fv.OriginalValue, 0) != Toolkit.ToInt32(fv.NewValue, 0)) {
                                                    // must be removing old then adding new
                                                    fkIdx.Resolvers[resolverName].RemoveResolvedID(Toolkit.ToInt32(fv.OriginalValue, 0), row.ID);
                                                    fkIdx.Resolvers[resolverName].AddResolvedID(Toolkit.ToInt32(fv.NewValue, 0), row.ID);

                                                } else {
                                                    // originalvalue == newvalue, nothing to do
                                                }


                                                // TODO: update index foreign key points to!!!!




                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }



                    } catch (Exception ex) {
                        logInfo(new ProgressEventArgs(getDisplayMember("performIndexUpdate{updatefailed}", "Updating a row in index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                    }

                    queuer.DecrementPendingWorkItemCount();

                }
            }
            debug("DONE performing index update in search engine for " + rows.Count + " rows");

        }

        [Conditional("DEBUGTHREADING")]
        private static void debug(string msg) {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + Process.GetCurrentProcess().Id + " - se - " + msg);
        }

        /// <summary>
        /// Updates the given indexName for each given UpdateRow object
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="rows"></param>
        public void UpdateIndex(string indexName, List<UpdateRow> rows) {

            debug("Received request in search engine to update " + rows.Count + " rows on index " + indexName);

            // we want to return UpdateIndex immediately, and just queue the request to update the indexes
            // as the actual processing time to update an index may be great.
            initIndexer();

            var idx = _indexer.GetIndex(indexName);
            if (idx != null && idx.Enabled && idx.Loaded) {
                // only update the index if it actually exists and it is enabled and loaded
                processInRealtimeUpdaterThread(indexName, rows);
                debug("Returning from request in search engine to update " + rows.Count + " rows");
            } else {
                debug("Skipping request to update search engine index because index " + indexName + " does not exist or is not enabled or loaded");
            }

        }

        /// <summary>
        /// Verifies the given index(es) by: 1) ensuring database connectivity and the index SQL is a valid statement, 2) ensuring corresponding resolver(s) have valid SQL, and 3) corresponding B+ tree does not contain any malformed nodes (such as an empty keyword or value pointing at a non-existant location in the data file)
        /// </summary>
        /// <param name="indexNames"></param>
        /// <param name="checkLeaves"></param>
        /// <returns></returns>
		public List<string> VerifyIndexes(List<string> indexNames, bool checkLeaves) {
			List<string> badIndexes = new List<string>();
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (idx.Enabled) {
					try {
						idx.VerifyIntegrity(true, checkLeaves);
					} catch (Exception ex) {
						badIndexes.Add(idx.Name + ": " + ex.Message);
						logInfo(new ProgressEventArgs(getDisplayMember("VerifyIndexes{failed}", "Verifying index '{0}' failed: {1}.  {2}", idx.Name ,ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				}
			}
			return badIndexes;
		}


        private void processInIndexManipulatorThread(ThreadQueuerCallback callback, string indexName, object[] args, int numberOfWorkItemsRepresented) {

            // cancel all pending realtime updates on this index since we're going to rebuild/defrag it anyway
            SearchHost.debug("Cancelling existing realtime work items for index " + indexName + "...");
            _realtimeUpdateQueuer.CancelWorkByIndex(indexName);

            // then queue the index work as needed
            SearchHost.debug("Queueing manipulator work item for index " + indexName + " containing " + numberOfWorkItemsRepresented + " items...");
            _indexManipulationQueuer.AddWork(callback, indexName, args, numberOfWorkItemsRepresented);
        }

        private delegate void ThreadQueuerCallback(ThreadQueuer queuer, QueuedCallback fromCallback);

        private void processInRealtimeUpdaterThread(string indexName, List<UpdateRow> rows) {
            SearchHost.debug("Queueing realtime work item for index " + indexName + " containing " + rows.Count + " items...");
            _realtimeUpdateQueuer.AddWork(performIndexUpdate, indexName, new object[] { rows }, rows.Count);
        }

        private void performIndexDefragmentation(ThreadQueuer queuer, QueuedCallback qc) {
            List<string> indexNames = qc.Arguments[0] as List<string>;
            bool rotateInOnComplete = (bool)qc.Arguments[1];

            var indexes = getIndexes(indexNames);
            foreach (var idx in indexes) {

                if (queuer.IsDone) {
                    // process is ending, be sure not to kill it off in the middle of an update (the still-pending ones are lost, but the tree / data file will still work properly)
                    return;
                }

                if (qc.IsCancelled) {
                    return;
                }

                try {
                    idx.Defrag(processDefrag);

                    if (rotateInOnComplete && idx.Enabled) {
                        List<string> rotateOne = new List<string>(new string[] { idx.Name });
                        // also assume they want to enable and verify them.
                        idx.Rotate(false);
                        idx.VerifyIntegrity(true, true);
                    }
                } catch (Exception ex) {
                    logInfo(new ProgressEventArgs(getDisplayMember("performIndexDefragmentation{failed}", "Defragging index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                }
            }
        }

        private void performIndexCreation(ThreadQueuer queuer, QueuedCallback qc) {
            List<string> indexNames = qc.Arguments[0] as List<string>;
            bool onlyIfEnabled = (bool)qc.Arguments[1];
            bool rotateInOnComplete = (bool)qc.Arguments[2];

            var indexes = getIndexes(indexNames);
            foreach (var idx in indexes) {

                if (queuer.IsDone) {
                    // process is ending, be sure not to kill it off in the middle of an update (the still-pending ones are lost, but the tree / data file will still work properly)
                    return;
                }


                if (idx.Enabled || !onlyIfEnabled) {
                    try {
                        // create a new index object to pull in any configuration changes

                        var ds = _indexer.FillDataSetFromConfig(null, false, idx.Name, null);

                        var dbDown = false;
                        //Index<BPlusString, BPlusListLong>.FillDataSetFromConfig(ds, idx.Name, _indexer.ConfigFilePath, _indexer.FolderName, _indexer.DataConnectionSpec.ProviderName, _indexer.DataConnectionSpec.ConnectionString, idx.Name, null, ref dbDown);
                        var newConfigIndex = Index<BPlusString, BPlusListLong>.FillObjectFromDataSet(_indexer, ds, idx.Name, ref dbDown, _indexer_OnProgress);

                        // create the index data
                        newConfigIndex.Rebuild(true);

                        if (rotateInOnComplete) {

                            // swap out the now-old index object
                            _indexer.SwapIndexObjects(idx, newConfigIndex);

                            // unload the now-old index object
                            idx.Unload();

                            // rotate in the files for the new index object
                            newConfigIndex.Rotate(false);
                            newConfigIndex.VerifyIntegrity(true, true);
                            newConfigIndex.SaveConfig(null, null);
                        }
                    } catch (Exception ex) {
                        logInfo(new ProgressEventArgs(getDisplayMember("performIndexCreation{failed}", "Creating index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                    }
                }
            }

        }

        /// <summary>
        /// Rebuilds each given index, optionally only if it is already enabled. Can optionally rotate index in after being rebuilt.
        /// </summary>
        /// <param name="indexNames"></param>
        /// <param name="onlyIfEnabled"></param>
        /// <param name="rotateInOnComplete"></param>
		public void RebuildIndexes(List<string> indexNames, bool onlyIfEnabled, bool rotateInOnComplete) {
			// index creation takes a LONG time.
			// spin it off into its own thread and return.
			// (we can't rely on WCF asynchronous calls because
			//  they would all boil down to the single thread in the service -- so during
			//  long-running calls like CreateIndexes, nothing else could be called -- not even Search().)

            if (indexNames == null || indexNames.Count == 0) {
                indexNames = this.ListIndexes(onlyIfEnabled);
            }

			// add this to the queue of stuff to do on the background thread
            foreach (var idx in indexNames) {
                // create a separate queue item for each one just so the 'pending work item count' is updated properly
                var names = new List<string>();
                names.Add(idx);

                // then tell the index to update
                processInIndexManipulatorThread(performIndexCreation, idx, new object[]{names, onlyIfEnabled, rotateInOnComplete}, 1);
            }

		}


        /// <summary>
        /// Creates each given index, optionally only if it is already enabled. Can optionally rotate index in after creation.
        /// </summary>
        /// <param name="indexNames"></param>
        /// <param name="rotateInOnComplete"></param>
        public void DefragIndexes(List<string> indexNames, bool rotateInOnComplete) {
            // index creation takes a LONG time.
            // spin it off into its own thread and return.
            // (we can't rely on WCF asynchronous calls because
            //  they would all boil down to the single thread in the service -- so during
            //  long-running calls like CreateIndexes, nothing else could be called -- not even Search().)

            if (indexNames == null || indexNames.Count == 0) {
                indexNames = this.ListIndexes(true);
            }

            // add this to the queue of stuff to do on the background thread
            foreach (var idx in indexNames) {
                // create a separate queue item for each one just so the 'pending work item count' is updated properly
                var names = new List<string>();
                names.Add(idx);
                processInIndexManipulatorThread(performIndexDefragmentation, idx, new object[]{names, rotateInOnComplete}, 1);
            }

        }


        /// <summary>
        /// Enables the index and prepares it for querying by opening corresponding B+ tree files and resolver B+ tree files.
        /// </summary>
        /// <param name="indexNames"></param>
		public void EnableIndexes(List<string> indexNames) {
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (!idx.Enabled) {
					try {
						logInfo(getDisplayMember("EnableIndexes{start}", "Enabling index {0}...", idx.Name));
						idx.Enabled = true;
                        idx.SaveEnabledToConfig();
                        var dbDown = false;
						idx.Load(ref dbDown);
                        logInfo(getDisplayMember("EnableIndexes{done}", "Successfully enabled index {0}.", idx.Name));
					} catch (Exception ex) {
						logInfo(new ProgressEventArgs(getDisplayMember("EnableIndexes{failed}", "Enabling index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				} else {
					logInfo(getDisplayMember("EnableIndexes{skip}", "Skipping Enable index {0}, it is already enabled.", idx.Name));
				}
			}
		}

        /// <summary>
        /// Disables the index(es) so they cannot be searched.
        /// </summary>
        /// <param name="indexNames"></param>
		public void DisableIndexes(List<string> indexNames) {
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (idx.Enabled) {
					try {
                        logInfo(getDisplayMember("DisableIndexes{start}", "Disabling index {0}...", idx.Name));
                        idx.Enabled = false;
                        idx.SaveEnabledToConfig();
                        logInfo(getDisplayMember("DisableIndexes{start}", "Successfully disabled index {0}...", idx.Name));
						idx.Unload();
					} catch (Exception ex) {
                        logInfo(new ProgressEventArgs(getDisplayMember("DisableIndexes{failed}", "Disabling index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				} else {
					logInfo(getDisplayMember("DisableIndexes{skip}", "Skipping Disable index {0}, it is already disabled.", idx.Name));
				}
			}
		}

        /// <summary>
        /// Permanently deletes the index(es) and their definition(s).
        /// </summary>
        /// <param name="indexNames"></param>
        public void DeleteIndexes(List<string> indexNames) {
            initIndexer();
            
            var indexes = getIndexes(indexNames);
            foreach (var idx in indexes) {
                try {
                    logInfo(getDisplayMember("DeleteIndexes{start}", "Deleting index {0}...", idx.Name));
                    idx.Indexer.DeleteIndex(idx.Name);
                    logInfo(getDisplayMember("DeleteIndexes{done}", "Successfully deleted index {0}.", idx.Name));
                } catch (Exception ex) {
                    logInfo(new ProgressEventArgs(getDisplayMember("DeleteIndexes{failed}", "Deleting index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                }
            }
        }

        /// <summary>
        /// Disables the resolver so it cannot be used for search results.
        /// </summary>
        /// <param name="indexNames"></param>
        public void DisableResolver(string indexName, string resolverName){
            var idx = getIndex(indexName);
            if (idx != null) {
                Resolver<BPlusString, BPlusListLong> res;
                if (idx.Resolvers.TryGetValue(resolverName, out res)) {
                    if (res.Enabled){
                        try {
                            logInfo(getDisplayMember("DisableResolver{start}", "Disabling resolver {0} for index {1}...", res.Name, idx.Name));
                            res.Enabled = false;
                            res.SaveEnabledToConfig();
                            logInfo(getDisplayMember("DisableResolver{done}", "Successfully disabled resolver {0} for index {1}...", res.Name, idx.Name));
                            idx.Reload();
                        } catch (Exception ex) {
                            logInfo(new ProgressEventArgs(getDisplayMember("DisableResolver{failed}", "Disabling resolver {0} for index {1} failed: {2}.  {3}", res.Name, indexName, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                        }
                    } else {
                        logInfo(getDisplayMember("DisableResolver{skip}", "Skipping Disable resolver {0} for index {1}, it is already disabled.", res.Name, idx.Name));
                    }
                }
            }
        }


        /// <summary>
        /// Enables the resolver so it can be used for search results.
        /// </summary>
        /// <param name="indexNames"></param>
        public void EnableResolver(string indexName, string resolverName) {
            var idx = getIndex(indexName);
            if (idx != null) {
                Resolver<BPlusString, BPlusListLong> res;
                if (idx.Resolvers.TryGetValue(resolverName, out res)) {
                    if (!res.Enabled) {
                        try {
                            logInfo(getDisplayMember("EnableResolver{start}", "Enabling resolver {0} for index {1}...", res.Name, idx.Name));
                            res.Enabled = true;
                            res.SaveConfig(null, null);
                            logInfo(getDisplayMember("EnableResolver{done}", "Successfully enabled resolver {0} for index {1}...", res.Name, idx.Name));
                            idx.Reload();
                        } catch (Exception ex) {
                            logInfo(new ProgressEventArgs(getDisplayMember("EnableResolver{failed}", "Enabling resolver {0} for index {1} failed: {2}.  {3}", res.Name, indexName, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
                        }
                    } else {
                        logInfo(getDisplayMember("EnableResolver{skip}", "Skipping Enable resolver {0} for index {1}, it is already enabled.", res.Name, idx.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Reloads all given indexes that are already enabled.  Does not enable or load indexes that are not enabled.
        /// </summary>
        /// <param name="indexNames"></param>
		public void ReloadIndexes(List<string> indexNames) {
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (idx.Enabled) {
					try {
						idx.Reload();
					} catch (Exception ex) {
                        logInfo(new ProgressEventArgs(getDisplayMember("ReloadIndexes{failed}", "Reloading index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				}
			}
		}

        /// <summary>
        /// Unloads the given indexes.  Skips indexes that are disabled.  Unloading means closing file handles for the index B+ tree and corresponding resolver(s) B+ tree(s).
        /// </summary>
        /// <param name="indexNames"></param>
		public void UnloadIndexes(List<string> indexNames) {
			var indexes = getIndexes(indexNames);
			foreach (var idx in indexes) {
				if (idx.Enabled) {
					try {
						idx.Unload();
					} catch (Exception ex) {
                        logInfo(new ProgressEventArgs(getDisplayMember("UnloadIndexes", "Unloading index '{0}' failed: {1}.  {2}", idx.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true));
					}
				}
			}
		}

        /// <summary>
        /// Returns a list of IndexReport objects on the current status of all indexes.
        /// </summary>
        /// <returns></returns>
		public List<IndexReport> ReportOnIndexes() {

			List<IndexReport> reports = new List<IndexReport>();

			var indexes = getIndexes(null);
			foreach (var idx in indexes) {
				IndexReport ir = new IndexReport { Name = idx.Name, IsEnabled = idx.Enabled, IsLoaded = idx.Loaded, Sql = idx.Sql };

				if (idx.Resolvers != null && idx.Resolvers.Count > 0) {
					foreach (Resolver<BPlusString, BPlusListLong> r in idx.Resolvers.Values) {

						ResolverReport rr = new ResolverReport {
							Name = r.Name,
							ForeignKeyField = r.ForeignKeyField,
//							IsEnabled = r.CacheEnabled,
							Method = r.Method.ToString(),
							PrimaryKeyField = r.PrimaryKeyField,
							Sql = r.Sql,
							ResolvedPrimaryKeyField = r.ResolvedPrimaryKeyField
						};

						ir.Resolvers.Add(rr);

					}
				}

				reports.Add(ir);

			}

			return reports;
		}

        public void SaveIndexerSettings(string dataSetAsXml) {
            initIndexer();
            var dsInput = Toolkit.DataSetFromXml(dataSetAsXml);
            var dt = dsInput.Tables["indexer"];
            if (dt != null) {
                foreach (DataRow dr in dt.Rows) {
                    _indexer.SaveConfig(dr);
                    logInfo(getDisplayMember("SaveIndexerSettings", "Saved configuration settings for indexer"));
                }
            }
        }

        public void SaveIndexSettings(string dataSetAsXml) {
            initIndexer();
            var dsInput = Toolkit.DataSetFromXml(dataSetAsXml);
            var dt = dsInput.Tables["index"];
            if (dt != null) {
                foreach (DataRow dr in dt.Rows) {
                    var idx = getIndex(dr["index_name"].ToString());
                    if (idx != null) {
                        idx.SaveConfig(dsInput, dr);
                        logInfo(getDisplayMember("SaveIndexSettings", "Saved configuration settings for index {0}", idx.Name));
                    }
                }
            }
        }

        public void SaveResolverSettings(string dataSetAsXml) {
            initIndexer();
            var dsInput = Toolkit.DataSetFromXml(dataSetAsXml);
            var dt = dsInput.Tables["resolver"];
            if (dt != null) {
                foreach (DataRow dr in dt.Rows) {
                    var idx = getIndex(dr["index_name"].ToString());
                    if (idx != null) {
                        Resolver<BPlusString, BPlusListLong> res = null;
                        if (idx.Resolvers.TryGetValue(dr["resolver_name"].ToString().ToLower(), out res)) {
                            res.SaveConfig(dsInput, dr);
                            logInfo(getDisplayMember("SaveResolverSettings", "Saved configuration settings for resolver {0} in index {1}", res.Name, idx.Name));
                        }
                    }
                }
            }
        }

		#endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "SearchHost", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
