using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using GrinGlobal.Core;

namespace GrinGlobal.Web
{
    public partial class MapsControl : System.Web.UI.UserControl
    {
        public MapsControl(): base()
        {
            this.Init += new EventHandler(MapsControl_Init);
        }

        private DataTable _dataSource;

        void MapsControl_Init(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                //what to take care
            }
            else
            {
                //what needs to be initialized here?
            }
        }

         public DataTable DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        private string dataSourceAsString
        {
            get
            {
                if (DataSource != null)
                {
                    return DataSource.ToJson(false);
                }
                else
                {
                    throw new NotSupportedException("DataSource does not contain valid data");
                }
            }
        }

        public override void DataBind()
        {
            hfMaps.Value = dataSourceAsString;
        }
      
    }
}