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
// PROJECT ORIGIN: Zeta.Extreme.Core/SimpleObjectDataScriptGenerator.cs

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Primary {
	/// <summary>
	///     Класс генерации первичных скриптов для классической реализации БД Zeta (2 версия)
	/// </summary>
	public sealed class Zeta2SqlScriptGenerator : IScriptGenerator {
		/// <summary>
		///     Строит SQL запрос с учетом прототипа, а по сути "хинтов" запроса
		/// </summary>
		/// <param name="queries"></param>
		/// <param name="prototype"></param>
		/// <returns>
		/// </returns>
		public string Generate(IQuery[] queries, PrimaryQueryPrototype prototype) {
			var script = "";
			if (0 != queries.Length) {
				var times = GetTimedGroup(queries);
				var colobj = GetColumnAndObjectGroups(queries).ToArray();
				var objtypes = GetObjTypeIdsGroup(queries).ToArray();
				string colids = null;
				var rowids = string.Join(",", queries.Select(_ => _.Row.Id).Distinct());
				colids = string.Join(",", queries.Select(_ => _.Col.Id).Distinct());
				if (objtypes.Length < colids.Length) {
					//ineffective plan
					script = times.Aggregate(
						script,
						(_, time) => objtypes.Aggregate(
							_, (current, objtype) =>
							   current + GenerateQuery(time, objtype, rowids, colids, prototype)
							             )
						);
				}
				else {
					script = times.Aggregate(
					script,
					(_, time) => colobj.Aggregate(
						_, (current, cobj) =>
						   current + GenerateQuery(time, cobj, rowids,colids, prototype)));	
				}
				
			}
			return script;
		}


		private static IEnumerable<ObjColQueryGeneratorStruct> GetObjTypeIdsGroup(IEnumerable<IQuery> queries)
		{
			var objtypes =
				queries.GroupBy(_ => _.Obj.Type+"_"+_.Obj.DetailMode, _ => _).Select(
					_ =>
					new ObjColQueryGeneratorStruct
					{
						t = _.First().Obj.Type,
						m = _.First().Obj.DetailMode,
						ids = string.Join(",",_.Select(__=>__.Obj.Id).Distinct().OrderBy(___=>___)),
						af = _.First().Reference.Contragents
					}).Distinct().
						ToArray();
			return objtypes;
		}

		private static IEnumerable<ObjColQueryGeneratorStruct> GetColumnAndObjectGroups(IEnumerable<IQuery> queries) {
			var colobj =
				queries.GroupBy(_ => _.Reference.GetCacheKey() + _.Obj.GetCacheKey() + _.Col.GetCacheKey(), _ => _).Select(
					_ =>
					new ObjColQueryGeneratorStruct
						{
							o = _.First().Obj.Id,
							c = _.First().Col.Id,
							t = _.First().Obj.Type,
							m = _.First().Obj.DetailMode,
							af = _.First().Reference.Contragents
						}).Distinct().
				        ToArray();
			return colobj;
		}

		private static IEnumerable<TimeQueryGeneratorStruct> GetTimedGroup(IEnumerable<IQuery> queries) {
			var times =
				queries.GroupBy(_ => _.Time.GetCacheKey(), _ => _.Time).Select(
					_ =>
					new TimeQueryGeneratorStruct
						{
							y = _.First().Year,
							p = _.First().Period,
							ps = _.First().Periods == null ? "" : string.Join(",", _.First().Periods)
						}).ToArray();
			return times;
		}


		private string GenerateQuery(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj, string rowids, string colids,
		                             PrimaryQueryPrototype prototype) {
			var select = GetSelectPart(time, cobj, prototype);
			var from = GetFromPart(prototype);
			var where = GetWherePart(time, cobj, rowids,colids, prototype);
			var groupby = GetGroupByPart(cobj, prototype);
			return select + from + where + groupby;
		}



		private string GetWherePart(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj, string rowids,string colids,
		                            PrimaryQueryPrototype prototype) {
			var objfld = GetObjectField(cobj);
			var contragentCondition = GetContragentCondition(cobj);
			var detailCondition = GetDetailCondition(cobj, prototype);
			var colcondition = " col = " + cobj.c;
			if (!string.IsNullOrWhiteSpace(colids)) {
				colcondition = " col in ( " + colids + ")";
			}
			var objcondition = objfld + "=" + cobj.o;
			if (0==cobj.o) {
				objcondition = objfld + " in ( " + cobj.ids + " )";
			}

			if (string.IsNullOrWhiteSpace(time.ps)) {
				return string.Format(
					@" WHERE period={0} and year={1} and {2} and {5} and row in ({4}){6}{7}  ",
					time.p, time.y, colcondition, cobj.o, rowids, objcondition, detailCondition, contragentCondition);
			}
			return string.Format(
				" WHERE period in ({0}) and year={1} and {2} and  {5} and row in ({4}){6}{7} ",
				time.ps, time.y, colcondition, cobj.o, rowids, objcondition, detailCondition, contragentCondition);
		}

		private static string GetContragentCondition(ObjColQueryGeneratorStruct cobj) {
			var altobjcond = "";
			if (!string.IsNullOrWhiteSpace(cobj.af)) {
				altobjcond = " and altobj in (" + cobj.af + ")";
			}
			return altobjcond;
		}

		private static string GetDetailCondition(ObjColQueryGeneratorStruct cobj, PrimaryQueryPrototype prototype) {
			var detcond = "";
			if (cobj.t == ZoneType.Obj) {
				if (prototype.PreserveDetails) {
					detcond = " and detail is null ";
				}
			}
			else if (prototype.RequireDetails) {
				detcond = " and detail is not null ";
			}
			return detcond;
		}

		private static string GetSelectPart(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj,
		                             PrimaryQueryPrototype prototype) {
			var objfld = GetObjectField(cobj);
			var valuereference = GetValueReference(prototype);
			var idfld = prototype.UseSum ? "0" : "id";
			return string.Format("\r\nSELECT {0},col,row,{1},year,{2}, {3} , {4} ,'{5}'  ", idfld, objfld, time.p, valuereference,
			                     (int) cobj.t, cobj.af);
		}

		private string GetGroupByPart(ObjColQueryGeneratorStruct cobj, PrimaryQueryPrototype prototype) {
			var objfld = GetObjectField(cobj);
			if (prototype.UseSum) {
				if (prototype.AggregatePeriod) {
					return string.Format(" GROUP BY col,row,{0},year", objfld);
				}
				return string.Format(" GROUP BY col,row,{0},year,period", objfld);
			}
			return "";
		}

		private static string GetObjectField(ObjColQueryGeneratorStruct cobj) {
			return cobj.t == ZoneType.Detail ? "detail" : "obj";
		}


		private static string GetFromPart(PrimaryQueryPrototype prototype) {
			return prototype.UseViewInsteadOfTable ? " FROM data " : " FROM cell ";
		}

		private static string GetValueReference(PrimaryQueryPrototype prototype) {
			var sb = new StringBuilder();
			if (prototype.UseSum) {
				sb.Append("SUM(");
			}

			if (prototype.UseZetaEval) {
				sb.Append("[zeta].[eval](id, decimalvalue, valuta, '");
				sb.Append(prototype.Currency);
				sb.Append("', year, period, row, col, 0,0)");
			}
			else {
				sb.Append("decimalvalue");
			}

			if (prototype.UseSum) {
				sb.Append(")");
			}

			return sb.ToString();
		}
	}
}