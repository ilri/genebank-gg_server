using System;
using System.Data;
using System.Configuration;
using System.Web;

using System.IO;

namespace GrinGlobal.Core {
	/// <summary>
	/// Logs all HTTP-based requests to the current application IF config setting "HttpLogFile" is set AND can write to that file AND can create temp files in that directory AND web.config contains the following entry:
	/// &lt;httpModules&gt;
	///		&lt;add
	///		   type="GrinGlobal.Core.HttpLogger,GrinGlobal.Core"
	///		   name="HttpLogger" /&gt;
	///	&lt;/httpModules&gt;
	/// in the &lt;system.web&gt; element.
	/// </summary>
	public class HttpLogger : IHttpModule {
		#region IHttpModule Members

		private static string __logFileName;
		private static object __lockObject;
		private static string __requestText;

		/// <summary>
		/// Cleans up any resources
		/// </summary>
		public void Dispose() {

		}

		/// <summary>
		/// Sets up the begin/end request event handlers
		/// </summary>
		/// <param name="context"></param>
		public void Init(HttpApplication context) {
			if (__lockObject == null){
				__lockObject = new Object();
			}
			lock (__lockObject) {
				if (__logFileName == null) {
					__logFileName = Toolkit.GetSetting("HttpLogFile", "");
					if (!String.IsNullOrEmpty(__logFileName)) {

						// the config file is pointing to a log file.  so enable the begin/end request logging...

						context.BeginRequest += new EventHandler(context_BeginRequest);
						context.EndRequest += new EventHandler(context_EndRequest);
						//__logFileName = __logFileName.Replace("~", ".");
						__logFileName = Toolkit.ResolveFilePath(__logFileName, false);
					}
				}
			}
		}

		void context_BeginRequest(object sender, EventArgs e) {
			try {
				HttpApplication app = (HttpApplication)sender;
				if (!String.IsNullOrEmpty(__logFileName)) {
					// d'oh each request hammers the entire file. save it, then append it to a more permanent log.
					lock (__lockObject) {
						app.Context.Request.SaveAs(__logFileName + ".req", true);
						HttpResponse resp = app.Context.Response;
						//resp.Filter = new LoggerFilter(resp.Filter, __logFileName + ".resp");
                        var txt = System.IO.File.ReadAllText(__logFileName + ".req");
						__requestText = "============================================================\r\nREQUEST " + DateTime.Now.ToUniversalTime() + ":\r\n\r\n" + txt + "\r\n----------------------------------\r\n";
						File.AppendAllText(__logFileName, __requestText);
					}
				}
			} catch (Exception ex) {
				// eat all errors when logging
				System.Diagnostics.Debug.WriteLine("eating http begin_request logging error: " + ex.Message);
			}
		}

		void context_EndRequest(object sender, EventArgs e) {
			try {
				HttpApplication app = (HttpApplication)sender;
				if (!String.IsNullOrEmpty(__logFileName)) {
					// d'oh each request hammers the entire file. save it, then append it to a more permanent log.
					lock (__lockObject) {
                        var txt = System.IO.File.ReadAllText(__logFileName + ".resp");
						string responseText = "RESPONSE " + DateTime.Now.ToUniversalTime() + ":\r\n\r\n" + txt + "\r\n============================================================\r\n\r\n";
						File.AppendAllText(__logFileName, responseText);
					}
				}
			} catch (Exception ex) {
				// eat all errors when logging
				System.Diagnostics.Debug.WriteLine("eating http end_request logging error: " + ex.Message);
			}
		}

		#endregion
	}

	internal class LoggerFilter : FileStream {
		private Stream _input;
		internal LoggerFilter(Stream inputStream, string outputFile) : base(outputFile, FileMode.Create) {
			_input = inputStream;
		}

		public override void WriteByte(byte value) {
			_input.WriteByte(value);
			base.WriteByte(value);
		}

		public override void Write(byte[] buffer, int offset, int count) {
			_input.Write(buffer, offset, count);
			base.Write(buffer, offset, count);
		}

	}
}
