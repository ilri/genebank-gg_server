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
	public partial class frmGrinTestHarness : Form {
		public frmGrinTestHarness() {
			InitializeComponent();
		}

		private void btnSearch_Click(object sender, EventArgs e) {

			// To Uncomment the following example code, simply add a single slash to the beginning of the next line:
			/*

			// The GRIN project will be using a DataManager object to
			// 'abstract away' the details of a particular database vendor.
			// For example, to pull data from an Oracle database, one would
			// typically do the following (assume oraCnnString is the connection string):
			OracleConnection cnn = new OracleConnection(oraCnnString);
			cnn.Open();
			OracleCommand cmd = new OracleCommand("select * from some_table", cnn);
			OracleDataAdapter adpt = new OracleDataAdapter(cmd);
			DataSet ds = new DataSet();
			adpt.Fill(ds, "Table0");
			cmd.Dispose();
			cnn.Dispose();
			// return the DataTable containing the data we are interested in (namely 'Table0' since that's what we bound it to)
			return ds.Tables["Table0"];

			// Now, if we wanted to use MySQL as a backend instead of Oracle, we would have to
			// change the source code to use MySQL-specific objects (these objects are not part of
			// the .NET framework itself, they are an open-source implementation of the appropriate interfaces
			// which you can download at http://dev.mysql.com/downloads/connector/net/5.1.html):
			// (assume mySqlCnnString is the connection string):
			MySqlConnection cnn = new MySqlConnection(mySqlCnnString);
			cnn.Open();
			MySqlCommand cmd = new MySqlCommand("select * from some_table", cnn);
			MySqlDataAdapter adpt = new MySqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			adpt.Fill(ds, "Table0");
			cmd.Dispose();
			cnn.Dispose();
			// return the DataTable containing the data we are interested in (namely 'Table0' since that's what we bound it to)
			return ds.Tables["Table0"];


			// Notice the pattern is exactly the same, just different objects are used (MySql* instead of Oracle*)
			// We could boil this down to more concise, reusable code (which is part of the DataManager code we wrote):
			OracleDataManager odm = new OracleDataManager(oraConnString);
			DataTable dt = odm.Read("select * from some_table", "Table0");

			// and the equivalent for MySQL:
			MySqlDataManager odm = new MySqlDataManager(mySqlConnString);
			DataTable dt = odm.Read("select * from some_table", "Table0");

			// However, notice we're still tied to a specific database vendor, and if we wanted to switch to a new
			// database vendor it would still require us to recompile and re-release the code.
			// By using a factory pattern, we can determine which implementation to use (OracleDataManager or MySqlDataManager)
			// at runtime, based exclusively on the settings in the configuration file (i.e. the connection string):
			DataManager dm = DataManager.Create(oraOrMySqlConnString);
			// if oraOrMySqlConnString is configured to point to an Oracle datasource, 
			// dm will be an instance of OracleDataManager.
			// if it is configured to point to a MySql datasource,
			// dm will be an instance of MySqlDataManager.
			DataTable dt = dm.Read("select * from some_table", "Table0");
			//*/



			// The big difference here: we're not creating a specific object using the 'new' keyword as we did
			// in the first two examples.  We're using a static method (DataManager.Create())to return us the proper implementation
			// of either OracleDataManager or MySqlDataManager.  That static method essentially does the 'new' for
			// us and returns it -- but is smart enough to pick the right one.  Either way, the point is -- our code
			// shouldn't and doesn't care which implementation is returned, OracleDataManager or MySqlDataManager.

			// One additional caveat: In C#, if you pass an object to the using() construct, it will automatically
			// call that object's Dispose() method.  This is how .NET cleans up resources, and should be done on
			// database connections.  Notice in the first code example at the top of this method, we're calling cnn.Dispose()
			// and cmd.Dispose().  The benefit of wrapping the object with a using() construct is that even if there
			// is an exception, Dispose() will be called.  So the moral is: ALWAYS use the using() statement around the DataManager
			// object!



			try {
				this.Cursor = Cursors.WaitCursor;

				// NOTE: any and all SQL we pass to any database vendor (Oracle, MySQL, SQL Server, etc)
				//       MUST be written in such a style that it works across all vendors.  If not,
				//       special care must be taken to ensure vendor-specific SQL is passed only to an
				//       instance of DataManager that is for that particular vendor.  So, for our example,
				//       we can't use the Oracle-specific join syntax of (+).  Instead, we use the ANSI-standard
				//       inner join

				string sql = @"
select 
	iv.* 
from 
	iv inner join igm 
	on iv.ivid = igm.ivid
where 
	igm.igname = @igname
";

				// notice the sql and the parameters are EXACTLY the same between different database vendors,
				// even though Oracle requires parameters to be prefixed with ':' and mysql requires parameters
				// to be prefixed with '?'. Sql Server uses '@', which is what this example uses.  The point is,
				// DataManager handles converting the parameter prefixes to the proper format as the database
				// vendor requires.  The only restriction is the prefix must exist, and it must be a valid
				// prefix character (':', '?', or '@').  Also, since parameters are implemented in this fashion,
				// positional or nameless parameters should NOT be used.  These typically consist of just the '?'
				// character in the statement.  For example, do NOT do this:
				//   select * from table1 where field1 = ? and field2 = ?
				// but do any of these instead:
				//   select * from table1 where field1 = ?field1 and field2 = ?field2
				//   select * from table1 where field1 = :field1 and field2 = :field2
				//   select * from table1 where field1 = @field1 and field2 = @field2
				//
				// ALSO, the parameter names ARE case sensitive.  So '@igName' != '@IGName'.  This is done
				// only for performance reasons.  If you do accidentally get the casing wrong, the database
				// request will fail, complaining of missing or incorrect parameters anyway.

				DataParameter param1 = new DataParameter("@igname", txtIgName.Text);
				DataParameters paramList = new DataParameters();
				paramList.Add(param1);



				// first pull data from oracle
				using (DataManager dm1 = DataManager.Create("oracle_connection")) {
					DataTable dt = dm1.Read(sql, "IV", paramList);
					dgvOracle.DataSource = dt;
				}

				// now pull data from mysql
				using (DataManager dm2 = DataManager.Create("mysql_connection")) {
					DataTable dt = dm2.Read(sql, "IV", paramList);
					dgvMySQL.DataSource = dt;
				}

				// now pull from sql server
				using (DataManager dm3 = DataManager.Create("sqlserver_connection")) {
					DataTable dt = dm3.Read(sql, "IV", paramList);
					dgvSqlServer.DataSource = dt;
				}


				// Take a moment to let things sink in as far as DataManager goes.  A lot of hours
				// of code and thought has gone into the approach to boil data access down to the essentials,
				// and it does a lot of the boilerplate-type code for you.  It may be tempting to use the
				// built-in .NET Odbc objects, but those suffer from performance problems, stability issues,
				// syntax limitations, and data limitations that _could_ prove to bite us in the end.  Using
				// this DataManager approach (which can use Odbc if need be, by the way), we future-proof our
				// application to be able to support other database vendors somewhat transparently -- say
				// support for PostgreSQL needs to be added, it would not be a gargantuan task.


				// As a side note, there are also static methods on DataManager that allow you to
				// do one-liner access to the database, but it assumes the connectionStringSettingName is
				// 'DataManager', which we are not doing in this example.  However, if we were, this is
				// what it would look like:

				// dgvOracle.DataSource = DataManager.ExecRead(sql, "IV", paramList);

			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void dgvOracle_Scroll(object sender, ScrollEventArgs e) {
			syncScroll((DataGridView)sender, e);

		}

		private void dgvMySQL_Scroll(object sender, ScrollEventArgs e) {
			syncScroll((DataGridView)sender, e);

		}

		private void dgvSqlServer_Scroll(object sender, ScrollEventArgs e) {
			syncScroll((DataGridView)sender, e);
		}

		private void syncScroll(DataGridView dgv, ScrollEventArgs e) {
			if (!chkSyncScroll.Checked) {
				return;
			}
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll) {
				dgvOracle.HorizontalScrollingOffset = e.NewValue;
				dgvMySQL.HorizontalScrollingOffset = e.NewValue;
				dgvSqlServer.HorizontalScrollingOffset = e.NewValue;
			} else {
				// align by rowindex since VerticalScrollingOffset is read-only.
				int row = dgv.FirstDisplayedScrollingRowIndex;
				dgvOracle.FirstDisplayedScrollingRowIndex = row;
				dgvMySQL.FirstDisplayedScrollingRowIndex = row;
				dgvSqlServer.FirstDisplayedScrollingRowIndex = row;
			}
		}

	}
}
