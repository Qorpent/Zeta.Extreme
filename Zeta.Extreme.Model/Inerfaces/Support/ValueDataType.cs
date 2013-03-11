#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ValueDataType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Flags]
	public enum ValueDataType {
		Decimal = 0,
		Int = 1,
		Bool = 2,
		Date = 3,
		String = 4,
		Dictionary = 5,
		Class = 6,
		Lookup = 7,
		Undefined = 8,
	}
}