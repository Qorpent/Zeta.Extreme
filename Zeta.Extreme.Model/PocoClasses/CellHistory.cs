#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CellHistory.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 
	/// </summary>
	public class CellHistory : ICellHistory {
		public virtual int Type { get; set; }
		public virtual string PseudoCode { get; set; }
		public virtual int RowId { get; set; }
		public virtual string Usr { get; set; }
		public virtual DateTime Time { get; set; }
		public virtual string Value { get; set; }
	}
}