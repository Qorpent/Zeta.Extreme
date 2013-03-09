#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDirectDate.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Comdiv.Olap.Model {
	public interface IWithDirectDate {
		DateTime DirectDate { get; set; }
	}
}