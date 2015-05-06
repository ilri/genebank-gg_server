using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace GrinGlobal.Interface.Forms {
    public interface IChildForm {

        void Open(IParentForm parentForm);

        void EditRow(DataRow dr);

        bool CancelEdit(string reason);

        bool SaveRow(DataRow dr);

        void Close(IParentForm parentForm);

    }
}
