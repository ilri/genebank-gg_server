using System;
using System.IO;

namespace GrinGlobal.Core {
	/// <summary>
	/// Class for sending email.
	/// </summary>
	public class Email {

		/// <summary>
		/// Gets the smtp server host name from the config file. default is mail.GrinGlobal.Core.com if not specified.
		/// </summary>
		public static string SmtpServerConfigSetting {
			get { return SmtpNative.SmtpServerConfigSetting; }
		}

		/// <summary>
		/// Gets the port from the config file. default is 25 if not specified.
		/// </summary>
		public static int SmtpServerPortSetting {
			get { return SmtpNative.SmtpPortConfigSetting; }
		}

		/// <summary>
		/// Sends an Email using default settings.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		public static void Send(string to, string from, string cc, string bcc, string subject, string body) {
			Email e = new Email();
			e.To = to;
			e.From = from;
			e.CC = cc;
			e.BCC = bcc;
			e.Subject = subject;
			e.Body = body;
			e.Send();
		}

		/// <summary>
		/// Sends an Email via the given smtp server
		/// </summary>
		/// <param name="smtpServerName"></param>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		public static void Send(string smtpServerName, string to, string from, string cc, string bcc, string subject, string body) {
			Email e = new Email();
			e.ServerName = smtpServerName;
			e.To = to;
			e.From = from;
			e.CC = cc;
			e.BCC = bcc;
			e.Subject = subject;
			e.Body = body;
			e.Send();
		}

		/// <summary>
		/// Creates an Email object
		/// </summary>
		public Email() {
			_id = 0;
			_to = "";
			_from = "";
			_replyTo = "";
			_cc = "";
			_bcc = "";
			_subject = "";
			_body = "";
			_maxLength = 0;
			_userId = null;
			_password = null;
			_serverPort = SmtpNative.SmtpPortConfigSetting;
			_serverName = SmtpNative.SmtpServerConfigSetting;
			_connectionTimeout = 60;
			_format = EmailFormat.Text;
			_priority = EmailPriority.Normal;
			_toBeSentAt = DateTime.Now;
			_attachments = new Attachments();
		}

		/// <summary>
		/// Creates an email object with the specified id.  Id is only intended for informational purposes -- can be used as primary key in a database. has no relevence to the email itself though.
		/// </summary>
		/// <param name="id"></param>
		public Email(int id) : this() {
			_id = id;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		public Email(string to, string from, string cc, string bcc, string subject, string body)
			: this(to, from, cc, bcc, subject, body, EmailFormat.Html, EmailPriority.Normal) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="format"></param>
		/// <param name="priority"></param>
		public Email(string to, string from, string cc, string bcc, string subject, string body, EmailFormat format, EmailPriority priority) : this() {
			_to = to;
			_from = from;
			_cc = cc;
			_bcc = bcc;
			_subject = subject;
			_body = body;
			_format = format;
			_priority = priority;

		}

		private int _id;
		/// <summary>
		/// An identifier for this object. Can be used as a primary key in a database if needed. Has no other relevence to the email itself, though.
		/// </summary>
		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		private string _userId;
		/// <summary>
		/// Gets or sets the user id for Smtp credentials 
		/// </summary>
		public string UserId {
			get { return _userId; }
			set { _userId = value; }
		}

		private string _password;
		/// <summary>
		/// Gets or sets the password for Smtp credentials 
		/// </summary>
		public string Password {
			get { return _password; }
			set { _password = value; }
		}

		private int _serverPort;
		/// <summary>
		/// Gets or sets the port for the smtp server. default is 25.
		/// </summary>
		public int ServerPort {
			get { return _serverPort; }
			set { _serverPort = value; }
		}

		private string _serverName;
		/// <summary>
		/// Gets or sets the name of the smtp server. default is mail.GrinGlobal.Core.com
		/// </summary>
		public string ServerName {
			get { return _serverName; }
			set { _serverName = value; }
		}

		private int _connectionTimeout;
		/// <summary>
		/// Gets or sets the connection timeout for the smtp server. default is 60 seconds.
		/// </summary>
		public int ConnectionTimeout {
			get { return _connectionTimeout; }
			set { _connectionTimeout = value; }
		}

		private string _to;
		/// <summary>
		/// Gets or sets the To email address
		/// </summary>
		public string To {
			get { return _to; }
			set { _to = value; }
		}

		private string _from;
		/// <summary>
		/// Gets or sets the From email address
		/// </summary>
		public string From {
			get { return _from; }
			set { _from = value; }
		}

		private string _replyTo;
		/// <summary>
		/// Gets or sets the ReplyTo email address
		/// </summary>
		public string ReplyTo {
			get { return _replyTo; }
			set { _replyTo = value; }
		}

		private string _cc;
		/// <summary>
		/// Gets or sets the CC email address
		/// </summary>
		public string CC {
			get { return _cc; }
			set { _cc = value; }
		}

		private string _bcc;
		/// <summary>
		/// Gets or sets the BCC email address
		/// </summary>
		public string BCC {
			get { return _bcc; }
			set { _bcc = value; }
		}

		private string _subject;
		/// <summary>
		/// Gets or sets the subject
		/// </summary>
		public string Subject {
			get { return _subject; }
			set { _subject = value; }
		}

		private string _body;
		/// <summary>
		/// Gets or sets the body
		/// </summary>
		public string Body {
			get { return _body; }
			set { _body = value; }
		}

		private DateTime _toBeSentAt;
		/// <summary>
		/// Gets or sets the DateTime when this email should be sent. Ignored when Send() is called directly.  For database support only.
		/// </summary>
		public DateTime ToBeSentAt {
			get { return _toBeSentAt; }
			set { _toBeSentAt = value; }
		}

		private int _maxLength;
		/// <summary>
		/// Gets or sets the maximum length of the email.
		/// </summary>
		public int MaximumLength {
			get { return _maxLength; }
			set { _maxLength = value; }
		}

		private Attachments _attachments;
		/// <summary>
		/// Gets the collection of Attachments associated with this email
		/// </summary>
		public Attachments Attachments {
			get { return _attachments; }
		}

		private EmailFormat _format;
		/// <summary>
		/// Gets or sets the format - text or html.  Default is Text
		/// </summary>
		public EmailFormat Format {
			get { return _format; }
			set { _format = value; }
		}

		private EmailPriority _priority;
		/// <summary>
		/// Gets or sets the priority - low, normal, high. default is normal.
		/// </summary>
		public EmailPriority Priority {
			get { return _priority; }
			set { _priority = value; }
		}

		/// <summary>
		/// Sends the email
		/// </summary>
		public void Send() {
			new SmtpNative().Send(this);
		}

	}

}
