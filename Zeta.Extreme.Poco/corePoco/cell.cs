#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : cell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class cell : IZetaCell {
		[Deprecated.Ref(ClassName = typeof (IZetaRow))] public virtual IZetaRow MainDataTree { get; set; }


		[Deprecated.Ref(ClassName = typeof (col))] public virtual IZetaColumn ValueType { get; set; }

		[Deprecated.Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Deprecated.Ref(ClassName = typeof (detail))] public virtual IZetaDetailObject Subpart { get; set; }

		public virtual string Path { get; set; }
		public virtual FixRuleResult? FixStatus { get; set; }

		[Deprecated.Map] public virtual string Valuta { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual string Data { get; set; }

		[Deprecated.Map] public virtual bool Finished { get; set; }

		//    [Many(ClassName = typeof (DataVersion))]
		//   public virtual IList<IDataVersion> Versions { get; set; }

		//    [Many(ClassName = typeof (Correction))]
		// public virtual IList<IAcceptedDataCorrection> Corrections { get; set; }

		//public virtual IDataVersion DataVersion{
		//    get { return Ver; }
		//    set { Ver = value; }
		//}

		//   [Ref(ClassName = typeof (Scenario))]
		// public virtual IScenario Scenario { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		[Deprecated.Nest] public virtual StandardRowData RowData {
			get { return rowData ?? (rowData = new StandardRowData()); }
			set { rowData = value; }
		}

		public virtual object Value {
			get { return RowData.GetValue(Column); }
			set { RowData.SetValue(Column, value); }
		}

		public virtual IZetaRow Row {
			get { return MainDataTree; }
			set { MainDataTree = value; }
		}

		[Deprecated.Map] public virtual int Year { get; set; }

		[Deprecated.Map("Kvart")] public virtual int Period { get; set; }

		[Deprecated.Map] public virtual DateTime DirectDate { get; set; }

		public virtual IZetaColumn Column {
			get { return ValueType; }
			set { ValueType = value; }
		}

		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		public virtual IZetaMainObject AltObj { get; set; }
		public virtual int AltObjId { get; set; }
		public virtual int RowId { get; set; }
		public virtual int ColumnId { get; set; }
		public virtual int ObjectId { get; set; }
		public virtual int DetailId { get; set; }


		public virtual IZetaDetailObject DetailObject {
			get { return Subpart; }
			set { Subpart = value; }
		}

		public virtual object Tag { get; set; }

		public virtual IPkg Pkg { get; set; }
		[Deprecated.Map] public virtual bool IsAuto { get; set; }
		[Deprecated.Map] public virtual string Usr { get; set; }
		private StandardRowData rowData;
	}
}