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
// PROJECT ORIGIN: Zeta.Extreme.Core/StrongSumProvider.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Класс, отвечающий за простраивание и вывод структур дельт для
	/// 	строк, отвечает за обработку простых формул
	/// </summary>
	public class StrongSumProvider {
		/// <summary>
		/// 	Возвращает кэшированное значение проверки строки на 
		/// 	суммируемость или вычисляет и кэширует
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public bool IsSum(IZetaQueryDimension r) {
			IZetaQueryDimension native = null;
			if (r is ObjHandler) {
				native = ((ObjHandler) r).Native as IZetaMainObject;
			}
			if (r is RowHandler) {
				native = ((RowHandler) r).Native;
			}
			if (r is ColumnHandler) {
				native = ((ColumnHandler)r).Native;
			}
			r = native ?? r;
			if (null == r) {
				return false;
			}
			lock (r.LocalProperties) {
				if (r.LocalProperties.ContainsKey("_zvs_h_t")) {
					return true;
				}
				if (r.LocalProperties.ContainsKey("_zvs_h_f")) {
					return false;
				}
				var issum = EvalIsSum(r);
				if (issum) {
					r.LocalProperties["_zvs_h_t"] = true;
				}
				else {
					r.LocalProperties["_zvs_h_f"] = true;
				}
				return issum;
			}
		}


		/// <summary>
		/// </summary>
		/// <param name="item"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public bool EvalIsSum(IZetaQueryDimension item) {
			var row = item as IZetaRow;
			if (null != row) {
				if (row.IsMarkSeted("0SA")) {
					return true;
				}
			}
			var obj = item as IZetaMainObject;
			if (null != obj) {
				if (obj.IsFormula && (string.IsNullOrWhiteSpace(obj.FormulaType) || obj.FormulaType == "sum") &&
				    !string.IsNullOrWhiteSpace(obj.Formula)) {
					return true;
				}
			}
			if (item.IsFormula && item.FormulaType == "boo" && IsSumableFormula(item.Formula)) {
				return true;
			}

			return false;
		}


		/// <summary>
		/// 	Если формула состоит только из простых дельт и действий + и -
		/// 	то такая формула рассматривается как аналог суммы
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public bool IsSumableFormula(string formula) {
			return Regex.IsMatch(formula, FormulaParserConstants.PseudoSumPattern,
			                     RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		}

		/// <summary>
		/// 	Возвращает массив смещений для расчетов
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public QueryDelta[] GetSumDelta(IZetaQueryDimension r) {
			if (r.LocalProperties.ContainsKey("_zvs")) {
				return (QueryDelta[]) r.LocalProperties["_zvs"];
			}
			var result = CollectSumDelta(r).ToArray();
			r.LocalProperties["_zvs"] = result;
			return result;
		}

		/// <summary>
		/// 	Возвращает набор исходных производных запросов для суммирования
		/// </summary>
		/// <param name="anywithformula"> </param>
		/// <returns> </returns>
		public IEnumerable<QueryDelta> CollectSumDelta(IWithFormula anywithformula) {
			return GetFormulaSumDelta(anywithformula);
		}


		/// <summary>
		/// 	Возвращает набор исходных производных запросов для суммирования
		/// </summary>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IEnumerable<QueryDelta> CollectSumDelta(IZetaQueryDimension item) {
			if (!IsSum(item)) {
				yield break;
			}
			if (item is IZetaMainObject) {
				var ids = item.Formula.SmartSplit().Select(_ => _.ToInt()).ToArray();
				foreach (var id in ids) {
					yield return new QueryDelta {ObjId = id};
				}
			}
			else if (item is IZetaRow) {
				var row = item as IZetaRow;
				if (row.IsMarkSeted("0SA")) {
					if (string.IsNullOrWhiteSpace(((IZetaFormsSupport) row).GroupCache)) {
						foreach (var i in GetNativeSumDelta(row)) {
							yield return i;
						}
					}
					else {
						foreach (var i in GetGroupSumDelta(row)) {
							yield return i;
						}
					}
				}

				else {
					foreach (var i in GetFormulaSumDelta(row)) {
						yield return i;
					}
				}
			}
			else {
				foreach (var i in GetFormulaSumDelta(item)) {
					yield return i;
				}
			}
		}

		private IEnumerable<QueryDelta> GetFormulaSumDelta(IWithFormula row) {
			var matches = Regex.Matches(row.Formula, FormulaParserConstants.PseudoSumVector, RegexOptions.Compiled);
			return from Match match in matches select QueryDelta.CreateFromMatch(match);
		}

		private IEnumerable<QueryDelta> GetGroupSumDelta(IZetaRow row) {
			var groups = ((IZetaFormsSupport) row).GroupCache.SmartSplit(false, true, '/', ';').Distinct().ToArray();
			var pluses = groups.Where(_ => !_.StartsWith("-")).ToArray();
			var minuses = groups.Where(_ => _.StartsWith("-")).Select(_ => _.Substring(1)).ToArray();
			foreach (var p in pluses) {
				if (RowCache.Bygroup.ContainsKey(p)) {
					foreach (var r in RowCache.Bygroup[p]) {
						if (r.IsMarkSeted("0MINUS")) {
							yield return new QueryDelta {Row = r, Multiplicator = -1};
						}
						else {
							yield return new QueryDelta {Row = r};
						}
					}
				}
			}
			foreach (var m in minuses) {
				if (RowCache.Bygroup.ContainsKey(m)) {
					foreach (var r in RowCache.Bygroup[m]) {
						if (r.IsMarkSeted("0MINUS")) {
							yield return new QueryDelta {Row = r};
						}
						else {
							yield return new QueryDelta {Row = r, Multiplicator = -1};
						}
					}
				}
			}
		}

		private IEnumerable<QueryDelta> GetNativeSumDelta(IZetaRow row) {
			return 
				from c in row.Children 
				where !c.IsMarkSeted("0CAPTION") 
				where !c.IsMarkSeted("0NOSUM") 
				where IsSum(c) || c.IsFormula || 0 == c.Children.Count 
				select new QueryDelta {Row = c};
		}
	}
}