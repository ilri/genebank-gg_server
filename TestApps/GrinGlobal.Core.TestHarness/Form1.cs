using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Core;

namespace GrinGlobal.Core.TestHarness {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			fill();
		}

		private void fill() {
			string sql = "select * from author order by id asc";

            if (chkSQLServer.Checked){
			    using (DataManager dm = DataManager.Create("sqlserverdbhere")) {
				    dgvSQLServer.DataSource = dm.Read(sql);
			    }
            }

            if (chkMySQL.Checked){
			    using (DataManager dm = DataManager.Create("mysqldbhere")) {
				    dgvMySQL.DataSource = dm.Read(sql);
			    }
            }

//			using (DataManager dm = DataManager.Create("ThirdPartyOracleDllNameHere")) {
            if (chkOracle.Checked){
			    using (DataManager dm = DataManager.Create("oracledbhere")) {
				    dgvOracle.DataSource = dm.Read(sql);
			    }
            }

            if (chkSQLite.Checked){
                using (DataManager dm = DataManager.Create("sqlitedbhere")) {
                    dgvSQLite.DataSource = dm.Read(sql);
                }
            }

            if (chkPostgreSQL.Checked){
			    using (DataManager dm = DataManager.Create("pgsqldbhere")) {
			        dgvPostgreSQL.DataSource = dm.Read(sql);
			    }
            }

            if (chkSybase.Checked){
			    using (DataManager dm = DataManager.Create("sybasedbhere")) {
			        dgvSybase.DataSource = dm.Read(sql);
			    }
            }
		}

		private void btnAdd_Click(object sender, EventArgs e) {
            DataManager msDM = null;
			int msID = 0;

            DataManager myDM = null;
            int myID = 0;
            
            DataManager orDM = null;
            int orID = 0;
            
            DataManager pgDM = null;
            int pgID = 0;
            
            DataManager syDM = null;
            int syID = 0;
            
            DataManager slDM = null;
            int slID = 0;

			// database vendor agnostic -- use this way!!!
			string sql = @"insert into author (author_name, author_age, author_date) values (?name, ?age, ?dt)";

			// do NOT use these ways, as they are database vendor-specific

			// oracle only
			//string sql = @"insert into author (author_name, author_age, author_date) values (:name, :age, :dt) returning id into :returnid";

			// sql server only
            //string sql = @"insert into author (author_name, author_age, author_date) values (@name, @age, @dt); select scope_identity();";

			// mysql only
            //string sql = @"insert into author (author_name, author_age, author_date) values (?name, ?age, ?dt); select last_insert_id()";

            // sqlite only
            //string sql = @"insert into author (author_name, author_age, author_date) values (?name, ?age, ?dt); select last_insert_rowid()";

			// postgresql only
            //string sql = @"insert into author (author_name, author_age, author_date) values (:name, :age, :dt) returning id;";

			DataParameters dps = new DataParameters(
				new DataParameter("?name", txtName.Text),
				new DataParameter("?age", Toolkit.ToInt32(txtAge.Text, 0), DbType.Int32),
				new DataParameter("?dt", DateTime.UtcNow, DbType.DateTime2));

            try {

                if (chkSQLServer.Checked){
                    msDM = DataManager.Create("sqlserverdbhere");
                    msDM.BeginTran(true);
                    msID = msDM.Write(sql, true, "id", dps);
                    msID = msDM.Write(sql, true, "id", dps);
                }
                if (chkMySQL.Checked){
                    myDM = DataManager.Create("mysqldbhere", msDM);
                    myDM.BeginTran(true);
                    myID = myDM.Write(sql, true, "id", dps);
                    myID = myDM.Write(sql, true, "id", dps);
                }
                if (chkOracle.Checked){
                    orDM = DataManager.Create("oracledbhere", myDM ?? msDM);
                    orDM.BeginTran(true);
                    orID = orDM.Write(sql, true, "id", dps);
                    orID = orDM.Write(sql, true, "id", dps);
				    orID = orDM.Write(sql, true, "id", dps);
                }
                if (chkPostgreSQL.Checked){
    				pgDM = DataManager.Create("pgsqldbhere", orDM ?? myDM ?? msDM);
			        pgDM.BeginTran(true);
			        pgID = pgDM.Write(sql, true, "id", dps);
                }

                if (chkSybase.Checked) {
                    syDM = DataManager.Create("sybasedbhere", pgDM ?? orDM ?? myDM ?? msDM);
                    syDM.BeginTran(true);
                    syID = syDM.Write(sql, true, "id", dps);
                }
                if (chkSQLite.Checked) {
                    slDM = DataManager.Create("sqlitedbhere", syDM ?? pgDM ?? orDM ?? myDM ?? msDM);
                    slDM.BeginTran(true);
                    slID = slDM.Write(sql, true, "id", dps);
                    slID = slDM.Write(sql, true, "id", dps);
				}
            } finally {
                if (slDM != null){
                    slDM.Dispose();
                }
                if (msDM != null){
                    msDM.Dispose();
                }
                if (syDM != null){
                    syDM.Dispose();
                }
                if (pgDM != null){
                    pgDM.Dispose();
                }
                if (orDM != null){
                    orDM.Dispose();
                }
                if (myDM != null){
                    myDM.Dispose();
                }
            }

			fill();

			MessageBox.Show("ms sql id=" + msID + "\nmysql id=" + myID + "\noracle id=" + orID + "\npostgres id=" + pgID + "\nsybase id=" + syID + "\nsqlite id=" + slID);

		}

		private void button2_Click(object sender, EventArgs e) {
            MessageBox.Show("Not implemented");


			//using (DataManager dm = DataManager.Create("sybasedbhere")) {
			//    string sql = @"insert into author (author_name, author_age) values (?name, ?age)";

			//    DataParameters dps = new DataParameters(
			//        new DataParameter("?name", textBox1.Text),
			//        new DataParameter("?age", tb1.Value, DbType.Int32));

			//    int output = dm.Write(sql, true, "id", dps);


			//}
		}
	}
}
