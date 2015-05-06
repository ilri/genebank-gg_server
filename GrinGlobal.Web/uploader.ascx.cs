using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.IO;

namespace GrinGlobal.Web
{
    public partial class Uploader : System.Web.UI.UserControl
    {
        // could be used for uploding files to tables other than order_request file table, then the record_type would tell which to go and get.
        static string _record_type = "";
        static int _userid = 0;
        static int _recordid = 0;
        string _caption = "";
        int _docid = 0;
        string _status = "";
        static bool _enabled = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvAttachments.Caption = _caption;
                refreshAtttachmentsGrid();
            }
            lblError.Visible = false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string fn = System.IO.Path.GetFileName(uploadAttachments.FileName);

            if (fn.Trim() != "")
            {
                int iExt = fn.LastIndexOf(".");
                if (iExt > 0)
                {
                    string sExt = fn.Substring(iExt + 1, fn.Length - iExt - 1).ToUpper();
                    string unsafeFile = Toolkit.GetSetting("UnSafeFileExtension", "exe;zip;asp;aspx;js;htm;html;shtml");
                    var unsafeFiles = unsafeFile.Split(';');
                    for (var i = 0; i < unsafeFiles.Length; i++)
                    {
                        if (sExt == unsafeFiles[i].Trim().ToUpper())
                        {
                            lblError.Text = "File cannot be uploaded. Please check your file and try again.";
                            lblError.Visible = true;
                            return;
                        }
                    }
                }
                else
                {
                    lblError.Text = "File cannot be uploaded. Please check your file and try again.";
                    lblError.Visible = true;
                    return;
                }

                lblError.Visible = false;

                fn = getFileSystemName(fn);

                if (fn != "")
                {
                    string fileName = Server.MapPath("~/uploads/imports/" + fn);
                    uploadAttachments.SaveAs(fileName);

                    //saveAttachment(fn);   

                    string action = "";
                    saveAttachment("~/uploads/imports/" + fn, ref action);
                    refreshAtttachmentsGrid();

                    // if it is the attachement for web order, and the web order status is ACCEPTED, need send email to order owner 
                    if (RecordType == "WebOrder") sendMail(fn, action);
                }
            }
        }

        private string getFileSystemName(string fileName)
        {
            //return string.Format("{0}_{1}_{2}", _userid, _recordid, fileName);
            return string.Format("{0}_{1}",  _recordid, fileName);
        }

        private void refreshAtttachmentsGrid()
        {
            DataTable attachments = getAttachments();
            gvAttachments.DataSource = attachments;
            gvAttachments.DataBind();

            uploadAttachments.Enabled = _enabled;
            btnUpload.Enabled = _enabled;
            gvAttachments.Columns[2].Visible = _enabled;

            //foreach (GridViewRow row in gvAttachments.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        LinkButton lb = row.FindControl("btnDelete") as LinkButton;
            //        lb.Visible  = _enabled;
            //    }
            //}
       }


        protected void gvAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string fileName = gvAttachments.DataKeys[e.RowIndex][0].ToString();

            //string fullPhysicalFilePath = Page.MapPath("~/uploads/imports/" + fileName);  
            string fullPhysicalFilePath = Page.MapPath(fileName);

            if (File.Exists(fullPhysicalFilePath))
            {
                File.Delete(fullPhysicalFilePath);
            }
            deleteAttachments(_docid);
            refreshAtttachmentsGrid();
            if (RecordType == "WebOrder") sendMail(fileName, "Deleted");
        }

        protected void gvAttachments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete") _docid = Convert.ToInt32(e.CommandArgument);
        }

        public string getAttachmentName(string url)
        {
            if (url != string.Empty)
            {
                //string[] path = url.Split('/');  // if saved other way
                //return path[1];
                //string fileNameHeader = url.Split('_')[0] + "_" + url.Split('_')[1] + "_";
                //url = url.Substring(fileNameHeader.Length, url.Length - fileNameHeader.Length);

                //take out userid in the file name now
                string fileNameHeader = url.Split('_')[0] + "_";
                url = url.Substring(fileNameHeader.Length, url.Length - fileNameHeader.Length);
                return url;
            }
            else
                return string.Empty;
        }

        public string getAttachmentsUrl(string url)
        {
            if (url is string && !String.IsNullOrEmpty(url as string)) {
                string path = url as string;
                //path = "~/uploads/imports/" + path;  
                return Page.ResolveClientUrl(path.Replace(@"\", "/").Replace("//", "/"));
            } else {
                return "";
            }
        }

        public string Caption
        {
            set
            {
                _caption = value;
            }
        }

        public int UserID
        {
            set
            {
                _userid = value;
            }
        }

        public int RecordID
        {
            get
            {
                return _recordid;
            }
            set
            {
                _recordid = value;
            }
        }

        private void saveAttachment(string filename, ref string action)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    int ct = Toolkit.ToInt32(dm.ReadValue(@"
                                select 
                                    count(web_order_request_attach_id)
                                from 
                                    web_order_request_attach
                                where 
                                    web_order_request_id = :orderrequestid
                                    and virtual_path = :vpath
                                    and created_by = :userid
                                ", new DataParameters(
                                ":orderrequestid", _recordid, DbType.Int32,
                                ":vpath", filename, DbType.String,
                                ":userid", _userid, DbType.Int32)), 0);

                    if (ct == 0)
                    {
                        dm.Write(@"
                        insert into web_order_request_attach
                        (web_cooperator_id, web_order_request_id, virtual_path, created_date, created_by, owned_date, owned_by)
                        values
                        (:wcopid, :orderrequestid, :vpath, :created_date, :created_by, :owned_date, :owned_by)
                        ", new DataParameters(
                        ":wcopid", sd.WebCooperatorID,
                        ":orderrequestid", _recordid,
                        ":vpath", filename,
                       ":created_date", DateTime.UtcNow, DbType.DateTime2,
                        ":created_by", _userid, DbType.Int32,
                        ":owned_date", DateTime.UtcNow, DbType.DateTime2,
                        ":owned_by", _userid, DbType.Int32));

                        action = "Added";
                    }
                    else
                    {
                        dm.Write(@"
                        update
                            web_order_request_attach
                        set
                            modified_date = :modified_date,
                            modified_by = :modified_by
                        where
                            web_order_request_id = :orderrequestid
                            and virtual_path = :vpath
                            and created_by = :userid
                        ", new DataParameters(
                        ":orderrequestid", _recordid, DbType.Int32,
                        ":vpath", filename, DbType.String,
                        ":userid", _userid, DbType.Int32,
                        ":modified_date", DateTime.UtcNow, DbType.DateTime2,
                        ":modified_by", _userid, DbType.Int32
                        ));

                        action = "Updated";
                    }
                }
            }
        }

        private void deleteAttachments(int docid)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Write(@"
                    delete from web_order_request_attach
                    where web_order_request_attach_id = :id                        
                    ", new DataParameters(":id", docid));
                }
            }
        }

        private DataTable getAttachments()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
//                using (DataManager dm = sd.BeginProcessing(true))
//                {
//                    DataTable dt = dm.Read(@"
//                    select web_order_request_attach_id as id, isnull(modified_date, created_date) as uploaddate, virtual_path as filePath
//                    from web_order_request_attach
//                    where web_order_request_id = :orderid                        
//                    ", new DataParameters(":orderid", RecordID));

//                    return dt;
//                }

                return sd.GetData("web_order_attachments", ":orderid=" + RecordID, 0, 0).Tables["web_order_attachments"];
            }
        }

        private void sendMail(string fileName, string action)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    string status = dm.ReadValue(@"select status_code from web_order_request where web_order_request_id = :wor", new DataParameters(":wor", _recordid, DbType.Int32)).ToString();
                    string emails = "";

                    if (status == "ACCEPTED")
                    {
                        DataTable dt = sd.GetData("web_order_request_owner", ":wor=" + _recordid, 0, 0).Tables["web_order_request_owner"];
                        
                        foreach (DataRow dr in dt.Rows)
                        {
                            emails += dr["email"].ToString() + ";"; 
                        }
                    }

                    if (emails.Length > 1)
                    {
                        int pos = fileName.LastIndexOf("/");
                        fileName = fileName.Substring(pos + 1, fileName.Length - pos - 1);

                        string eSubject = Page.DisplayText("ExtraOrderAttachEmailSubject", "GRIN-GLOBAL - Public Order Attachment");
                        eSubject += " (Web Order Number " + _recordid + ") (action: " + action + " file " + fileName + ")";

                        string adminEmailTo = Toolkit.GetSetting("EmailOrderTo", "");
                        if (!String.IsNullOrEmpty(adminEmailTo)) emails = adminEmailTo;

                        string eBody = "File " + fileName + " is " + Char.ToLowerInvariant(action[0]) + action.Substring(1) + ".";
                        try
                        {
                            Email.Send(emails,
                                        Toolkit.GetSetting("EmailFrom", ""),
                                        "",
                                        "",
                                        eSubject,
                                        eBody);
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message; // debug
                            Logger.LogTextForcefully("Application error: Sending email failed for notifying web order attachment " + _recordid + ". ", ex);
                        }
                    }
                }
            }
        }

        public string RecordType
        {
            get
            {
                return _record_type;
            }
            set
            {
                _record_type = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }

        }

    }
}