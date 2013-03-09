#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PkgState.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public enum PkgState {
		None = 0,
		Initial = 1,
		Closed = 2,
		Revoked = 4,
		Reserved1 = 8,
		Reserved2 = 16,
		Reserved3 = 32,
		Reserved4 = 64,
		Reserved5 = 128,
	}
}