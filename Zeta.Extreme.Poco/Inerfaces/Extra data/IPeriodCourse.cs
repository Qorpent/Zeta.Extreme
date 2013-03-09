#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPeriodCourse.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IPeriodCourse : IWithId, IWithVersion {
		int Year { get; set; }
		int Period { get; set; }
		string InType { get; set; }
		string OutType { get; set; }
		decimal Value { get; set; }
	}
}