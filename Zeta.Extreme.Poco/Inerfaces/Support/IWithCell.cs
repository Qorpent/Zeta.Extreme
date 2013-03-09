#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;

namespace Comdiv.Olap.Model {
	public interface IWithCell<T, R, C, M, D> where T : IOlapCell<R, C, M, D>
	                                          where R : IOlapRow
	                                          where C : IOlapColumn
	                                          where M : IOlapMainObjectBase
	                                          where D : IOlapDetailObjectBase<M> {
		[Classic("MainDataRow")] T Cell { get; set; }
	                                          }
}