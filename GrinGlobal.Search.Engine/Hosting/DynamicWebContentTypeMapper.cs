using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace GrinGlobal.Search.Engine.Hosting {
    public class DynamicWebContentTypeMapper : WebContentTypeMapper {

        public static bool UseJsonFormat(IncomingWebRequestContext req) {

            string url = "";
            if (url.ToLower().Contains("/json/")) {
                return true;
            }

            if (!String.IsNullOrEmpty(req.Accept)) {
                if (req.Accept.Contains("application/json") || req.Accept.Contains("text/javascript")) {
                    return true;
                }
            }

            if (!String.IsNullOrEmpty(req.ContentType)) {
                if (req.ContentType.Contains("application/json") || req.ContentType.Contains("text/javascript")) {
                    return true;
                }
            }

            return false;
        }

        public static bool UseXmlFormat(IncomingWebRequestContext req) {

            string url = "";
            if (url.ToLower().Contains("/xml/")) {
                return true;
            }

            if (!String.IsNullOrEmpty(req.Accept)) {
                if (req.Accept.Contains("text/xml") || req.Accept.Contains("application/xml")) {
                    return true;
                }
            }

            if (!String.IsNullOrEmpty(req.ContentType)) {
                if (req.ContentType.Contains("text/xml") || req.ContentType.Contains("application/xml")) {
                    return true;
                }
            }

            return false;


        }




        public override WebContentFormat GetMessageFormatForContentType(string contentType) {

            //IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            //if (UseJsonFormat(req)) {
            //    return WebContentFormat.Json;
            //} else if (UseXmlFormat(req)){
            //    return WebContentFormat.Xml;
            //} else {
                return WebContentFormat.Default;
            //}
        }
    }
}
