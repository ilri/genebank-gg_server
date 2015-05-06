using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public interface IAsyncDataTrigger {
        bool IsAsynchronous { get; }

        object Clone();
    }
}
