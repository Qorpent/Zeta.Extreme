#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/Column.cs
#endregion
using System;
using System.Collections.Generic;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
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

		/// <summary>
		///Currency of entity
		/// </summary>
		public virtual string Currency { get; set; }

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