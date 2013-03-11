#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : cell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class cell : IZetaCell {
		[Ref(ClassName = typeof (IZetaRow))] public virtual IZetaRow MainDataTree { get; set; }


		[Ref(ClassName = typeof (col))] public virtual IZetaColumn ValueType { get; set; }

		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Ref(ClassName = typeof (detail))] public virtual IZetaDetailObject Subpart { get; set; }

		public virtual string Path { get; set; }
		[Map] public virtual Guid Uid { get; set; }

		[Map] public virtual string Valuta { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual string Data { get; set; }

		[Map] public virtual bool Finished { get; set; }

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

		[Map] public virtual int Id { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		[Nest] public virtual StandardRowData RowData {
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

		[Map] public virtual int Year { get; set; }

		[Map("Kvart")] public virtual int Period { get; set; }

		[Map] public virtual DateTime DirectDate { get; set; }

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

		public virtual string Tag { get; set; }


		[Map] public virtual bool IsAuto { get; set; }
		[Map] public virtual string Usr { get; set; }
		private StandardRowData rowData;

		public override string ToString() {
			return string.Format("cell:{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}",
			                     Row.Code, Column.Code, Object.Code, null == DetailObject ? "" : DetailObject.Code, Year, Period,
			                     DirectDate.ToString("yyyy-MM-dd"), Value
				);
		}
	}
}