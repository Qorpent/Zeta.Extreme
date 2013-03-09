#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithOlapColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Comdiv.Olap.Model {
	public interface IWithOlapColumn<ColumnType> where ColumnType : IOlapColumn {
		ColumnType Column { get; set; }
	}
}