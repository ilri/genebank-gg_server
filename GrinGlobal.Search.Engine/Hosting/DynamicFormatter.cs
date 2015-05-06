using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace GrinGlobal.Search.Engine.Hosting {
    class DynamicFormatter : IDispatchMessageFormatter {
        public IDispatchMessageFormatter Json { get; set; }
        public IDispatchMessageFormatter Xml { get; set; }




        public static bool UseJsonFormat(Message msg) {

            if (msg.Headers.To.AbsolutePath.ToLower().Contains("/json/")) {
                return true;
            }

            HttpRequestMessageProperty prop = (HttpRequestMessageProperty)msg.Properties[HttpRequestMessageProperty.Name];
            string accepts = prop.Headers[HttpRequestHeader.Accept];
            if (accepts != null) {
                if (accepts.Contains("application/json") || accepts.Contains("text/javascript")) {
                    return true;
                }
            } 

            string contentType = prop.Headers[HttpRequestHeader.ContentType];
            if (contentType != null) {
                if (contentType.Contains("application/json") || contentType.Contains("text/javascript")) {
                    return true;
                }
            }

            return false;


        }

        public static bool UseXmlFormat(Message msg) {

            if (msg.Headers.To.AbsolutePath.ToLower().Contains("/xml/")) {
                return true;
            }

            HttpRequestMessageProperty prop = (HttpRequestMessageProperty)msg.Properties[HttpRequestMessageProperty.Name];
            string accepts = prop.Headers[HttpRequestHeader.Accept];
            if (accepts != null) {
                if (accepts.Contains("text/xml")) {
                    return true;
                }
            }

            string contentType = prop.Headers[HttpRequestHeader.ContentType];
            if (contentType != null) {
                if (contentType.Contains("text/xml")) {
                    return true;
                }
            }

            return false;


        }

        public void DeserializeRequest(Message message, object[] parameters) {
            var httpRequest = message.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            if (UseJsonFormat(message)) {
                httpRequest.Headers["Content-Type"] = "application/json";
                Json.DeserializeRequest(message, parameters);
            } else {
                httpRequest.Headers["Content-Type"] = "text/xml; charset=utf-8";
                Xml.DeserializeRequest(message, parameters);
            }


        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result) {
            Message msg = OperationContext.Current.RequestContext.RequestMessage;

            if (UseJsonFormat(msg)) {
                return Json.SerializeReply(messageVersion, parameters, result);
            } else {
                return Xml.SerializeReply(messageVersion, parameters, result);
            }

        }

    }
}
