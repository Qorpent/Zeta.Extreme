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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaColumn.cs

#endregion

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	///     Zeta column part of attributes dimension
	/// </summary>
	public interface IZetaColumn : IZetaQueryDimension,
	                               IWithMarkCache, IWithMeasure, IWithCurrency {
		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		int Year { get; set; }

		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		int Period { get; set; }

		/// <summary>
		///     Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		string ForeignCode { get; set; }

		/// <summary>
		///     Type of native data under column
		/// </summary>
		ValueDataType DataType { get; set; }
	}
}