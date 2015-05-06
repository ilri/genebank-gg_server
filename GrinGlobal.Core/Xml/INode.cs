using System;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Summary description for INode.
	/// </summary>
	public interface INode : IAttributes {
		/// <summary>
		/// 
		/// </summary>
		Nodes Nodes { get; }

		/// <summary>
		/// 
		/// </summary>
		string NodeName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string NodeValue { get; set; }
		
	}
}
