#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithMeasure.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Comdiv.Olap.Model {
	public interface IWithMeasure {
		string Measure { get; set; }
		bool IsDynamicMeasure { get; set; }
	}
}