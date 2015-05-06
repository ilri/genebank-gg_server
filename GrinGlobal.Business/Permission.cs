using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Business {
	public enum PermissionLevel {
		Inherit = 0,
        Allow = 1,
        Deny = 2,
        VariesByRow = 3
	}

	public enum PermissionAction : int {
		Create = 0,
		Read = 1,
		Update = 2,
		Delete = 3
	}

	public class Permission {

		public static Permission Create {
			get {
				return new Permission(PermissionAction.Create, PermissionLevel.Allow);
			}
		}
		public static Permission Read {
			get {
				return new Permission(PermissionAction.Read, PermissionLevel.Allow);
			}
		}
		public static Permission Update {
			get {
				return new Permission(PermissionAction.Update, PermissionLevel.Allow);
			}
		}
		public static Permission Delete {
			get {
				return new Permission(PermissionAction.Delete, PermissionLevel.Allow);
			}
		}




		/// <summary>
		/// Given a maximum of all four distinct permission actions (Create, Read, Update, Delete), returns an array of all four permission actions with levels set to passed-in values, and non-passed-in actions are defaulted to Inherit.
		/// </summary>
		/// <param name="permissions"></param>
		/// <returns></returns>
		public static Permission[] CRUDPermissions(Permission[] permissions) {
			Permission[] output = CRUDPermissions(PermissionLevel.Inherit);
			if (permissions != null && permissions.Length > 0) {
				foreach (Permission p in permissions) {
					output[(int)p.Action].Level = p.Level;
				}
			}
			return output;
		}

		/// <summary>
		/// Creates the four CRUD Permission objects with given PermissionLevel
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public static Permission[] CRUDPermissions(PermissionLevel level) {
			Permission[] output = new Permission[]{
					new Permission(PermissionAction.Create, level),
					new Permission(PermissionAction.Read, level),
					new Permission(PermissionAction.Update, level),
					new Permission(PermissionAction.Delete, level),
			};

			return output;

		}

        /// <summary>
        /// Creates an array of permission objects, all with Deny except Read, which is Allow.
        /// </summary>
        /// <returns></returns>
        public static Permission[] CRUDPermissionsReadOnly() {
            Permission[] output = new Permission[]{
					new Permission(PermissionAction.Create, PermissionLevel.Deny),
					new Permission(PermissionAction.Read, PermissionLevel.Allow),
					new Permission(PermissionAction.Update, PermissionLevel.Deny),
					new Permission(PermissionAction.Delete, PermissionLevel.Deny),
			};

            return output;
        }

		/// <summary>
		/// The Level this Permission represents.  Inherit (-1), Deny (0), or Allow (1)
		/// </summary>
		public PermissionLevel Level { get; set; }

		/// <summary>
		/// The Action this Permission applies to.  Create, Read, Update, or Delete
		/// </summary>
		public PermissionAction Action { get; set; }
		public Permission(PermissionAction action, PermissionLevel level) {
			Level = level;
			Action = action;
		}

		public override string ToString() {
			return this.Action + " -> " + this.Level;
		}

        /// <summary>
        /// Creates a deep copy of this object
        /// </summary>
        /// <returns></returns>
        public Permission Clone() {
            Permission p = new Permission(this.Action, this.Level);
            return p;
        }

        /// <summary>
        /// Creates a deep copy of the given array
        /// </summary>
        /// <param name="perms"></param>
        /// <returns></returns>
        public static Permission[] Clone(Permission[] perms) {
            if (perms == null) {
                return null;
            }

            var output = new Permission[perms.Length];
            for(var i=0;i<perms.Length;i++){
                output[i] = perms[i].Clone();
            }
            return output;
        }

		/// <summary>
		/// Parses given databaseValue as corresponding PermissionLevel. 'A' maps to PermissionLevel.Allow.  'D' maps to PermissionLevel.Deny.  'V' maps to PermissionLevel.VariesByRow.  All other values map to PermissionLevel.Inherit.
		/// </summary>
		/// <param name="databaseValue"></param>
		/// <returns></returns>
		public static PermissionLevel ParseLevel(string databaseValue){
			if (String.IsNullOrEmpty(databaseValue)) {
				return PermissionLevel.Inherit;
			}
			switch (databaseValue.ToUpper()) {
				case "A":
                //case "ALLOW":
                //case "ALLOWED":
					return PermissionLevel.Allow;
				case "D":
                //case "DENY":
                //case "DENIED":
					return PermissionLevel.Deny;
                case "V":
                //case "VARY":
                //case "VARIES":
                    return PermissionLevel.VariesByRow;
                case "I":
				default:
					return PermissionLevel.Inherit;
			}
		}

		/// <summary>
		/// Returns 'A' for PermissionLevel.Allow, 'D' for PermissionLevel.Deny, 'I' for PermissionLevel.Inherit, 'V' for PermissionLevel.VariesByRow.
		/// </summary>
		public string DatabaseValue {
			get {
				switch(Level){
					case PermissionLevel.Inherit:
					default:
						return "I";
					case PermissionLevel.Allow:
						return "A";
					case PermissionLevel.Deny:
						return "D";
                    case PermissionLevel.VariesByRow:
                        return "V";
				}
			}
		}
	}
}
