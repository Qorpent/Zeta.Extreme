#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SimpleObjectDataScriptGenerator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Класс для простой генерации скриптов в простейшем варианте (на объект)
	/// </summary>
	public class SimpleObjectDataScriptGenerator : IScriptGenerator {
		/// <summary>
		/// 	Строит SQL запрос с учетом прототипа, а по сути "хинтов" запроса
		/// </summary>
		/// <param name="queries"> </param>
		/// <param name="prototype"> </param>
		/// <returns> </returns>
		public string Generate(IQuery[] queries, PrimaryQueryPrototype prototype) {
			var script = "";
			if (0 != queries.Length) {
				var times =
					queries.GroupBy(_ => _.Time.GetCacheKey(), _ => _.Time).Select(
						_ =>
						new TimeQueryGeneratorStruct
							{
								y = _.First().Year,
								p = _.First().Period,
								ps = _.First().Periods == null ? "" : string.Join(",", _.First().Periods)
							}).ToArray();
				var colobj =
					queries.GroupBy(_ => _.Obj.GetCacheKey() + _.Col.GetCacheKey(), _ => _).Select(
						_ =>
						new ObjColQueryGeneratorStruct
							{o = _.First().Obj.Id, c = _.First().Col.Id, t = _.First().Obj.Type, m = _.First().Obj.DetailMode}).Distinct().
						ToArray();
				var rowids = string.Join(",", queries.Select(_ => _.Row.Id).Distinct());
				script = times.Aggregate(script,
				                         (_, time) =>
				                         colobj.Aggregate(_,
				                                          (current, cobj) => current + GenerateQuery(time, cobj, rowids, prototype)));
			}
			return script;
		}

		private string GenerateQuery(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj, string rowids,
		                             PrimaryQueryPrototype prototype) {
			if (!prototype.UseSum &&
			    (cobj.t == ObjType.Obj || cobj.t == ObjType.Detail || cobj.t == ObjType.None /* obj by default */)) {
				return ConvertToSimpleQueryOfObject(time, cobj, rowids, prototype);
			}
			throw new NotSupportedException(prototype.ToString());
		}

		private static string ConvertToSimpleQueryOfObject(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj,
		                                                   string rowids, PrimaryQueryPrototype prototype) {
			var objfld = cobj.t == ObjType.Detail ? "detail" : "obj";
			var detcond = "";
			if (cobj.t == ObjType.Obj) {
				if (prototype.PreserveDetails) {
					detcond = " and detail is null ";
				}
			}
			var valuesel = "decimalvalue";
			if (prototype.RequreZetaEval) {
				valuesel = "[zeta].[eval](id, decimalvalue, valuta, '" + prototype.Valuta + "', year, period, row, col, 0,0)";
			}
			if (string.IsNullOrWhiteSpace(time.ps)) {
				return string.Format(
					@"
select id,col,row,{5},year,period,{7} , {8}
from cell where period={0} and year={1} and col={2} and {5}={3} and row in ({4}) {6}",
					time.p, time.y, cobj.c, cobj.o, rowids, objfld, detcond, valuesel, (int) cobj.t);
			}
			return string.Format(
				"\r\nselect 0,col,row,{6},year,{5},sum({8}), {9} from cell where period in ({0}) and year={1} and col={2} and {6}={3} and row in ({4}) {7} group by col,row,{6},year ",
				time.ps, time.y, cobj.c, cobj.o, rowids, time.p, objfld, detcond, valuesel, (int) cobj.t);
		}
	}
}