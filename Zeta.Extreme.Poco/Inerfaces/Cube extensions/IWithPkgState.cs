#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithPkgState.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithPkgState {
		PkgState State { get; set; }
	}
}