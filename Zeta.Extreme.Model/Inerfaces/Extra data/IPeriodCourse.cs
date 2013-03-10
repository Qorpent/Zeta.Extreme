#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPeriodCourse.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IPeriodCourse : IWithId, IWithVersion {
		int Year { get; set; }
		int Period { get; set; }
		string InType { get; set; }
		string OutType { get; set; }
		decimal Value { get; set; }
	}
}