﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQueryDelta.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описывает потенциальный подзапрос для оптимизации расчета сумм и простых формул
	/// </summary>
	public sealed class QueryDelta {
		/// <summary>
		/// 	Применяет смещение к целевому запросу
		/// 	Если есть изменения - то правильно создает копии и переписывает кэш-строку
		/// </summary>
		/// <param name="target"> </param>
		/// <returns> </returns>
		public Query Apply(Query target) {
			lock (this) {
				if (NoChanges(target)) {
					return target;
				}
				var result = target.Copy();
				MoveColumn(result);
				MoveRow(result);
				MoveObj(result);
				MoveTime(result);
				result.InvalidateCacheKey();
				return result;
			}
		}

		/// <summary>
		/// 	Создает дельту из результатов регекса со стандартными именами групп
		/// </summary>
		/// <param name="match"> </param>
		/// <returns> </returns>
		public static QueryDelta CreateFromMatch(Match match) {
			var delta = new QueryDelta();
			var s = match.Groups["s"].Value != "-";
			var r = match.Groups["r"].Value;
			var c = match.Groups["c"].Value;
			var y = match.Groups["y"].Value.ToInt();
			var ys = match.Groups["ys"].Value != "-";
			if (!ys) {
				y = -y;
			}
			var p = match.Groups["p"].Value.ToInt();
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
					delta.Col = _c;
				}
				delta.ColCode = c;
			}
			if (0 != y) {
				delta.Year = y;
			}
			if (0 != p) {
				delta.Period = p;
			}
			return delta;
		}

		/// <summary>
		/// 	Конвертирует дельту в C# - конструктор для генерации формул
		/// </summary>
		/// <param name="infunctionName"> Опциональное имя метода в который надо обернуть конструктор </param>
		/// <returns> </returns>
		public string ToCSharpString(string infunctionName = "") {
			var s = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(infunctionName)) {
				s.Append(infunctionName);
				s.Append("(");
			}
			s.Append(" new " + GetType().FullName + "{ ");
			var rcode = RowCode;
			if (null != Row) {
				rcode = Row.Code;
			}
			if (!string.IsNullOrWhiteSpace(rcode)) {
				s.Append("RowCode = \"" + rcode + "\", ");
			}
			var ccode = ColCode;
			if (null != Col) {
				ccode = Col.Code;
			}
			if (!string.IsNullOrWhiteSpace(ccode)) {
				s.Append("ColCode = \"" + ccode + "\", ");
			}

			var objid = ObjId;
			if (null != Obj) {
				objid = Obj.Id;
			}
			if (0 != objid) {
				s.Append("ObjId = " + objid + ", ");
			}
			if (Year != 0) {
				s.Append("Year = " + Year + ", ");
			}
			if (Period != 0) {
				s.Append("Period = " + Period + ", ");
			}
			s.Append("}");
			if (!string.IsNullOrWhiteSpace(infunctionName)) {
				s.Append(")");
			}
			return s.ToString();
		}

		private void MoveTime(Query result) {
			if ((Year != 0 && Year != result.Time.Year) || (Period != 0 && Period != result.Time.Period)) {
				result.Time = result.Time.Copy();
				if (0 != Year && Year < 1900) {
					result.Time.Year += Year;
				}
				else if (Year != result.Time.Year) {
					result.Time.Year = Year;
				}
				if (0 != Period) {
					if (0 < Period) {
						result.Time.Period = Period;
					}
					else {
						var eval = Periods.Eval(result.Time.Year, result.Time.Period, Period);
						result.Time.Year = eval.Year;
						result.Time.Period = eval.Periods[0];
					}
				}
			}
		}

		private void MoveColumn(Query result) {
			if (null != Col) {
				if (Col != result.Col.Native) {
					result.Col = new ColumnHandler {Native = Col};
				}
			}
			else if (!string.IsNullOrWhiteSpace(ColCode)) {
				if (ColCode != result.Col.Code) {
					result.Col = result.Col.Copy();
					if (null != result.Col.Native) {
						result.Col.Native = null;
					}
					result.Col.Code = ColCode;
				}
			}
		}

		private void MoveRow(Query result) {
			if (null != Row) {
				if (Row != result.Row.Native) {
					result.Row = new RowHandler {Native = Row};
				}
			}
			else if (!string.IsNullOrWhiteSpace(RowCode)) {
				if (RowCode != result.Row.Code) {
					result.Row = result.Row.Copy();
					if (null != result.Row.Native) {
						result.Row.Native = null;
					}
					result.Row.Code = RowCode;
				}
			}
		}

		private void MoveObj(Query result) {
			if (null != Obj) {
				if (Obj != result.Obj.Native) {
					result.Obj = new ObjHandler {Native = Obj};
				}
			}
			else if (0 != ObjId) {
				if (ObjId != result.Obj.Id) {
					result.Obj = result.Obj.Copy();
					if (null != result.Obj.Native) {
						result.Obj.Native = null;
					}
					result.Obj.Id = ObjId;
				}
			}
		}


		private bool NoChanges(Query target) {
			if (0 > Period) {
				return false; //формульный период
			}
			if (0 != Year && Math.Abs(Year) < 1900) {
				return false; //смещение года
			}
			if (null != Col && Col != target.Col.Native) {
				return false;
			}
			if (null != Row && Row != target.Row.Native) {
				return false;
			}
			if (null != Obj && Obj != target.Obj.Native) {
				return false;
			}
			if (!string.IsNullOrWhiteSpace(ColCode) && ColCode != target.Col.Code) {
				return false;
			}
			if (!string.IsNullOrWhiteSpace(RowCode) && RowCode != target.Row.Code) {
				return false;
			}
			if (0 != ObjId && ObjId != target.Obj.Id) {
				return false;
			}
			if (0 != Period && Period != target.Time.Period) {
				return false;
			}
			if (0 != Year && Year != target.Time.Year) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Смещение по колонке (прямое) или null
		/// </summary>
		public IZetaColumn Col;

		/// <summary>
		/// 	Смещение по колонке (код) или пустое значение для отсутствия смещения
		/// </summary>
		public string ColCode;

		/// <summary>
		/// 	Множитель при расчете значений
		/// </summary>
		public decimal Multiplicator = 1;

		/// <summary>
		/// 	Прямое смещение по объекту
		/// </summary>
		public IZetaMainObject Obj;

		/// <summary>
		/// 	Смещение по объекту
		/// </summary>
		public int ObjId;

		/// <summary>
		/// 	Прямое или дельтированное указание периода
		/// </summary>
		public int Period;

		/// <summary>
		/// 	Прямое укзание смещения строки или отсутствие
		/// </summary>
		public IZetaRow Row;

		/// <summary>
		/// 	Смещение по строке или пустое значение для отсутствия смещения
		/// </summary>
		public string RowCode;

		/// <summary>
		/// 	Прямое или дельтированое указание года
		/// </summary>
		public int Year;
	}
}