#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodCourse.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public class PeriodCourse : IPeriodCourse {
		public virtual int Id { get; set; }
		public virtual int Year { get; set; }
		public virtual int Period { get; set; }
		public virtual string InType { get; set; }
		public virtual string OutType { get; set; }
		public virtual decimal Value { get; set; }
		public virtual DateTime Version { get; set; }
	}
}