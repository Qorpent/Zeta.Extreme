#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithRange.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithRange {
		DateRange Range { get; set; }
	}
}