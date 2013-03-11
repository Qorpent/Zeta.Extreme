#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : MetalinkRecord.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent;

namespace Zeta.Extreme.Model.PocoClasses {
	public class MetalinkRecord {
		public MetalinkRecord() {
			Active = true;
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
		}

		public int Id { get; set; }
		public string Code { get; set; }
		public string SrcType { get; set; }
		public string TrgType { get; set; }
		public string Src { get; set; }
		public string Trg { get; set; }
		public string Type { get; set; }
		public string SubType { get; set; }
		public string Tag { get; set; }
		public bool Active { get; set; }
		public DateTime Start { get; set; }
		public DateTime Finish { get; set; }
	}
}