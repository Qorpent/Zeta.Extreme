#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GraphRelationType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Flags]
	public enum GraphRelationType {
		None = 0,
		Expand = 1,
		Block = 2,
		Ignore = 4
	}
}