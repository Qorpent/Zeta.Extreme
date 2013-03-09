#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWthRefTo.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Comdiv.Olap.Model {
	public interface IWthRefTo<T> {
		T RefTo { get; set; }
	}
}