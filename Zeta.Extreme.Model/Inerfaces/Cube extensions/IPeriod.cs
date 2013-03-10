#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPeriod.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IPeriod : IEntity, Qorpent.Model.IWithFormula {
		int ClassicId { get; set; }
		string Category { get; set; }
		string ShortName { get; set; }
		bool IsDayPeriod { get; set; }
		//1899 - for override with year
		DateTime StartDate { get; set; }
		DateTime EndDate { get; set; }
		int MonthCount { get; set; }
	}
}