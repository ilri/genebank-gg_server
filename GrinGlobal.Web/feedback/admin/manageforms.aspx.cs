using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web.feedback.admin
{
    public partial class manageforms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                bindData();
            }
        }

        private void bindData() {
            using (var sd = UserManager.GetSecureData(false)) {

                gvForms.DataSource = sd.GetData("web_feedback_form", ":feedbackformid=;", 0, 0).Tables["web_feedback_form"];
                gvForms.DataBind();

            }
        }

        protected void gvForms_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var formID = Toolkit.ToInt32(gvForms.DataKeys[e.RowIndex][0], -1);

                var dt = sd.GetData("web_feedback_form", ":feedbackformid=" + formID, 0, 0).Tables["web_feedback_form"];

                if (dt.Rows.Count > 0) {

                    // first delete all fields associated with it
                    var dtFields = sd.GetData("web_feedback_form_field", ":feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_field"];
                    foreach (DataRow drField in dtFields.Rows) {
                        drField.Delete();
                    }
                    sd.SaveData(dtFields.DataSet, true, null);

                    // then all traits
                    var dtTraits = sd.GetData("web_feedback_form_trait", ":feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_trait"];
                    foreach (DataRow drTrait in dtTraits.Rows) {
                        drTrait.Delete();
                    }
                    sd.SaveData(dtTraits.DataSet, true, null);

                    // finally the form itself
                    dt.Rows[0].Delete();
                    sd.SaveData(dt.DataSet, true, null);
                }
                Master.ShowMessage("Form has been deleted.");
                bindData();

            }
        }

    }
}
