#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCellData.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithCellData {
		StandardRowData RowData { get; set; }
		object Value { get; set; }
	}
}