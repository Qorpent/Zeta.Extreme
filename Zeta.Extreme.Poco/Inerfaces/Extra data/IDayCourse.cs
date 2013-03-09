#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDayCourse.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IDayCourse : IWithId, IWithVersion {
		DateTime Day { get; set; }
		string InType { get; set; }
		string OutType { get; set; }
		decimal Value { get; set; }
	}
}