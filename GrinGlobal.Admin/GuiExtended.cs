using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GrinGlobal.Admin {

    delegate void WebRequestCallback(HttpWebRequest req);

    class GuiExtended : GUISvc.GUI {

        public WebRequestCallback WebRequestCreatedCallback;

        protected override WebRequest GetWebRequest(Uri uri) {


            var hwr = base.GetWebRequest(uri) as HttpWebRequest;

            if (WebRequestCreatedCallback != null) {
                WebRequestCreatedCallback(hwr);
            }

            return hwr;
        }
    }
}
