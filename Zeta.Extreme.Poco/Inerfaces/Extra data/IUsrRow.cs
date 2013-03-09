#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IUsrRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IUsrRow : IWithId,IWithName, IWithCode {
		bool Active { get; set; }
		IZetaUnderwriter Usr { get; set; }
		IZetaRow Row { get; set; }
	}
}