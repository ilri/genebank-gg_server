using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public interface IDataTriggerDescription {
        string GetDescription(string ietfLanguageTag);
        string GetTitle(string ietfLanguageTag);
    }
}
