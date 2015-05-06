using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GrinGlobal.Sql.GuiData;
//using GrinGlobal.Sql;
using GrinGlobal.Business;
using GrinGlobal.Core;

namespace GrinGlobal.Web {
	public partial class StreamData : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// we'll need:
			// userName
			// password
			// dvName
			// rsParams

            Page.Error += new EventHandler(Page_Error);

			string token = Request["token"];
			string dataview = Request["dataview"];
			string delimitedParameterList = Request["params"];
            string cols = ("" + Request["prettycolumns"]).ToLower();
            bool prettyColumns = cols == "true" || cols == "1" || cols == "Y";
			string format = Request["format"];
			if (String.IsNullOrEmpty(format)) {
				format = "tab";
			}
			int limit = Toolkit.ToInt32(Request["limit"], 0);
			int offset = Toolkit.ToInt32(Request["offset"], 0);

			if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(dataview)) {
                throw new ArgumentException("<html><body><pre>Not all required parameters were provided.\r\n" +
                    "token=" + (String.IsNullOrEmpty(token) ? "(missing!)" : token) +
                    "\r\ndataview=" + (String.IsNullOrEmpty(dataview) ? "(missing!)" : dataview) +
                    "\r\nparams=" + (String.IsNullOrEmpty(delimitedParameterList) ? "(missing but optional)" : delimitedParameterList) +
                    "\r\nformat=" + format +
                    "\r\nprettycolumns=" + (prettyColumns ? "true" : "false") + 
                    "\r\nlimit=" + limit +
					"\r\noffset=" + offset + "\r\n\r\nRestrictions:\r\n================\r\n- If offset is given, limit must also be given\r\n- params, if required by the specified dataview, must be provided in the following format:\r\n    :prm1=val1;:prm2=val2;\r\n- Valid values for 'format' (defaults to json if not provided):\r\n    csv\r\n    tab\r\n    json\r\n\r\n\r\n</pre>" +
                    @"You can use the following form as an example:
<form method='POST'>
  Token: <input type='text' value='' name='token' maxlength='200' size='50' />  <a href='StreamLogin.aspx' target='streamlogin'>Create a token</a><br />
  Dataview: <input type='text' value='' name='dataview' maxlength='50' size='50' /><br />
  Params: <input type='text' value='' name='params' maxlength='500' size='100' /><br />
  Format: <select name='format'><option value='csv'>csv</option><option value='json'>json</option><option value='tab' selected='selected'>tab</option></select><br />
  Pretty Columns: <input type='checkbox' value='true' name='prettycolumns' /><br />
  Limit: <input type='text' value='0' name='limit' maxlength='10' size='10' /><br />
  Offset: <input type='text' value='0' name='offset' maxlength='10' size='10' /><br />
<input type='submit' value='Go' />
</form>
</body>
</html>"
                );
			} else {
                if (dataview.StartsWith(":")) {
                    throw new InvalidOperationException("Colon queries are not allowed via StreamData.aspx");
                }
				using (SecureData sd = new SecureData(false, token)){
                    if (sd.LastLoginDate < DateTime.UtcNow.AddMinutes(-20)) {
                        throw new InvalidOperationException("Token has expired and is no longer valid.  Please generate a new token via StreamLogin.aspx and reissue your request.");
                    }
					Response.Buffer = false;
					Response.Clear();
                    Response.ContentType = "text/plain";
					sd.StreamData(dataview, delimitedParameterList, prettyColumns, limit, offset, format, Response.OutputStream, null);
                    Response.End();
				}
			}


		}

        void Page_Error(object sender, EventArgs e) {
            var ex = Server.GetLastError();
            Response.Clear();
            //Response.StatusCode = (ex is ArgumentException ? 501 : ex is InvalidOperationException ? 502 : 503);
            //Response.StatusDescription = Server.GetLastError().Message;
            Response.Write("***Error***\r\n" + Server.GetLastError().Message);
            Response.End();
            
        }

	}
}
