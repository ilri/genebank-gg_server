using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;

namespace GrinGlobal.Search.Engine.Hosting {
    public class ServiceHostFactory2 : ServiceHostFactory {
        protected override ServiceHost CreateServiceHost(Type serviceType,
                                                         Uri[] baseAddresses) {
            WebServiceHost2 webServiceHost2 =
                new WebServiceHost2(serviceType, baseAddresses);
            return webServiceHost2;
        }
    }
}
