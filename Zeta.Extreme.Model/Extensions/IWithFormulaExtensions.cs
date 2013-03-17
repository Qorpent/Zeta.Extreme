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
// PROJECT ORIGIN: Zeta.Extreme.Model/IWithFormulaExtensions.cs
#endregion
using Qorpent.Model;

#if NEWMODEL
using Comdiv.Extensions;
using Comdiv.Olap.Model;

#endif

namespace Zeta.Extreme.Model.Extensions {
	/// <summary>
	/// 	¬спомогательный класс дл€ работы с формулами
	/// </summary>
	public static class IWithFormulaExtensions {
		/// <summary>
		/// 	Null-safe formula
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static string Formula(this IWithFormula formula) {
			if (null == formula) {
				return null;
			}
			return formula.Formula;
		}

		/// <summary>
		/// 	null-safe formula type
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static string FormulaType(this IWithFormula formula) {
			if (null == formula) {
				return null;
			}
			return formula.FormulaType;
		}

		

		/// <summary>
		/// 	null-safe is formula
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static bool IsFormula(this IWithFormula formula) {
			if (null == formula) {
				return false;
			}
			return formula.IsFormula;
		}
	}
}