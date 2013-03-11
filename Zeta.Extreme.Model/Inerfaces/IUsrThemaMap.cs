#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IUsrThemaMap.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IUsrThemaMap : IWithId, IWithVersion {
		IZetaMainObject Object { get; set; }
		IZetaUnderwriter Usr { get; set; }
		string System { get; set; }
		string Thema { get; set; }
		bool IsPlan { get; }
		string ThemaCode { get; }
	}
}