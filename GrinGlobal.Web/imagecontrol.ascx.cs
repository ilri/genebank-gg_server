using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GrinGlobal.Web {
    public partial class ImageControl : System.Web.UI.UserControl {

        private DataTable _dataSource;

        public DataTable DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; } // scrubData(value); }
        }

        public override void DataBind()
        {
            this.dlTN.DataSource = DataSource;
            this.dlTN.DataBind();
        }

        public string Resolve(object url) {
            if (url is string && !String.IsNullOrEmpty(url as string)) {
                // convert \ to / and resolve ~/ to /gringlobal/ ....
                string path = url as string;
                if (path.ToUpper().IndexOf("HTTP://") > -1)
                {
                    return path;
                }
                else
                {
                    string rootPath = Core.Toolkit.GetSetting("WebServerURL", "");

                    if (rootPath == "")
                    {
                        path = "~/uploads/images/" + path;
                        //return Page.ResolveClientUrl((url as string).Replace(@"\", "/"));
                        return Page.ResolveClientUrl(path.Replace(@"\", "/").Replace("//", "/"));
                    }
                    else
                    {
                        path = (rootPath + "/uploads/images/" + path).Replace(@"\", "/").Replace("//", "/");
                        return path;
                    }
                }
            } else {
                return "";
            }
        }

        //private DataTable scrubData(DataTable dt)
        //{
        //    DataTable dt2 = new DataTable();
        //    string thumbPath = "";
        //    string virtualPath = "";
        //    string firstChar = "";
        //    string appRoot = Page.ResolveUrl("~/");

        //    dt2.Columns.Add("thumbnail_virtual_path", typeof(string));
        //    dt2.Columns.Add("title", typeof(string));
        //    dt2.Columns.Add("virtual_path", typeof(string));

        //    object[] rowVals = new object[3];
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        thumbPath = dr["thumbnail_virtual_path"].ToString();
        //        firstChar = thumbPath.Substring(0, 1);
        //        if (firstChar == "~")
        //        {
        //            thumbPath = thumbPath.Replace("~/", appRoot);
        //        }

        //        virtualPath = dr["virtual_path"].ToString();
        //        firstChar = virtualPath.Substring(0, 1);
        //        if (firstChar == "~")
        //        {
        //            virtualPath = virtualPath.Replace("~/", appRoot);
        //        }

        //        rowVals[0] = thumbPath;
        //        rowVals[1] = dr["title"].ToString();
        //        rowVals[2] = virtualPath;
        //        dt2.Rows.Add(rowVals);
        //    }
        //    return dt2;
        //}

        protected void Page_Load(object sender, EventArgs e) {
        }
    }
}