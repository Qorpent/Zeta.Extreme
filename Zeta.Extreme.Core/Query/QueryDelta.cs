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
// PROJECT ORIGIN: Zeta.Extreme.Core/QueryDelta.cs
#endregion
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

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
		public IQuery Apply(IQuery target) {
			if (NoChanges(target)) {
				return target;
			}
			var result = target.Copy();
			MoveColumn(result);
			MoveRow(result);
			MoveObj(result);
			MoveTime(result);
			MoveContragent(result);
			result.InvalidateCacheKey();
			return result;
		}

		private void MoveContragent(IQuery result) {
			if (!string.IsNullOrWhiteSpace(Contragents)) {
				result.Reference = result.Reference.Copy();
				result.Reference.Contragents = Contragents;
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
			var o = match.Groups["o"].Value.ToInt();
			var y = match.Groups["y"].Value.ToInt();
			var aof = match.Groups["aof"].Value.ToStr(); //ZC-248 AltObjFilter
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
			if (!string.IsNullOrWhiteSpace(r) && "_" != r) {
				// оставляем писать возможность формулы в синтаксисе типа $_.toobj(...)
				var zetaRow = RowCache.get(r);
				if (null != zetaRow) {
					delta.Row = zetaRow;
				}
				else {
					delta.RowCode = r;
				}
			}
			if (0 != o) {
				delta.ObjId = o;
			}
			if (!string.IsNullOrWhiteSpace(c)) {
				var zetaColumn = ColumnCache.get(c);
				if (null != zetaColumn) {
					delta.Col = zetaColumn;
				}
				delta.ColCode = c;
			}
			if (0 != y) {
				delta.Year = y;
			}
			if (0 != p) {
				delta.Period = p;
			}

			//ZC-248
			if (!string.IsNullOrWhiteSpace(aof)) {
				delta.Contragents = aof;
			}
			return delta;
		}

		/// <summary>
		/// Фильтр по контрагенту
		/// </summary>
		/// <remarks>Совместимая реализация по ZC-248</remarks>
		public string Contragents { get; set; }

		/// <summary>
		/// 	Конвертирует дельту в C# - конструктор для генерации формул
		/// </summary>
		/// <param name="shortName"> </param>
		/// <param name="infunctionName"> Опциональное имя метода в который надо обернуть конструктор </param>
		/// <returns> </returns>
		public string ToCSharpString(bool shortName = true, string infunctionName = "") {
			var s = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(infunctionName)) {
				s.Append(infunctionName);
				s.Append("(");
			}
			s.Append(" new " + (shortName ? GetType().Name : GetType().FullName) + "{ ");
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
			if (!string.IsNullOrWhiteSpace(Contragents)) {
				s.Append("Contragents = \"" + Contragents + "\", ");
			}
			s.Append("}");
			if (!string.IsNullOrWhiteSpace(infunctionName)) {
				s.Append(")");
			}
			return s.ToString();
		}

		private void MoveTime(IQuery result) {
			if ((Year != 0 && Year != result.Time.Year) || (Period != 0 && Period != result.Time.Period)) {
				result.Time = result.Time.Copy();
				if (0 != Year && Year < 1900) {
					result.Time.Year += Year;
				}
				else if (0 != Year && Year != result.Time.Year) {
					result.Time.Year = Year;
				}
				if (0 != Period) {
					if (0 < Period) {
						result.Time.Period = Period;
					}
					else {
						var eval = Periods.Eval(result.Time.Year, result.Time.Period, Period);

						result.Time.Year = eval.Year;
						if (eval.Periods.Length == 1) {
							result.Time.Period = eval.Periods[0];
						}
						else {
							result.Time.Periods = eval.Periods.OrderBy(_ => _).ToArray();
							result.Time.Period = 0;
						}
					}
				}
				result.Time.Normalize(result.Session);
			}
		}

		private void MoveColumn(IQuery result) {
			if (null != Col) {
				if (Col != result.Col.Native) {
					result.Col = new ColumnHandler {Native = Col};
				}
			}
			else if (!string.IsNullOrWhiteSpace(ColCode)) {
				if (ColCode != result.Col.Code) {
					result.Col = new ColumnHandler {Code = ColCode};
				}
			}
		}

		private void MoveRow(IQuery result) {
			if (null != Row) {
				if (Row != result.Row.Native) {
					result.Row = new RowHandler {Native = Row};
				}
			}
			else if (!string.IsNullOrWhiteSpace(RowCode)) {
				if (RowCode != result.Row.Code) {
					result.Row = new RowHandler {Code = RowCode};
				}
			}
		}

		private void MoveObj(IQuery result) {
			if (HasObjDelta(result)) {
				if (null != Obj) {
					if (!Equals(Obj, result.Obj.Native)) {
						result.Obj = new ObjHandler {Native = Obj};
					}
				}
				else if (0 != ObjId) {
					if (ObjId != result.Obj.Id) {
						result.Obj = new ObjHandler { Id =ObjId};
					}
				}
			}

		}

		private bool HasObjDelta(IQuery result) {
			if (null != Obj) {
				if (null == result.Obj) return true;
				if (result.Obj.Native != Obj) return true;
			}
			if (0 != ObjId) {
				if (null == result.Obj) return true;
				if (ObjId != result.Obj.Id) return true;
			}
			return false;
		}


		private bool NoChanges(IQuery target) {
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

			if (HasObjDelta(target)) return false;

			if (!string.IsNullOrWhiteSpace(Contragents) && target.Reference.Contragents != Contragents) return false;
			
			if (!string.IsNullOrWhiteSpace(ColCode) && ColCode != target.Col.Code) {
				return false;
			}
			if (!string.IsNullOrWhiteSpace(RowCode) && RowCode != target.Row.Code) {
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