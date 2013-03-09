#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaUnderwriter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Comdiv.Security;

namespace Comdiv.Zeta.Model {
	public interface IZetaUnderwriter :
		IEntityDataPattern,
		IWithMainObject<IZetaMainObject>,
		IWithContactHuman {
		string Login2 { get; set; }
		string SlotList { get; set; }
		IList<string> Slots { get; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		[global::Zeta.Extreme.Poco.Deprecated.Map] string Documents { get; set; }

		bool IsFor(string slot);
		}
}