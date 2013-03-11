#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCells.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithCells<T, R, C, M, D> where T : IOlapCell<R, C, M, D>
	                                           where R : IOlapRow
	                                           where C : IOlapColumn
	                                           where M : IOlapMainObjectBase
	                                           where D : IOlapDetailObjectBase<M> {
		IList<T> Cells { get; set; }
	                                           }
}