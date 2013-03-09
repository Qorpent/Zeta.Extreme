#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapFact.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IOlapFact<RowType, ColumnType, MainObjectType, DetailObjectType> :
		IWithCellData, IOlapVector<RowType, ColumnType, MainObjectType, DetailObjectType>
		where RowType : IOlapRow
		where ColumnType : IOlapColumn
		where MainObjectType : IOlapMainObjectBase
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType> {}
}