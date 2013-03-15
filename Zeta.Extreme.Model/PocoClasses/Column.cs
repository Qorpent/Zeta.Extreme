#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : col.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	/// <summary>
	/// 
	/// </summary>
	public partial class Column : IZetaColumn {
		 public virtual Guid Uid { get; set; }

		public virtual string DataTypeString {
			get { return DataType.ToString(); }
			set { DataType = (ValueDataType) Enum.Parse(typeof (ValueDataType), value, true); }
		}


		public virtual IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual int Year { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual int Period { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual string ForeignCode { get; set; }

		 public virtual string Tag { get; set; }

		 public virtual string Lookup { get; set; }

		 public virtual string Valuta { get; set; }

		 public virtual bool IsDynamicLookup { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		 public virtual bool IsFormula { get; set; }

		 public virtual string Formula { get; set; }

		

		 public virtual string Measure { get; set; }

		 public virtual bool IsDynamicMeasure { get; set; }
		 public virtual string MarkCache { get; set; }

		public virtual ValueDataType DataType { get; set; }


		 public virtual string DataTypeDetail { get; set; }


		

		

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }

		private IDictionary<string, object> localProperties;

		public virtual string GetStaticMeasure(string format) {
			if (IsDynamicMeasure) {
				return "";
			}
			if (Measure.IsNotEmpty()) {
				if (format.IsNotEmpty()) {
					return string.Format(format, Measure);
				}
				return Measure;
			}
			return "";
		}

		public virtual string GetDynamicMeasure(IZetaRow source, string format) {
			if (!IsDynamicMeasure) {
				return "";
			}
			if (source.Measure.IsNotEmpty()) {
				if (format.IsNotEmpty()) {
					return string.Format(format, source.Measure);
				}
				return source.Measure;
			}
			return GetStaticMeasure(format);
		}

		public virtual bool IsMarkSeted(string code) {
			return WithMarksExtension.IsMarkSeted(this, code);
		}
	}
}