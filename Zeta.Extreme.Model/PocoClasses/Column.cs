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

using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// </summary>
	public partial class Column : Entity, IZetaColumn {
		/// <summary>
		///     Temporary (local) properties collection
		/// </summary>
		public virtual IDictionary<string, object> LocalProperties {
			get { return _localProperties ?? (_localProperties = new Dictionary<string, object>()); }
			set { _localProperties = value; }
		}

		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual int Year { get; set; }

		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual int Period { get; set; }

		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		public virtual string ForeignCode { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Column.Currency" /> of entity
		/// </summary>
		public virtual string Currency { get; set; }


		/// <summary>
		///     Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		///     Formula's activity flag
		/// </summary>
		public virtual bool IsFormula { get; set; }

		/// <summary>
		///     Formula's definition
		/// </summary>
		public virtual string Formula { get; set; }


		/// <summary>
		///     Type of measure <c>(ru : единица измерения)</c>
		/// </summary>
		public virtual string Measure { get; set; }

		/// <summary>
		///     Flag that measure must be setted up dynamically
		/// </summary>
		public virtual bool IsDynamicMeasure { get; set; }

		/// <summary>
		///     Slash-delimited list of mark codes
		/// </summary>
		public virtual string MarkCache { get; set; }

		/// <summary>
		///     Type of native data under column
		/// </summary>
		public virtual ValueDataType DataType { get; set; }


		private IDictionary<string, object> _localProperties;
	}
}