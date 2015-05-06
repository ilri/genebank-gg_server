using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace GrinGlobal.Search.Engine.Hosting {
    public class DynamicFormatterTypeAttribute : Attribute, IOperationBehavior  {

//        private IDispatchMessageFormatter _origFormatter;

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation) {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation) {
//            _origFormatter = dispatchOperation.Formatter;
//            dispatchOperation.Formatter = this;
        }

        public void Validate(OperationDescription operationDescription) {
        }


        //#region IDispatchMessageFormatter Members

        //public void DeserializeRequest(Message message, object[] parameters) {
        //    var httpRequest = message.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
        //    if (DynamicFormatter.UseJsonFormat(message)) {
        //        httpRequest.Headers["Content-Type"] = "application/json";
        //    } else if (DynamicFormatter.UseXmlFormat(message)){
        //        httpRequest.Headers["Content-Type"] = "text/xml; charset=utf-8";
        //    }
        //}

        //public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result) {
        //    return _origFormatter.SerializeReply(messageVersion, parameters, result);
        //}

        //#endregion
    }
}
