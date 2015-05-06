using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GrinGlobal.Core {
	/// <summary>
	/// Lightweight representation of domain name and user name
	/// </summary>
	public class DomainUser {

		/// <summary>
		/// Gets the domain/user under which the caller is executing
		/// </summary>
		public static DomainUser Current {
			get {
				return new DomainUser(Environment.UserDomainName, Environment.UserName);
			}
		}

        public static DomainUser CurrentLoggedIn {
            get {
                DomainUser du = Toolkit.GetDomainUser("explorer.exe", true);
                return du;
            }
        }

		private string _domain;

		/// <summary>
		/// Gets or sets the domain for a user.  Can be a workgroup as well.
		/// </summary>
		public string Domain {
			get { return _domain; }
			set { _domain = value; }
		}

		private string _userName;

		/// <summary>
		/// Gets or sets the name of user.
		/// </summary>
		public string UserName {
			get { return _userName; }
			set { _userName = value; }
		}

		private string _password;
		/// <summary>
		/// The cleartext password for the given domain/user
		/// </summary>
		public string Password {
			get { return _password; }
			set { _password = value; }
		}


		/// <summary>
		/// Creates an instance with null domain and user name
		/// </summary>
		public DomainUser() {
		}
		/// <summary>
		/// Creates an instance with given domain and user name values
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="userName"></param>
		public DomainUser(string domain, string userName) : this(domain, userName, null) {
		}

		/// <summary>
		/// Creates an instance with given domain and user name values as well as the password
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public DomainUser(string domain, string userName, string password) {
			if (String.IsNullOrEmpty(domain) && userName != null && userName.Contains("\\")) {
				int pos = userName.IndexOf('\\');
				_domain = Toolkit.Cut(userName, 0, pos);
				_userName = Toolkit.Cut(userName, pos + 1);
			} else {
				_domain = domain;
				_userName = userName;
			}
			_password = password;
		}

		/// <summary>
		/// Returns true if given user name is a system account -- i.e. 'SYSTEM', 'LOCAL SERVICE', or 'NETWORK SERVICE'
		/// </summary>
		/// <param name="du"></param>
		/// <returns></returns>
		public static bool IsSystemAccount(DomainUser du) {
			if (String.IsNullOrEmpty(du.UserName)) {
				return false;
			}
			string un = du.UserName.ToLower();
			return un == "system" || un == "local service" || un == "network service";
		}

		/// <summary>
		/// Returns domain\user
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (String.IsNullOrEmpty(_domain)) {
				return _userName;
			} else {
				return _domain + @"\" + _userName;
			}
		}

		/// <summary>
		/// Returns true if given user is null or has no domain or user name.
		/// </summary>
		/// <param name="du"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(DomainUser du) {
			return (du == null || du.ToString() == @"\");
		}

		/// <summary>
		/// Returns true if given object is DomainUser type and Domain and UserName are same as the current object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			DomainUser du = (DomainUser)obj;
			if (obj == null) {
				return false;
			} else {
				return this.Domain == du.Domain && this.UserName == du.UserName && this.Password == du.Password;
			}
		}

		/// <summary>
		/// Returns same as object.GetHashCode();
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return base.GetHashCode();
		}


		/// <summary>
		/// Returns true if both Domain and UserName are the same (case sensitive!) - or both objects are null
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(DomainUser a, DomainUser b) {
			
			if ((object)a == null && (object)b == null) {
				return true;
			} else if ((object)a == null || (object)b == null) {
				return false;
			} else {
				return a.Domain == b.Domain && a.UserName == b.UserName && a.Password == b.Password;
			}
		}

		/// <summary>
		/// Returns true if either Domain or UserName do not match (case sensitive!)
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(DomainUser a, DomainUser b){
			return !(a == b);
		}

	}
}
