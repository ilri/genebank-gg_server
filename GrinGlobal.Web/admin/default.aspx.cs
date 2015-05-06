using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;

using System.Web.Configuration;
using System.Web.Security;
using System.Configuration;
 

namespace GrinGlobal.Web.admin {
    public partial class _default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void btnClearCache_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                sd.ClearCache(null, true);
            }
        }

        protected void btnEncryptString_Click(object sender, EventArgs e)
        {
            string appPath = Request.ApplicationPath;
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration(appPath);
            ConfigurationSection configurationSection = configuration.GetSection("connectionStrings");
            if (configurationSection != null && !configurationSection.SectionInformation.IsLocked && !configurationSection.SectionInformation.IsProtected && !configurationSection.IsReadOnly())
            {
                configurationSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                configurationSection.SectionInformation.ForceSave = true;
                configuration.Save(ConfigurationSaveMode.Full);
            }
            else
            {
                if (configurationSection != null)
                {
                    configurationSection.SectionInformation.UnprotectSection();
                    configurationSection.SectionInformation.ForceSave = true;
                }
                configuration.Save(ConfigurationSaveMode.Full);
            }
        }

        protected void btnEncryptText_Click(object sender, EventArgs e)
        {
            try {
                if (tb1.Text != "")
                {
                    string password = Toolkit.GetSetting("StringPassword", "");
                    if (password == "")
                        tb2.Text = Crypto.EncryptText(tb1.Text.Trim());
                    else
                        tb2.Text = Crypto.EncryptText(tb1.Text.Trim(), password);
                }
            }
            catch {}
        }

    }
}
