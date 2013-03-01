using System.Linq;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// Класс для простой генерации скриптов в простейшем варианте (на объект)
	/// </summary>
	public class SimpleObjectDataScriptGenerator : IScriptGenerator {
		/// <summary>
		/// 	Строит SQL запрос с учетом прототипа, а по сути "хинтов" запроса
		/// </summary>
		/// <param name="queries"> </param>
		/// <param name="prototype"> </param>
		/// <returns> </returns>
		public string Generate(Query[] queries, PrimaryQueryPrototype prototype)
		{

			var script = "";
			if (0 != queries.Length)
			{
				var times = queries.GroupBy(_=>_.Time.GetCacheKey(),_=>_.Time).Select(_ => new TimeQueryGeneratorStruct { y = _.First().Year, p = _.First().Period, ps = _.First().Periods == null ? "" : string.Join(",", _.First().Periods) }).ToArray();
				var colobj = queries.GroupBy(_=>_.Obj.GetCacheKey()+_.Col.GetCacheKey(),_=>_).Select(_ => new ObjColQueryGeneratorStruct { o = _.First().Obj.Id, c = _.First().Col.Id, t = _.First().Obj.Type, m = _.First().Obj.DetailMode }).Distinct().ToArray();
				var rowids = string.Join(",", queries.Select(_ => _.Row.Id).Distinct());
				script = times.Aggregate(script, (_, time) => colobj.Aggregate(_, (current, cobj) => current + GenerateQuery(time, cobj, rowids, prototype)));
			}
			return script;
		}

		private string GenerateQuery(TimeQueryGeneratorStruct time, ObjColQueryGeneratorStruct cobj, string rowids, PrimaryQueryPrototype prototype) {
			if(string.IsNullOrWhiteSpace(time.ps)) {
				return string.Format(
					@"
							--	if exists(select top 1 id from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4}))
									select id,col,row,obj,year,period,decimalvalue 
									from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4})",
					time.p, time.y, cobj.c, cobj.o, rowids);
			}else {
				return string.Format(
								"\r\nselect 0,col,row,obj,year,{5},sum(decimalvalue) from cell where period in ({0}) and year={1} and col={2} and obj={3} and row in ({4}) group by col,row,obj,year ",
								time.ps, time.y, cobj.c, cobj.o, rowids, time.p);
			}
		}
	}
}