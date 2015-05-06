using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.Interface.Forms {
    public interface IParentForm {

        void CancelEdit(string reason);

        void Save();

        void MoveToRow(int rowNumber, bool isRelative);

        void Delete();

        DataSet GetData(string dataviewName, string parameters);

        DataSet SaveData(DataSet ds);


    }
}
