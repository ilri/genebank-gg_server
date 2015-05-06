using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Core;

namespace GrinGlobal.Business.DataTriggers {
	class TestDataTrigger : ITableSaveDataTrigger {


        #region ISaveDataTrigger Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            switch (args.SaveMode) {
                case SaveMode.Delete:
                    break;
                case SaveMode.Insert:
                    break;
                case SaveMode.Update:
                    break;
                default:
                    throw new NotImplementedException(getDisplayMember("TestDataTrigger{TableSaving}", "SaveMode.{0} not implemented in Test.TableSaving()", args.SaveMode.ToString()));
            }
        }
		public void TableRowSaving(ISaveDataTriggerArgs args) {

			switch (args.SaveMode) {
				case SaveMode.Delete:
					break;
				case SaveMode.Insert:
					break;
				case SaveMode.Update:
					break;
				default:
                    throw new NotImplementedException(getDisplayMember("TestDataTrigger{TableRowSaving}", "SaveMode.{0} not implemented in Test.TableRowSaving()", args.SaveMode.ToString()));
			}
		}

		public void TableRowSaved(ISaveDataTriggerArgs args) {
			switch (args.SaveMode) {
				case SaveMode.Delete:
					break;
				case SaveMode.Insert:
					break;
				case SaveMode.Update:
					break;
				default:
                    throw new NotImplementedException(getDisplayMember("TestDataTrigger{TableRowSaved}", "SaveMode.{0} not implemented in Test.TableRowSaved()", args.SaveMode.ToString()));
			}
		}

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
            switch (args.SaveMode) {
                case SaveMode.Delete:
                    break;
                case SaveMode.Insert:
                    break;
                case SaveMode.Update:
                    break;
                default:
                    throw new NotImplementedException(getDisplayMember("TestDataTrigger{TableRowSaveFailed}", "SaveMode.{0} not implemented in Test.TableRowSaveFailed()", args.SaveMode.ToString()));
            }
        }

        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            switch (args.SaveMode) {
                case SaveMode.Delete:
                    break;
                case SaveMode.Insert:
                    break;
                case SaveMode.Update:
                    break;
                default:
                    throw new NotImplementedException(getDisplayMember("TestDataTrigger{TableSaved}", "SaveMode.{0} not implemented in Test.TableSaved()", args.SaveMode.ToString()));
            }
        }

		#endregion

        #region IAsyncDataTrigger Members

        public bool IsAsynchronous {
            get {
                return false;
            }
        }

        public virtual object Clone() {
            return this;
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "This is a class for testing DataTrigger invocation.  Serves no purpose outside the context of troubleshooting or debugging.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Test Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return null; }
        }

        #endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "DataTriggers", resourceName, null, defaultValue, substitutes);
        }
    }
}
