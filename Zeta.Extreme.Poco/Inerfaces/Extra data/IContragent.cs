#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IContragent.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IContragent : IEntityDataPattern {
		string FullName { get; set; }
		string Type { get; set; }
		string Address { get; set; }
		string Contact { get; set; }
	}
}