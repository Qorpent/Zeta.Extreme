#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCellData.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithCellData {
		StandardRowData RowData { get; set; }
		object Value { get; set; }
	}
}