#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormState.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IFormState : IWithId, IWithVersion, IWithComment {
		IForm Form { get; set; }
		string State { get; set; }
		string Usr { get; set; }
		IFormState Parent { get; set; }
		string GetReadableState();
	}
}