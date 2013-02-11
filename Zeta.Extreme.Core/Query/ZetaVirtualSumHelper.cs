﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZetaVirtualSumHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Comdiv.Extensions;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Класс, отвечающий за простраивание и вывод структур дельт для
	/// 	строк, отвечает за обработку простых формул
	/// </summary>
	public class ZetaVirtualSumHelper {
		/// <summary>
		/// 	Возвращает кэшированное значение проверки строки на 
		/// 	суммируемость или вычисляет и кэширует
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public bool IsSum(IZetaRow r) {
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
		/// <param name="zetaRow"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public bool EvalIsSum(IZetaRow zetaRow) {
			if (zetaRow.IsMarkSeted("0SA")) {
				return true;
			}
			if (zetaRow.IsFormula && zetaRow.FormulaEvaluator == "boo" && IsSumableFormula(zetaRow.Formula)) {
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
		public ZexQueryDelta[] GetSumDelta(IZetaRow r) {
			if (r.LocalProperties.ContainsKey("_zvs")) {
				return (ZexQueryDelta[]) r.LocalProperties["_zvs"];
			}
			var result = CollectSumDelta(r).ToArray();
			r.LocalProperties["_zvs"] = result;
			return result;
		}

		/// <summary>
		/// 	Возвращает набор исходных производных запросов для суммирования
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public IEnumerable<ZexQueryDelta> CollectSumDelta(IZetaRow row) {
			if (!IsSum(row)) {
				yield break;
			}
			if (row.IsMarkSeted("0SA")) {
				if (string.IsNullOrWhiteSpace(row.Group)) {
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

		private IEnumerable<ZexQueryDelta> GetFormulaSumDelta(IZetaRow row) {
			var matches = Regex.Matches(row.Formula, FormulaParserConstants.PseudoSumVector, RegexOptions.Compiled);
			return from Match match in matches select ZexQueryDelta.CreateFromMatch(match);
		}

		private IEnumerable<ZexQueryDelta> GetGroupSumDelta(IZetaRow row) {
			var groups = row.Group.split(false, true, '/', ';').Distinct();
			var pluses = groups.Where(_ => !_.StartsWith("-")).ToArray();
			var minuses = groups.Where(_ => _.StartsWith("-")).Select(_ => _.Substring(1)).ToArray();
			foreach (var p in pluses) {
				if (RowCache.bygroup.ContainsKey(p)) {
					foreach (var r in RowCache.bygroup[p]) {
						if (r.IsMarkSeted("0MINUS")) {
							yield return new ZexQueryDelta {Row = r, Multiplicator = -1};
						}
						else {
							yield return new ZexQueryDelta {Row = r};
						}
					}
				}
			}
			foreach (var m in minuses) {
				if (RowCache.bygroup.ContainsKey(m)) {
					foreach (var r in RowCache.bygroup[m]) {
						if (r.IsMarkSeted("0MINUS")) {
							yield return new ZexQueryDelta {Row = r};
						}
						else {
							yield return new ZexQueryDelta {Row = r, Multiplicator = -1};
						}
					}
				}
			}
		}

		private IEnumerable<ZexQueryDelta> GetNativeSumDelta(IZetaRow row) {
			foreach (var c in row.Children) {
				if (c.IsMarkSeted("0CAPTION")) {
					continue;
				}
				if (c.IsMarkSeted("0NOSUM")) {
					continue;
				}
				if (IsSum(c)) {
					yield return new ZexQueryDelta {Row = c};
				}
				if (c.IsFormula) {
					continue;
				}
				if (c.Children.Count != 0) {
					continue;
				}
				yield return new ZexQueryDelta {Row = c};
			}
		}
	}
}