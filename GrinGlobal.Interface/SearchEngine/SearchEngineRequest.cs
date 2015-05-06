using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.Interface {
    public class SearchEngineRequest : IDisposable {

        public string MethodName { get; set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public Exception Exception;

        protected System.ServiceModel.Channels.Binding Binding;
        protected System.ServiceModel.EndpointAddress Address;

        public SearchEngineRequest(){
            Parameters = new Dictionary<string,string>();
        }

        protected string GetString(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                return val;
            }
            return null;
        }

        protected DateTime GetDateTime(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                return Util.ToDateTime(val, DateTime.MinValue);
            }
            return DateTime.MinValue;
        }

        protected decimal GetDecimal(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                return Util.ToDecimal(val, 0.0M);
            }
            return 0.0M;
        }

        protected int GetInt32(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                return Util.ToInt32(val, 0);
            }
            return 0;
        }

        protected bool GetBool(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                return Util.ToBoolean(val, false);
            }
            return false;
        }

        protected List<string> GetListString(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                if (val == null) {
                    return new List<string>();
                } else {
                    return Util.ParseDelimitedLine(val, ';');
                }
            }
            return null;
        }


        protected UpdateRow GetUpdateRow(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                var row = UpdateRow.FromXml(val);
                return row;
            }
            return null;
        }

        protected List<UpdateRow> GetListUpdateRow(string parameterName) {
            string val;
            if (Parameters.TryGetValue(parameterName, out val)) {
                var rows = UpdateRow.RowsFromXml(val);
                return rows;
            }
            return null;
        }

        //public virtual SearchEngineRequest Execute(){
        //    throw new NotImplementedException();
        //}

        public virtual string ToXml() {
            throw new NotImplementedException();
        }

        #region IDisposable Members

        public void Dispose() {
            // TODO: cleanup as needed
        }

        #endregion
    }
}
