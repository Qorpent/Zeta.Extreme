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

using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model{
    [global::Zeta.Extreme.Poco.Deprecated.Classic("ValueType")]
    [global::Zeta.Extreme.Poco.Deprecated.ForSearch("Колонка, показатель")]
    public interface IZetaColumn :
        IOlapColumn,
		IZetaQueryDimension,
        IWithMarks<IZetaColumn, IZetaColumnMark>,
        IWithMarkCache,
        IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
        IWithFixRules{
        string Valuta { get; set; }
	    IDictionary<string, object> LocalProperties { get; set; }
	    string GetStaticMeasure(string format);
        string GetDynamicMeasure(IZetaRow source, string format);
		/// <summary>
		/// Поддержка режима "колонка как заместитель колсета"
		/// </summary>
	    int Year { get; set; }
		/// <summary>
		/// Поддержка режима "колонка как заместитель колсета"
		/// </summary>
	    int Period { get; set; }

	    /// <summary>
	    /// Поддержка режима "колонка как заместитель колсета"
	    /// </summary>
	    [NoMap] string ForeignCode { get; set; }
        }
		
}