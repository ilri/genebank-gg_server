using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GrinGlobal.Interface.DataTriggers;
// using GrinGlobal.Business.SearchSvc;
using GrinGlobal.Search.Engine.Service.SearchSvc;
using System.Threading;
using GrinGlobal.Core;

namespace GrinGlobal.Business.DataTriggers {
	internal class SearchDataTrigger : ITableSaveDataTrigger {

		#region ISaveTrigger Members

        private List<GrinGlobal.Interface.UpdateRow> _pendingRows;
        private int _chunkSize = 100;
        public void TableSaving(ISaveDataTriggerArgs args) {
            _pendingRows = new List<GrinGlobal.Interface.UpdateRow>();
            _chunkSize = Toolkit.GetSetting("SearchEngineRealtimeUpdateChunkSize", 100);
        }
		public void TableRowSaving(ISaveDataTriggerArgs args) {
			// nothing to do
		}

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
        }


		public void TableRowSaved(ISaveDataTriggerArgs args) {

            // instead of issuing a call to the search engine for every single row,
            // we queue them up and issue one large update call at the end.
            // this will keep the chatter down to a minimum and should minimize the response time
            // since search engine updates are asycnhronous

            if (args.SaveOptions.SkipSearchEngineUpdates) {
                // caller elects to skip search engine updates.  usually means an import is happening
                // and they will manually rebuild the indexes when they are done.
                return;
            }

            try {
                switch (args.SaveMode) {
                    case SaveMode.None:
                        break;
                    case SaveMode.Delete:
                        _pendingRows.Add(((SaveDataTriggerArgs)args).SecureData.QueueSearchUpdate(args.Table, args.SaveMode, args.RowToSave, args.OriginalPrimaryKeyID));
                        break;
                    case SaveMode.Insert:
                    case SaveMode.Update:
                        // update search index as needed -- 
                        _pendingRows.Add(((SaveDataTriggerArgs)args).SecureData.QueueSearchUpdate(args.Table, args.SaveMode, args.RowToSave, args.NewPrimaryKeyID));
                        break;
                }
            } catch (Exception ex) {
                // we should NEVER throw exceptions or call args.Cancel() from here since updating the search engine 
                // isn't considered critical to saving GRIN-Global data.
                // so here we should log all our exceptions...
                Debug.WriteLine(ex.Message);

            }

            if (_pendingRows.Count > _chunkSize) {
                // we queue up to _chunkSize rows, then make a call into the search engine to process them
                // (updateSearchEngine method takes care of clearing _pendingRows as needed)
                updateSearchEngine(args);
            }
		}

        [Conditional("DEBUGDATATRIGGERS")]
        private void debug(string msg) {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + Process.GetCurrentProcess().Id + " - trigger - " + msg);
        }

        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {

            if (args.SaveOptions.SkipSearchEngineUpdates) {
                // caller elects to skip search engine updates.  usually means an import is happening
                // and they will manually rebuild the indexes when they are done.
                return;
            }

            // if there were any that are still pending (i.e. count was smaller than the last grouping of rows)
            // we need to be sure to update them...
            updateSearchEngine(args);
        }

        private void updateSearchEngine(ISaveDataTriggerArgs args) {
            // update the search engine with the queued-up rows
            try {
                if (_pendingRows.Count > 0) {
                    debug("Sending request search engine to update " + _pendingRows.Count + " rows");
                    ((SaveDataTriggerArgs)args).SecureData.UpdateSearchIndex(args.Table, _pendingRows, null, null);
                    debug("Returned from search engine to update " + _pendingRows.Count + " rows");
                    _pendingRows.Clear();
                }
            } catch (Exception ex) {
                // eat all errors so that subsequent rows are still updated!
                Logger.LogText("Failed on realtime update to search engine: " + ex.Message, ex);
                debug("Error updating search engine: " + ex.Message);
            }

        }
        #endregion

        #region IAsyncDataTrigger Members

        public bool IsAsynchronous {
            get { return false; }
        }

        public virtual object Clone() {
            var sf = new SearchDataTrigger { _pendingRows = this._pendingRows == null ? null : this._pendingRows.ToList() };
            return sf;
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Tells the GRIN-Global Search Engine when data is updated via the CT so it can index those changes.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Search Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
