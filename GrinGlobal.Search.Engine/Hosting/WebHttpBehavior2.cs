using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace GrinGlobal.Search.Engine.Hosting {
    class WebHttpBehavior2 : WebHttpBehavior {

        public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {
            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription op, ServiceEndpoint endpoint) {
            var invoker = op.Behaviors.Find<WebInvokeAttribute>();
            var get = op.Behaviors.Find<WebGetAttribute>();
            var requestType = op.Behaviors.Find<DynamicFormatterTypeAttribute>();

            if (requestType != null) {
                if (invoker != null) {
                    invoker.RequestFormat = WebMessageFormat.Json;
                    var json = base.GetRequestDispatchFormatter(op, endpoint);

                    invoker.RequestFormat = WebMessageFormat.Xml;
                    var xml = base.GetRequestDispatchFormatter(op, endpoint);

                    return new DynamicFormatter() {
                        Json = json,
                        Xml = xml
                    };
                } else if (get != null) {
                    get.RequestFormat = WebMessageFormat.Json;
                    var json = base.GetRequestDispatchFormatter(op, endpoint);

                    get.RequestFormat = WebMessageFormat.Xml;
                    var xml = base.GetRequestDispatchFormatter(op, endpoint);
                    return new DynamicFormatter() {
                        Json = json,
                        Xml = xml
                    };
                }

            }

            return base.GetRequestDispatchFormatter(op, endpoint);

        }

        protected override IDispatchMessageFormatter GetReplyDispatchFormatter(OperationDescription op,
                                                                               ServiceEndpoint endpoint) {
            var invoker = op.Behaviors.Find<WebInvokeAttribute>();
            var get = op.Behaviors.Find<WebGetAttribute>();
            var responseType = op.Behaviors.Find<DynamicFormatterTypeAttribute>();

            if (responseType != null) {
                if (invoker != null) {
                    invoker.ResponseFormat = WebMessageFormat.Json;
                    var json = base.GetReplyDispatchFormatter(op, endpoint);

                    invoker.ResponseFormat = WebMessageFormat.Xml;
                    var xml = base.GetReplyDispatchFormatter(op, endpoint);

                    return new DynamicFormatter() {
                        Json = json,
                        Xml = xml
                    };
                } else if (get != null) {
                    get.ResponseFormat = WebMessageFormat.Json;
                    var json = base.GetReplyDispatchFormatter(op, endpoint);

                    get.ResponseFormat = WebMessageFormat.Xml;
                    var xml = base.GetReplyDispatchFormatter(op, endpoint);

                    return new DynamicFormatter() {
                        Json = json,
                        Xml = xml
                    };
                }
            }

            return base.GetReplyDispatchFormatter(op, endpoint);

        }
    }
}
