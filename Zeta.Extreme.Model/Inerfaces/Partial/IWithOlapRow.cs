#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithOlapRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithOlapRow<RowType>
		where RowType : IOlapRow {
		[Classic("MainDataTree")] RowType Row { get; set; }
		}
}