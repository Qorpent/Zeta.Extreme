#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapVector.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces.Bases {
	public interface IOlapVector<RowType, ColumnType, MainObjectType, DetailObjectType> :
		IWithOlapRow<RowType>,
		IWithOlapTime,
		IWithOlapColumn<ColumnType>,
		IWithOlapObject<MainObjectType, DetailObjectType>
		where RowType : IOlapRow
		where ColumnType : IOlapColumn
		where MainObjectType : IOlapMainObjectBase
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType> {}
}