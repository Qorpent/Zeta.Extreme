#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DayCourse.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public class daycourse : IDayCourse {
		public virtual int Id { get; set; }
		public virtual DateTime Day { get; set; }
		public virtual string InType { get; set; }
		public virtual string OutType { get; set; }
		public virtual decimal Value { get; set; }
		public virtual DateTime Version { get; set; }
	}
}