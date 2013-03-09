#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapRowMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Comdiv.Olap.Model {
	public interface IOlapRowMark<RowType> :
		IItemDataPattern,
		IMarkLink<RowType> //,
		// IWithOlapRow<RowType>
		where RowType : IOlapRow {}
}