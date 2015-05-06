using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GrinGlobal.Core;

namespace GrinGlobal.Web {

	public partial class Client : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {

            Response.Redirect("~/downloads/default.aspx");

            Response.End();
            pnlUploadDocument.Visible = pnlUploadExecutable.Visible = Page.User.IsInRole("Admins");
            if (!IsPostBack) {
                bindData();
            }
        }

        void bindData() {

            fillGrid("~/uploads/installers/", gvExecutables);

            fillGrid("~/uploads/documents/", gvDocuments);

        }

        private void fillGrid(string virtualPath, GridView gv) {
            string folder = Page.MapPath(virtualPath);

            List<UploadedFileInfo> files = new List<UploadedFileInfo>();

            if (Directory.Exists(folder)) {
                foreach (string file in Directory.GetFiles(folder)) {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Name != "placeholder.txt") {
                        decimal size = (((decimal)fi.Length / 1024.0M) / 1024.0M);
                        files.Add(new UploadedFileInfo { DisplayName = fi.Name, FileName = fi.Name, CreationTime = fi.CreationTime, LastWriteTime = fi.LastWriteTime, IsValid = true, Size = size.ToString("###,###,##0.0#") + " MB" });
                    }
                }
            }

            files = files.OrderByDescending(f => f.LastWriteTime).ThenByDescending(f => f.FileName).ToList();

            removeAdminColumnsAsNeeded(gv);

            gv.DataSource = files;
            gv.DataBind();

        }

        private void removeAdminColumnsAsNeeded(GridView gv) {
            if (User.IsInRole("Admins")) {
                return;
            }

            for (int i = 0; i < gv.Columns.Count; i++) {
                if (gv.Columns[i].HeaderText == "") {
                    gv.Columns.RemoveAt(i);
                    i--;
                }
            }

        }

        protected void btnUploadDocument_Click(object sender, EventArgs e) {

            string fileName = Server.MapPath("~/uploads/documents/" + filUploadDocument.FileName);

            filUploadDocument.SaveAs(fileName);

            this.Master.FlashMessage("Documentation file has been saved.");

        }

        protected void btnUploadExecutable_Click(object sender, EventArgs e) {

            string fileName = Server.MapPath("~/uploads/installers/" + filUploadExecutable.FileName);

            filUploadExecutable.SaveAs(fileName);

            this.Master.FlashMessage("Executable file has been saved.");

        }

        protected void gvDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            string fileName = gvDocuments.DataKeys[e.RowIndex][0].ToString();


            string fullPhysicalFilePath = Page.MapPath("~/uploads/documents/" + fileName);

            if (File.Exists(fullPhysicalFilePath)) {
                File.Delete(fullPhysicalFilePath);
                this.Master.FlashMessage("File '" + fileName + "' deleted.");
            } else {
                this.Master.FlashError("File '" + fileName + "' does not exist.");
            }

        }

        protected void gvExecutables_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            string fileName = gvExecutables.DataKeys[e.RowIndex][0].ToString();


            string fullPhysicalFilePath = Page.MapPath("~/uploads/installers/" + fileName);

            if (File.Exists(fullPhysicalFilePath)) {
                File.Delete(fullPhysicalFilePath);
                this.Master.FlashMessage("File '" + fileName + "' deleted.");
            } else {
                this.Master.FlashError("File '" + fileName + "' does not exist.");
            }

        }

	}
}
