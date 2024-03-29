﻿#region LICENSE
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
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.SqlSupport;

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
			MoveAccounts(result);
			MoveConsobj(result);
			result.InvalidateCacheKey();
			return result;
		}

		private void MoveConsobj(IQuery query) {
			if (UseConsolidateObject) {

				var nzr = new NativeZetaReader();
				var filter = string.IsNullOrWhiteSpace(ConsolidateObjectFilter)
					             ? ""
					             : ("/" + ConsolidateObjectFilter.Replace(",", "/") + "/");
				var formula = "";
				if (filter.StartsWith("/_")) {
					formula = ConsolidateUpFromTerminalObj(query, filter.Substring(2,filter.Length-3), nzr);
				}
				else {

					formula = ConsolidateDownRootObj(query, filter, nzr);
				}
				query.Obj = new ObjHandler { Native = new Obj { IsFormula = true, Formula = formula } };
			}
		}

		private string ConsolidateUpFromTerminalObj(IQuery query,string filter, NativeZetaReader nzr) {
			var root = filter.Split('.')[0] == "_ROOT";
			var type = filter.Split('.')[1] == "TYPE";
			var current = query.Obj.Native as IZetaMainObject;
			if (null == current) return "-1";
			var obj = current;
			while (null!=obj.ParentId) {
				obj = MetaCache.Default.Get<IZetaMainObject>( obj.ParentId);
				if (!root) break;
			}
#pragma warning disable 612,618
			var newfilter = "/"+( type ? current.ObjType.Code : current.ObjType.Class.Code )+"/";
#pragma warning restore 612,618
			query.Obj = new ObjHandler{Native =  obj};
			return ConsolidateDownRootObj(query, newfilter, nzr);

		}

		private static string ConsolidateDownRootObj(IQuery query, string filter, NativeZetaReader nzr) {
			int[] ids = null;
			if (string.IsNullOrWhiteSpace(filter)) {
				ids = nzr.ReadObjectsWithTypes("o.Path like '%/" + query.Obj.Id + "/%'").Select(_ => _.Id).ToArray();
			}
			else {
				ids =
					nzr.ReadObjectsWithTypes(
						string.Format("Path like '%/{0}/%' and ('{1}' like '%/'+t.Code+'/%' or '{1}' like '%/'+c.Code+'/%')",
						              query.Obj.Id, filter)).Select(_ => _.Id).ToArray();
			}
			return string.Join(",", ids);
		}

		private void MoveContragent(IQuery result) {
			if (!string.IsNullOrWhiteSpace(Contragents)) {
				result.Reference = result.Reference.Copy();
				result.Reference.Contragents = Contragents;
			}
		}

		private void MoveAccounts(IQuery result)
		{
			if (!string.IsNullOrWhiteSpace(Types))
			{
				result.Reference = result.Reference.Copy();
				result.Reference.Types = Types;
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
			var t = match.Groups["t"].Value;
			var co = !string.IsNullOrWhiteSpace(match.Groups["co"].Value);
			var cov = match.Groups["cov"].Value;
			var aof = match.Groups["aof"].Value.ToStr(); //ZC-248 AltObjFilter
			var ys = match.Groups["ys"].Value != "-";
			if (!ys) {
				y = -y;
			}
			var p = match.Groups["p"].Value.ToInt();
			var ps = match.Groups["ps"].Value != "-";
		    var pds = match.Groups["pds"].Value;
		    int[] periods = null;
            if (!string.IsNullOrWhiteSpace(pds))
            {
                periods = pds.Split(',').Select(_ => _.ToInt()).ToArray();
            }
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

            if (null != periods)
            {
                delta.Periods = periods;
            }

			//ZC-248
			if (!string.IsNullOrWhiteSpace(aof)) {
				delta.Contragents = aof;
			}

			if (!string.IsNullOrWhiteSpace(t)) {
				delta.Types = t;
			}

			if (co) {
				delta.UseConsolidateObject = true;
				delta.ConsolidateObjectFilter = cov;
			}
			return delta;
		}
        /// <summary>
        /// Множественный перевод
        /// </summary>
	    public int[] Periods { get; set; }

	    /// <summary>
		/// Фильтр типов для консолидации объекта
		/// </summary>
		public string ConsolidateObjectFilter { get; set; }

		/// <summary>
		/// Признак использования консолидации
		/// </summary>
		public bool UseConsolidateObject { get; set; }

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
			if (!string.IsNullOrWhiteSpace(Types))
			{
				s.Append("Types = \"" + Types + "\", ");
			}
			if (UseConsolidateObject)
			{
				s.Append("UseConsolidateObject = true, ");
			}
			if (!string.IsNullOrWhiteSpace(ConsolidateObjectFilter))
			{
				s.Append("ConsolidateObjectFilter = \"" + ConsolidateObjectFilter + "\", ");
			}
			s.Append("}");
			if (!string.IsNullOrWhiteSpace(infunctionName)) {
				s.Append(")");
			}
			return s.ToString();
		}

		private void MoveTime(IQuery result) {
			if (
                    (Year != 0 && Year != result.Time.Year) 
                || 
                    (Period != 0 && Period != result.Time.Period)
                ||
                    (Periods!=null)
                
                ) {
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
						var eval = Model.MetaCaches.Periods.Eval(result.Time.Year, result.Time.Period, Period);

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
                if (null != Periods) {
                    result.Time.Period = 0;
                    result.Time.Periods = Periods;
                }
				result.Time.Normalize(result);
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
					var realcolcode = ColCode;
					if (ColCode.StartsWith("__"))
					{
						realcolcode = result.ResolveRealCode(realcolcode.Substring(2));
					}
					result.Col = new ColumnHandler { Code = realcolcode };
				}
			}
		}

		private void MoveRow(IQuery result) {
			if (null != Row) {
				if (Row != result.Row.Native) {
					result.Row = new RowHandler {Native = Row};
				}

			}
			else if (!string.IsNullOrWhiteSpace(RowCode))
			{
				if (RowCode != result.Row.Code)
				{
					var realrowcode = RowCode;
					if (RowCode.StartsWith("__"))
					{
						realrowcode = result.ResolveRealCode(RowCode.Substring(2));
					}
					result.Row = new RowHandler { Code = realrowcode };
				}
			}
			
		}
		/// <summary>
		/// Режим смены объекта до корневого
		/// </summary>
		public const int TO_ROOT_OBJ_MODE = -1;
		/// <summary>
		/// Режим смены объекта до родительского
		/// </summary>
		public const int TO_PARENT_OBJ_MODE = -2;

		private void MoveObj(IQuery result) {
			if (HasObjDelta(result)) {
				if (null != Obj) {
					if (!Equals(Obj, result.Obj.Native)) {
						result.Obj = new ObjHandler {Native = Obj};
					}
				}
				else if (0 != ObjId) {
					if (ObjId == TO_ROOT_OBJ_MODE ||ObjId== TO_PARENT_OBJ_MODE) {
						var mc = MetaCache.Default;
						if (null != result.Session) {
							mc = result.Session.GetMetaCache();
						}
						if (null != result.Obj.Native) {
							var current = (Obj)result.Obj.Native;
							while (current.ParentId.HasValue) {
								current = mc.Get<Obj>(current.ParentId.Value);
								if (ObjId == TO_PARENT_OBJ_MODE) break;
							}
							if (current.Id != result.Obj.Id) {
								result.Obj = new ObjHandler {Native = current};
							}
						}
						else {
							throw new Exception("cannot apply root object to null");
						}
					}
					
					else if (ObjId != result.Obj.Id) {
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
            if (null != Periods)
            {
                return false;
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

			if (!string.IsNullOrWhiteSpace(Types) && target.Reference.Types != Types) {
				return false;
			}
			if (UseConsolidateObject) {
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

		/// <summary>
		///  Поддержка счетов
		/// </summary>
		public string Types;
	}
}