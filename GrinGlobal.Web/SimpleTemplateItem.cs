using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace GrinGlobal.Web {
    public class SimpleTemplateItem :
        Control, INamingContainer, IDataItemContainer {
        private object _currentDataItem;
        public SimpleTemplateItem(object currentItem) {
            _currentDataItem = currentItem;
        }

        #region IDataItemContainer Members

        public object DataItem {
            get { return _currentDataItem; }
        }

        public int DataItemIndex {
            get {
                throw new Exception
                    ("The method or operation is not implemented.");
            }
        }

        public int DisplayIndex {
            get {
                throw new Exception
                    ("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
