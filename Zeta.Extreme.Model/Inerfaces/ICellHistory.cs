#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ICellHistory.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface ICellHistory {
		string PseudoCode { get; set; }
		int RowId { get; set; }
		string Usr { get; set; }
		DateTime Time { get; set; }
		string Value { get; set; }
		int Type { get; set; }
	}
}