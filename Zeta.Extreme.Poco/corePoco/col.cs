#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : col.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class col : IZetaColumn, IZetaQueryDimension {
		[Map] public virtual Guid Uid { get; set; }

		public virtual string DataTypeString {
			get { return DataType.ToString(); }
			set { DataType = (ValueDataType) Enum.Parse(typeof (ValueDataType), value, true); }
		}

		[Many(ClassName = typeof (ColumnMark))] public virtual IList<IZetaColumnMark> MarkLinks { get; set; }

		public virtual IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap] public virtual int Year { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap] public virtual int Period { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap] public virtual string ForeignCode { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map] public virtual string Lookup { get; set; }

		[Map] public virtual string Valuta { get; set; }

		[Map("IsDinamycLookUp")] public virtual bool IsDynamicLookup { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		[Map] public virtual bool IsFormula { get; set; }

		[Map] public virtual string Formula { get; set; }

		[Map] public virtual string ParsedFormula { get; set; }

		[Map] public virtual string FormulaEvaluator { get; set; }

		[Map] public virtual string Measure { get; set; }

		[Map] public virtual bool IsDynamicMeasure { get; set; }
		[Map] public virtual string MarkCache { get; set; }

		public virtual ValueDataType DataType { get; set; }


		[Map] public virtual string DataTypeDetail { get; set; }


		[Many(ClassName = typeof (cell))] public virtual IList<IZetaCell> Cells { get; set; }

		[Many(ClassName = typeof (fixrule))] public virtual IList<IFixRule> FixRules { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }

		private IDictionary<string, object> localProperties;
	}
}