#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapCell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces.Bases {
	public interface IOlapCell<RowType, ColumnType, MainObjectType, DetailObjectType> :
		IWithId, IWithVersion,
		IOlapFact<RowType, ColumnType, MainObjectType, DetailObjectType>
		where RowType : IOlapRow
		where ColumnType : IOlapColumn
		where MainObjectType : IOlapMainObjectBase
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType> {}
}