using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Text;

namespace GrinGlobal.Web
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSendFeedback.Text = Site1.DisplayText("btnSendFeedback", "Send Request"); 
        }
        protected void btnSendFeedback_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            string CRLF = "\r\n";

            sb.Append("At " + DateTime.Now + " request was sent from the Contact Us Page" + CRLF + CRLF);
            sb.Append("Name:  " + this.txtName.Text + CRLF + CRLF);
            sb.Append("Email Address: " + this.txtEmail.Text + CRLF + CRLF);
            sb.Append("Subject: " + this.ddlSubject.SelectedItem.Text.Trim() + CRLF + CRLF);
            sb.Append("Message:  " + this.txtMessage.Text.Trim());

            try
            {
                Email.Send(Toolkit.GetSetting("EmailHelpTo", ""),
                            Toolkit.GetSetting("EmailFrom", ""),
                            this.txtEmail.Text.Trim(),
                            "",
                            "GRIN-GLOBAL - " + ddlSubject.SelectedItem.Text.Trim(),
                            sb.ToString());
            }
            catch (Exception ex)
            {
                // debug, nothing we can/need to do if mail failed to send.
                string s = ex.Message;
                Logger.LogTextForcefully("Application error: Sending email failed for contact us from " + this.txtEmail.Text + ". ", ex);
            }
            finally
            {
            }

            this.pnlSendEmail.Visible = false;
            this.pnlMailSent.Visible = true;
        }
    }
}
