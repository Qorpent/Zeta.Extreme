#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaUnderwriter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IZetaUnderwriter :
		IEntity,
		IWithMainObject<IZetaMainObject>,
		IWithContactHuman {
		string Login2 { get; set; }
		string SlotList { get; set; }
		IList<string> Slots { get; }
		string Roles { get; set; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		[global::Zeta.Extreme.Poco.Deprecated.Map] string Documents { get; set; }

		bool IsFor(string slot);
		}
}