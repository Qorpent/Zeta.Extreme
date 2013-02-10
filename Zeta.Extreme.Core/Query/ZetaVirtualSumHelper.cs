#region LICENSE

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
		/// 	Регулярное выражение оценки и разбора суммовых формул
		/// </summary>
		public const string SummableFormulaRegex =
			@"^\s*(-?\s*\$[\w\d]+(@[\w\d]+)?(\.Y-?\d+)?(\.P-?\d+)?\?)(((\s*[+-]\s*)|\s+)\$[\w\d]+(@[\w\d]+)?(\.Y-?\d+)?(\.P-?\d+)?\?)*\s*$";

		/// <summary>
		/// 	Регулярное выражение для выборки отдельного элемента формулы
		/// </summary>
		public const string FormulaItemSplitRegex =
			@"(?<s>[-+])?\s*\$(?<r>[\w\d]+)(@(?<c>[\w\d]+))?(\.Y(?<ys>-)?(?<y>\d+))?(\.P(?<ps>-)?(?<p>\d+))?\?";

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
			return Regex.IsMatch(formula, SummableFormulaRegex, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
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
			var matches = Regex.Matches(row.Formula, FormulaItemSplitRegex, RegexOptions.Compiled);
			foreach (Match match in matches) {
				var delta = new ZexQueryDelta();
				var s = match.Groups["s"].Value != "-";
				var r = match.Groups["r"].Value;
				var c = match.Groups["c"].Value;
				var y = match.Groups["y"].Value.toInt();
				var ys = match.Groups["ys"].Value != "-";
				if (!ys) {
					y = -y;
				}
				var p = match.Groups["p"].Value.toInt();
				var ps = match.Groups["ps"].Value != "-";
				if (!ps) {
					p = -p;
				}
				if (!s) {
					delta.Multiplicator = -1;
				}
				if (!string.IsNullOrWhiteSpace(r)) {
					var _r = RowCache.get(r);
					if (null != _r) {
						delta.Row = _r;
					}
					else {
						delta.RowCode = r;
					}
				}
				if (!string.IsNullOrWhiteSpace(c)) {
					var _c = ColumnCache.get(c);
					if (null != _c) {
						delta.Column = _c;
					}
					delta.ColumCode = c;
				}
				if (0 != y) {
					delta.Year = y;
				}
				if (0 != p) {
					delta.Period = p;
				}
				yield return delta;
			}
		}

		private IEnumerable<ZexQueryDelta> GetGroupSumDelta(IZetaRow row) {
			yield break;
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