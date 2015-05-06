using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GrinGlobal.Core {
	/// <summary>
	/// Summary description for Smtp
	/// </summary>
	internal class SmtpNative {

		public static int SmtpPortConfigSetting {
			get {
				return Toolkit.GetSetting("SmtpPort", 25);
			}
		}

		public static string SmtpServerConfigSetting {
			get {
				return Toolkit.GetSetting("SmtpServer", "mail.gringlobal.org");
			}
		}

		public SmtpNative() {
			_serverName = SmtpServerConfigSetting;
			_port = SmtpPortConfigSetting;
		}

		private const string UNIQUE_BOUNDARY = "ggc-unique-mime-boundary-2010";
		private const int ATTACHMENT_CHUNK_SIZE = 1048576;
		private const int MAX_LINE_LENGTH = 76;

		protected Socket _socket;
		protected int _port;

		private string _serverName;
		public string ServerName {
			get { return _serverName; }
			set { _serverName = value; }
		}

		private string _response;
		protected string Response {
			get { return _response; }
		}
		protected string _expectedResponse;


		protected bool isConnected {
			get { return _socket != null && _socket.Connected; }
		}

		private string _serverIp;
		protected void connect() {
			if (_socket == null || !_socket.Connected) {
				IPHostEntry entry = Dns.GetHostEntry(_serverName);
				_serverIp = entry.AddressList[0].ToString();
				System.Diagnostics.Debug.WriteLine("Attempting to connect to " + _serverIp);
				IPEndPoint endpoint = new IPEndPoint(entry.AddressList[0], _port);

				_socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				_socket.Connect(endpoint);
				receive(false);
			}
		}

		protected void send(string msg, int sleep) {
			byte[] bytes = new byte[1024];

			if (sleep > 0) {
				Thread.Sleep(sleep);
			}

			// ignore anything the server sent us between requests
			while (_socket.Available > 0) {
				int read = 0;
				read = _socket.Receive(bytes, 0, bytes.Length, SocketFlags.None);
				Debug.WriteLine("Client ignored server message of " + Encoding.ASCII.GetString(bytes, 0, read));
				if (sleep > 0) {
					Thread.Sleep(sleep);
				}
			}
			msg += "\r\n";
			Debug.WriteLine("Client sending " + msg);
			bytes = Encoding.ASCII.GetBytes(msg);
			_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
		}

		protected void send(string msg) {
			send(msg, 0);
		}

		protected string receive(bool multiLine) {
			StreamReader sr = new StreamReader(new NetworkStream(_socket));
			StringBuilder resp = new StringBuilder();
			string temp = null;
			if (!multiLine) {
				temp = sr.ReadLine();
				// 'single line' requests may be continued
				// using the "-" as the 4th char in the response.
				while (temp.Length > 3 && temp.Substring(3, 1) == "-") {
					temp = sr.ReadLine();
				}
				_response = temp + "\r\n";
			} else {
				// 'multi line' requests always end with a dot on a single line:
				// lots of text here\r\n
				// .\r\n

				// so we should look for .\r\n on a single line
				while ((temp = sr.ReadLine()) != ".") {
					resp.Append(temp + "\r\n");
				}

				// a ".\r\n" that is actual data must be escaped
				_response = resp.ToString().Replace("\r\n..\r\n", "\r\n.\r\n");
			}

			Debug.WriteLine("Server " + _serverIp + " responded " + _response);

			return _response;

		}

		protected void quit() {
			if (_socket != null && _socket.Connected) {
				send("QUIT");
				receive(false);
				Debug.WriteLine("Disconnecting from server " + _serverIp);
				_socket.Close();
			}
		}

		protected bool isValidResponse() {
			return isValidResponse(_response);
		}

		protected bool isValidResponse(string response) {
			return response.IndexOf(_expectedResponse) == 0;
		}


		public void Send(Email email) {
			if (email == null) {
				throw new ArgumentNullException("Email", "A non-null GrinGlobal.Core.Email object is required");
			}


			// if the email specifies a server name, use it instead of the default
			if (email.ServerName != null && email.ServerName.Trim().Length > 0) {
				_serverName = email.ServerName;
			}

			try {
				// connect
				_expectedResponse = "220";
				connect();

				if (!isValidResponse()) {
					throw new ApplicationException("Connect to server " + _serverIp + " failed. Error=" + _response);
				}

				_expectedResponse = "250";
				send("HELO " + Dns.GetHostName(), 100);
				if (!isValidResponse(receive(false))) {
					Debug.WriteLine("HELO error=" + _response);
					throw new ApplicationException("Greeting from server " + _serverIp + " failed. Error=" + _response);
				}

				send("MAIL From: " + email.From);
				if (!isValidResponse(receive(false))) {
					Debug.WriteLine("MAIL error=" + _response);
					throw new ApplicationException("From: message to server " + _serverIp + " failed. Error=" + _response);
				}

				StringBuilder recipients = new StringBuilder();
				if (email.To != null && email.To.Length > 0) {
					recipients.Append(";RCPT To: ");
					email.To = email.To.Trim();
					while (email.To.StartsWith(";")) {
						email.To = email.To.Substring(1);
					}
					while (email.To.EndsWith(";")) {
						email.To = email.To.Substring(0, email.To.Length - 1);
					}
					recipients.Append(email.To.Replace(";", ";RCPT To: ").Replace(",", ";RCPT To: "));
				}
				if (email.CC != null && email.CC.Length > 0) {
					if (recipients.Length > 0) {
						recipients.Append(";RCPT To: ");
					}
					email.CC = email.CC.Trim();
					while(email.CC.StartsWith(";")) {
						email.CC = email.CC.Substring(1);
					}
					while (email.CC.EndsWith(";")) {
						email.CC = email.CC.Substring(0, email.CC.Length - 1);
					}
					recipients.Append(email.CC.Replace(";", ";RCPT To: ").Replace(",", ";RCPT To: "));
				}
				if (email.BCC != null && email.BCC.Length > 0) {
					if (recipients.Length > 0) {
						recipients.Append(";RCPT To: ");
					}
					email.BCC = email.BCC.Trim();
					while (email.BCC.StartsWith(";")) {
						email.BCC = email.BCC.Substring(1);
					}
					while (email.BCC.EndsWith(";")) {
						email.BCC = email.BCC.Substring(0, email.BCC.Length - 1);
					}
					recipients.Append(email.BCC.Replace(";", ";RCPT To: ").Replace(",", ";RCPT To: "));
				}

				string[] dest = recipients.ToString().Split(new char[] { ';' });
				for (int i = 1; i < dest.Length; i++) {
					if (dest[i] != null && dest[i].Trim() != "RCPT To:") {
						send(dest[i]);
						if (!isValidResponse(receive(false))) {
							Debug.WriteLine("RCPT error=" + _response);
							throw new ApplicationException("RCPT message to server " + _serverIp + " failed. Error for '" + dest[i] + "'=" + _response);
						}
					}
				}


				_expectedResponse = "354";
				send("DATA");
				if (!isValidResponse(receive(false))) {
					Debug.WriteLine("DATA error=" + _response);
					throw new ApplicationException("Entering DATA state on server " + _serverIp + " failed. Error=" + _response);
				}

				// this is the "DATA" section, which is headers, body, and any attachments.

				StringBuilder dataSection = new StringBuilder();

				dataSection.Append("From: ");
				dataSection.Append(email.From);
				dataSection.Append("\r\n");

				dataSection.Append("Reply-To: ");
				if (email.ReplyTo == null || email.ReplyTo.Length == 0) {
					// no reply-to specified.  use From:
					dataSection.Append(email.From);
				} else {
					dataSection.Append(email.ReplyTo);
				}
				dataSection.Append("\r\n");


				if (email.To != null && email.To.Length > 0) {
					dataSection.Append("To: ");
					dataSection.Append(email.To);
					dataSection.Append("\r\n");
				}
				if (email.CC != null && email.CC.Length > 0) {
					dataSection.Append("Cc: ");
					dataSection.Append(email.CC);
					dataSection.Append("\r\n");
				}
				if (email.BCC != null && email.BCC.Length > 0) {
					dataSection.Append("Bcc: ");
					dataSection.Append(email.BCC);
					dataSection.Append("\r\n");
				}
				dataSection.Append("Subject: ");
				dataSection.Append(email.Subject);
				dataSection.Append("\r\n");


				switch (email.Priority) {
					case EmailPriority.High:
						dataSection.Append("X-Priority: 1\r\n");
						dataSection.Append("Importance: High\r\n");
						break;
					case EmailPriority.Normal:
						dataSection.Append("X-Priority: 3\r\n");
						dataSection.Append("Importance: Normal\r\n");
						break;
					case EmailPriority.Low:
						dataSection.Append("X-Priority: 5\r\n");
						dataSection.Append("Importance: Low\r\n");
						break;
				}

				dataSection.Append("X-Mailer: GrinGlobal.Core.SmtpNative\r\n"); // ver. ");
				//				dataSection.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
				//				dataSection.Append(" (Core component)\r\n" );

				dataSection.Append("Date: ");
				string fmt = "ddd, dd MMM yyyy HH:mm:ss ZZZZ";
				string dt = DateTime.Now.ToUniversalTime().ToString(fmt);
				dataSection.Append(dt);
				//				dataSection.Append(DateTime.Now.ToString("ddd, d M y H:m:s z" ));
				dataSection.Append("\r\n");

				// attachments go here
				if (email.Attachments.Count == 0) {

					if (email.Format == EmailFormat.Html) {
						dataSection.Append("Content-Type: text/html\r\n");
						dataSection.Append("Content-Transfer-Encoding: 7Bit\r\n");
					}

					dataSection.Append("\r\n");
					dataSection.Append(email.Body.Replace("\r\n.\r\n", "\r\n..\r\n"));
					if (dataSection[dataSection.Length - 2] != '\r' || dataSection[dataSection.Length - 1] != '\n') {
						dataSection.Append("\r\n.");
					} else {
						dataSection.Append(".");
					}
				} else {
					dataSection.Append("MIME-Version: 1.0\r\n");
					dataSection.Append("Content-Type: multipart/mixed; boundary=");
					dataSection.Append(UNIQUE_BOUNDARY);
					dataSection.Append("\r\n");
					dataSection.Append("\r\n");
					dataSection.Append("This is a multi-part message in MIME format.\r\n");

					dataSection.Append("--");
					dataSection.Append(UNIQUE_BOUNDARY);
					dataSection.Append("\r\n");
					if (email.Format == EmailFormat.Html) {
						dataSection.Append("Content-Type: text/html\r\n");
						dataSection.Append("Content-Transfer-Encoding: 7Bit\r\n");
					} else {
						dataSection.Append("Content-Type: text/plain\r\n");
						dataSection.Append("Content-Transfer-Encoding: 7Bit\r\n");
					}
					dataSection.Append("\r\n");
					dataSection.Append(email.Body.Replace("\r\n.\r\n", "\r\n..\r\n"));
					if (dataSection[dataSection.Length - 2] != '\r' || dataSection[dataSection.Length - 1] != '\n') {
						dataSection.Append("\r\n");
					}

					int maxChunkSize = 10485760;

					foreach (Attachment a in email.Attachments) {
						dataSection.Append("--");
						dataSection.Append(UNIQUE_BOUNDARY);
						dataSection.Append("\r\n");
						dataSection.Append("Content-Type: application/octet-stream; file=" + a.Name + "\r\n");
						dataSection.Append("Content-Transfer-Encoding: base64\r\n");
						dataSection.Append("Content-Disposition: attachment; filename=" + a.Name + "\r\n");
						dataSection.Append("\r\n");


						char[] chars = a.ToString().ToCharArray();

						if (MAX_LINE_LENGTH >= chars.Length) {
							dataSection.Append(chars);
							send(dataSection.ToString());
							dataSection = new StringBuilder();
						} else {
							// this is an attachment that is larger then 76 chars -- send it in chunks
							// with each line not exceeding 76 chars
							int offset = 0;
							int chunk = 0;
							send(dataSection.ToString());
							dataSection = new StringBuilder();

							while (offset < chars.Length - MAX_LINE_LENGTH) {
								dataSection.Append(chars, offset, MAX_LINE_LENGTH);
								offset += MAX_LINE_LENGTH;
								chunk += MAX_LINE_LENGTH;

								if (chunk >= maxChunkSize - MAX_LINE_LENGTH) {
									// send the chunk of data
									send(dataSection.ToString());
									dataSection = new StringBuilder();
									chunk = 0;
								} else {
									// do not send the chunk of data.
									// insert crlf
									dataSection.Append("\r\n");
								}

							}

							if (offset < chars.Length) {
								// there's a little left over on the last line.
								dataSection.Append(chars, offset, chars.Length - offset);
								send(dataSection.ToString());
								dataSection = new StringBuilder();
							}
						}
					}
					dataSection.Append("--");
					dataSection.Append(UNIQUE_BOUNDARY);
					dataSection.Append("--\r\n");
					dataSection.Append(".");
				}

				_expectedResponse = "250";
				send(dataSection.ToString());
				if (!isValidResponse(receive(false))) {
					Debug.WriteLine("DATA error=" + _response);
					throw new ApplicationException("DATA section message to server " + _serverIp + " failed. Error=" + _response);
				}
			} finally {
				quit();
			}
		}
	}
}
