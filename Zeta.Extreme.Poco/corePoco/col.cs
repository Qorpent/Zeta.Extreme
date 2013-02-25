// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
	public partial class col : IZetaColumn, IZetaQueryDimension {
        private IList<IZetaColumnMark> _markLinks;
	    private IDictionary<string, object> localProperties;

	    [Map]
        public virtual Guid Uid { get; set; }
		public virtual IDictionary<string, object> LocalProperties
		{
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		/// <summary>
		/// Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap]
		public virtual int Year { get; set; }

		/// <summary>
		/// Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap]
		public virtual int Period { get; set; }

		/// <summary>
		/// Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[NoMap] public virtual string ForeignCode { get; set; }

		#region IZetaColumn Members
        [Map]
        public virtual string Tag { get; set; }

        [Map]
        public virtual string Lookup { get; set; }

		[Map]
        public virtual string Valuta { get; set; }

        [Map("IsDinamycLookUp")]
        public virtual bool IsDynamicLookup { get; set; }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual string Name { get; set; }

        [Map]
        public virtual string Code { get; set; }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        [Map]
        public virtual bool IsFormula { get; set; }

        [Map]
        public virtual string Formula { get; set; }

        [Map]
        public virtual string ParsedFormula { get; set; }

        [Map]
        public virtual string FormulaEvaluator { get; set; }

        [Map]
        public virtual string Measure { get; set; }

        [Map]
        public virtual bool IsDynamicMeasure { get; set; }
        [Map]
        public virtual string MarkCache { get; set; }

        public virtual ValueDataType DataType { get; set; }

        
        public virtual string DataTypeString {
            get { return DataType.ToString(); }
            set { DataType = (ValueDataType) Enum.Parse(typeof (ValueDataType), value, true); }
        }

        [Map]
        public virtual string DataTypeDetail { get; set; }


        [Many(ClassName = typeof (ColumnMark))]
        public virtual IList<IZetaColumnMark> MarkLinks{
            get { return _markLinks; }
            set { _markLinks = value; }
        }

        [Many(ClassName = typeof (cell))]
        public virtual IList<IZetaCell> Cells { get; set; }

        [Many(ClassName = typeof (fixrule))]
        public virtual IList<IFixRule> FixRules { get; set; }

        #endregion
    }
}