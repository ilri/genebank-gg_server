using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Description;

namespace GrinGlobal.Search.Engine.Hosting {
    public class WebServiceHost2 : WebServiceHost {
        public WebServiceHost2(object singletonInstance, params Uri[] baseAddresses) 
            : base(singletonInstance, baseAddresses) { }
        public WebServiceHost2(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) { }

        protected override void OnOpening() {
            base.OnOpening();

            foreach (var ep in this.Description.Endpoints) {
                if (ep.Behaviors.Find<WebHttpBehavior>() != null) {
                    ep.Behaviors.Remove<WebHttpBehavior>();
                    ep.Behaviors.Add(new WebHttpBehavior2());
                }
            }
        }
    }
}
