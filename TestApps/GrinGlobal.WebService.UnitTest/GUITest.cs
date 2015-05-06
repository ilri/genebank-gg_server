using GrinGlobal.WebService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Data;

using System.IO;
using GrinGlobal.Core;
using GrinGlobal.Sql;
using System;

namespace GrinGlobal.WebService.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for GUITest and is intended
    ///to contain all GUITest Unit Tests
    ///</summary>
	[TestClass()]
	public class GUITest {

		private string _tgtDir;

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext) {

			GUI gui = new GUI();

			//if (!File.Exists("SearchTestDataSet.xml")) {
			//    DataSet dsSearch = gui.Search(true, "brock", "passw0rd!", "100005 crop:maize");
			//    dsSearch.WriteXml(tgtDir + "SearchTestDataSet.xml");
			//}

			string tgtDir = Toolkit.ResolveDirectoryPath(@"%TEMP%\gringlobaltest\", true);

			DataSet dsToSave = gui.GetInventory(true, "brock", "passw0rd!", "BrockTestPrepare");
			if (!File.Exists(tgtDir + "SaveDataSetTestDataSet.xml")) {

				// write the after test, save it as the result to compare against
				if (dsToSave.Tables["PROD.IV"].Rows.Count > 0) {
					dsToSave.Tables["PROD.IV"].Rows[0]["LOC1"] = "junko";
					dsToSave.Tables["PROD.IV"].Rows[0]["LOC1"] = "bwaftertest";
				}
				DataSet dsSave = gui.SaveDataSet(true, "brock", "passw0rd!", dsToSave);
				dsSave.WriteXml(tgtDir + "SaveDataSetTestDataSet.xml");
			}

			// write the data back as junk so we can overwrite it with the test value during hte test
			if (dsToSave.Tables["PROD.IV"].Rows.Count > 0) {
				dsToSave.Tables["PROD.IV"].Rows[0]["LOC1"] = "bwbeforetest";
			}
			gui.SaveDataSet(true, "brock", "passw0rd!", dsToSave);


			//File.WriteAllText("GetVersion.xml", gui.GetVersion());


			//if (!File.Exists("GetGroupsTestDataSet.xml")) {
			//    DataSet dsGroups = gui.GetGroups(true, "brock", "passw0rd!", 75227);
			//    dsGroups.WriteXml(tgtDir + "GetGroupsTestDataSet.xml");
			//}

			//if (!File.Exists("CreateUserTestDataSet.xml")) {
			//    DataSet dsCreateUser = gui.CreateUser(true, "brock", "passw0rd!", "Unit_Test", "password", true);
			//    dsCreateUser.WriteXml(tgtDir + "CreateUserTestDataSet.xml");
			//}

			//gui.RenameGroup(true, "brock", "passw0rd!", "BrockTestResult", "BrockTestPrepare").WriteXml("RenameGroupTestDataSet.xml");

			//if (!File.Exists("GetResourcesTestDataSet.xml")) {
			//    DataSet dsResources = gui.GetResources(true, "brock", "passw0rd!");
			//    dsResources.WriteXml(tgtDir + "GetResourcesTestDataSet.xml");
			//}

			//if (!File.Exists("GetResourceTestDataSet.xml")) {
			//    DataSet dsResources = gui.GetResource(true, "brock", "passw0rd!", "btnSave");
			//    dsResources.WriteXml(tgtDir + "GetResourceTestDataSet.xml");
			//}

			//if (!File.Exists("ChangePasswordTestDataSet.xml")) {
			//    DataSet dsChangePw = gui.ChangePassword(true, "brock", "passw0rd!", "Unit_Test", "password2");
			//    dsChangePw.WriteXml(tgtDir + "ChangePasswordTestDataSet.xml");
			//}

			using (DataManager dm = DataManager.Create()) {
				dm.Write("delete from sec_user where user_name = 'Unit_Test'");

			}

		}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		[ClassCleanup()]
		public static void MyClassCleanup() {
		}
		//
		//Use TestInitialize to run code before running each test
		[TestInitialize()]
		public void MyTestInitialize() {
			_tgtDir = Toolkit.ResolveDirectoryPath(@"%TEMP%\gringlobaltest\", true);
		}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Search
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void SearchTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string query = "100005 crop:maize"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "SearchTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.Search(suppressExceptions, userName, password, query);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for SaveDataSet
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void SaveDataSetTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value

			DataSet ds = target.GetInventory(suppressExceptions, userName, password, "Ama.Test");
			if (ds.Tables["PROD.IV"].Rows.Count > 0) {
				string newval = new Random().Next(1, 99999999).ToString();
				ds.Tables["PROD.IV"].Rows[0]["LOC2"] = newval;
			}

			//DataSet expected = new DataSet();
			//expected.ReadXml(_tgtDir + "SaveDataSetTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.SaveDataSet(suppressExceptions, userName, password, ds);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for RenameGroup
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void RenameGroupTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string existingGroupName = "BrockTestPrepare"; // TODO: Initialize to an appropriate value
			string newGroupName = "BrockTestResult"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "RenameGroupTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.RenameGroup(suppressExceptions, userName, password, existingGroupName, newGroupName, 75227);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetVersion
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetVersionTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
//			string expected = File.ReadAllText("GetVersion.xml"); // TODO: Initialize to an appropriate value
			string actual;
			actual = target.GetVersion();
			string expected = "GrinGlobal.WebService v 1.0.";
			Assert.AreEqual(expected, actual.Substring(0,expected.Length));
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetResources
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetResourcesTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "GetResourcesTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetResources(suppressExceptions, userName, password);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetResource
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetResourceTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string name = "btnSave"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "GetResourceTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetResource(suppressExceptions, userName, password, name);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetInventory
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetInventoryTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string groupName = "Zea.Ames"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "GetInventoryTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetInventory(suppressExceptions, userName, password, groupName);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetGroups
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetGroupsTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			int cno = 0; // TODO: Initialize to an appropriate value
//			DataSet expected = null; // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetGroups(suppressExceptions, userName, password, cno);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetCooperators
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetCooperatorsTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string site = string.Empty; // TODO: Initialize to an appropriate value
//			DataSet expected = null; // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetCooperators(suppressExceptions, userName, password, site);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetAccessions
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GetAccessionsTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string groupName = string.Empty; // TODO: Initialize to an appropriate value
//			DataSet expected = null; // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.GetAccessions(suppressExceptions, userName, password, groupName);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for DeleteGroup
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void DeleteGroupTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string groupName = string.Empty; // TODO: Initialize to an appropriate value
//			DataSet expected = null; // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.DeleteGroup(suppressExceptions, userName, password, groupName, 75227);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for CreateUser
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void CreateUserTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string newUserUserName = "Unit_Test"; // TODO: Initialize to an appropriate value
			string newUserPassword = "password"; // TODO: Initialize to an appropriate value
			bool newUserEnabled = true; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "CreateUserTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.CreateUser(suppressExceptions, userName, password, newUserUserName, newUserPassword, newUserEnabled);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for ClearCache
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void ClearCacheTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string cacheName = string.Empty; // TODO: Initialize to an appropriate value
//			DataSet expected = null; // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.ClearCache(suppressExceptions, userName, password, cacheName);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for ChangePassword
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void ChangePasswordTest() {
			GUI target = new GUI(); // TODO: Initialize to an appropriate value
			bool suppressExceptions = true; // TODO: Initialize to an appropriate value
			string userName = "brock"; // TODO: Initialize to an appropriate value
			string password = "passw0rd!"; // TODO: Initialize to an appropriate value
			string targetUserName = "Unit_Test"; // TODO: Initialize to an appropriate value
			string newPassword = "password2"; // TODO: Initialize to an appropriate value
			//DataSet expected = new DataSet();
			//expected.ReadXml(tgtDir + "ChangePasswordTestDataSet.xml"); // TODO: Initialize to an appropriate value
			DataSet actual;
			actual = target.ChangePassword(suppressExceptions, userName, password, targetUserName, newPassword);
			Assert.AreEqual(0, actual.Tables["ExceptionTable"].Rows.Count);
//			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GUI Constructor
		///</summary>
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("D:\\projects\\GrinGlobal\\GrinGlobal.WebService", "/")]
		[UrlToTest("http://localhost:2600/")]
		public void GUIConstructorTest() {
			GUI target = new GUI();
			Assert.IsInstanceOfType(target, typeof(GUI));
//			Assert.Inconclusive("TODO: Implement code to verify target");
		}
	}
}
