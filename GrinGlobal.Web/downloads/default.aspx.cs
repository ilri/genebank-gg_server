using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace GrinGlobal.Web.downloads {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            pnlUploadExecutable.Visible = Page.User.IsInRole("Admins");
            pnlUploadDataview.Visible = Page.User.IsInRole("Admins");
            pnlUploadFile.Visible = Page.User.IsInRole("Admins");
            if (!IsPostBack) {
                bindData();
            }
        }

        void bindData() {
            fillGrid("~/uploads/installers/", gvExecutables);

            try {
                fillGrid("~/uploads/dataviews/", gvDataviews);
            } catch {
                gvDataviews.Visible = false;
            }

            try
            {
                fillGrid("~/uploads/documents/", gvFiles);
            }
            catch
            {
                gvFiles.Visible = false;
            }

        }

        private void fillGrid(string virtualPath, GridView gv) {
            string folder = Page.MapPath(virtualPath);

            List<UploadedFileInfo> files = new List<UploadedFileInfo>();

            if (Directory.Exists(folder)) {
                foreach (string file in Directory.GetFiles(folder)) {
                    FileInfo fi = new FileInfo(file);

                    // HACK: only show the updater.exe link...
                    if (virtualPath.Contains("/installers/")) {
                        if (fi.Name.ToLower() == "gringlobal_updater_setup.exe") {

                            //if (fi.Name != "placeholder.txt" && fi.Extension.ToLower() != ".version") {
                            decimal size = (((decimal)fi.Length / 1024.0M) / 1024.0M);
                            files.Add(new UploadedFileInfo { DisplayName = "GRIN-Global Updater", FileName = fi.Name, CreationTime = fi.CreationTime, LastWriteTime = fi.LastWriteTime, IsValid = true, Size = size.ToString("###,###,##0.0#") + " MB" });
                            //files.Add(new UploadedFileInfo { DisplayName = fi.Name, FileName = fi.Name, CreationTime = fi.CreationTime, LastWriteTime = fi.LastWriteTime, IsValid = true, Size = size.ToString("###,###,##0.0#") + " MB" });
                            //}
                        }
                    } else {
                        if ((fi.Extension.ToLower() == ".dataview") && (virtualPath.Contains("/dataviews/")))
                        {
                            decimal size = (((decimal)fi.Length / 1024.0M) / 1024.0M);
                            files.Add(new UploadedFileInfo { DisplayName = fi.Name, FileName = fi.Name, CreationTime = fi.CreationTime, LastWriteTime = fi.LastWriteTime, IsValid = true, Size = size.ToString("###,###,##0.0#") + " MB" });
                        }
                        else if (virtualPath.Contains("/documents/"))
                        {
                            if (fi.Name != "placeholder.txt") {
                                decimal size = (((decimal)fi.Length / 1024.0M) / 1024.0M);
                                files.Add(new UploadedFileInfo { DisplayName = fi.Name, FileName = fi.Name, CreationTime = fi.CreationTime, LastWriteTime = fi.LastWriteTime, IsValid = true, Size = size.ToString("###,###,##0.0#") + " MB" });
                            }
                        }
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

        protected void btnUploadExecutable_Click(object sender, EventArgs e) {

            string fileName = Server.MapPath("~/uploads/installers/" + filUploadExecutable.FileName);

            filUploadExecutable.SaveAs(fileName);

            this.Master.FlashMessage("Executable file has been saved.");

        }

        protected void btnUploadDataview_Click(object sender, EventArgs e) {

            string fileName = Server.MapPath("~/uploads/dataviews/" + filUploadDataview.FileName);

            filUploadDataview.SaveAs(fileName);

            this.Master.FlashMessage("Dataview file has been saved.");

        }

        protected void gvDataviews_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            string fileName = gvDataviews.DataKeys[e.RowIndex][0].ToString();
            string fullPhysicalFilePath = Server.MapPath("~/uploads/dataviews/" + fileName);
            if (File.Exists(fullPhysicalFilePath)) {
                File.Delete(fullPhysicalFilePath);
                this.Master.FlashMessage("File '" + fileName + "' deleted.");
            } else {
                this.Master.FlashError("File '" + fileName + "' does not exist.");
            }
        }

        protected void gvExecutables_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            string fileName = gvExecutables.DataKeys[e.RowIndex][0].ToString();


            string fullPhysicalFilePath = Server.MapPath("~/uploads/installers/" + fileName);

            if (File.Exists(fullPhysicalFilePath)) {
                File.Delete(fullPhysicalFilePath);
                this.Master.FlashMessage("File '" + fileName + "' deleted.");
            } else {
                this.Master.FlashError("File '" + fileName + "' does not exist.");
            }

        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {

            string fileName = Server.MapPath("~/uploads/documents/" + filUploadFile.FileName);

            filUploadFile.SaveAs(fileName);

            this.Master.FlashMessage("file has been saved.");

        }

        protected void gvFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string fileName = gvFiles.DataKeys[e.RowIndex][0].ToString();
            string fullPhysicalFilePath = Server.MapPath("~/uploads/documents/" + fileName);
            if (File.Exists(fullPhysicalFilePath))
            {
                File.Delete(fullPhysicalFilePath);
                this.Master.FlashMessage("File '" + fileName + "' deleted.");
            }
            else
            {
                this.Master.FlashError("File '" + fileName + "' does not exist.");
            }
        }
    }
}
