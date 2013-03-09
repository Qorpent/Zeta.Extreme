#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormStates.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Comdiv.Zeta.Model {
	[Flags]
	public enum FormStates {
		None = 0,
		Open = 1,
		Closed = 2,
		Accepted = 4,
		Rejected = 8
	}
}