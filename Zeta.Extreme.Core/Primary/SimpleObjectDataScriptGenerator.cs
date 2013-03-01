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
			var usualperiods = queries.Where(_ => _.Time.Periods == null).ToArray();
			var sumperiods = queries.Where(_ => _.Time.Periods != null).ToArray();
			var script = "";
			if (0 != usualperiods.Length)
			{
				var times = usualperiods.Select(_ => new { y = _.Time.Year, p = _.Time.Period }).Distinct();
				var colobj = usualperiods.Select(_ => new { o = _.Obj.Id, c = _.Col.Id }).Distinct();
				var rowids = string.Join(",", usualperiods.Select(_ => _.Row.Id).Distinct());

				foreach (var time in times)
				{
					foreach (var cobj in colobj)
					{
						script +=
							string.Format(
								@"
								if exists(select top 1 id from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4}))
									select id,col,row,obj,year,period,decimalvalue 
									from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4})",
								time.p, time.y, cobj.c, cobj.o, rowids);
					}
				}
			}
			if (0 != sumperiods.Length)
			{
				var sptimes =
					sumperiods.Select(_ => new { y = _.Time.Year, p = _.Time.Period, ps = string.Join(",", _.Time.Periods) }).Distinct();
				var spcolobj = sumperiods.Select(_ => new { o = _.Obj.Id, c = _.Col.Id }).Distinct();
				var sprowids = string.Join(",", sumperiods.Select(_ => _.Row.Id).Distinct());
				foreach (var time in sptimes)
				{
					foreach (var cobj in spcolobj)
					{
						script +=
							string.Format(
								"\r\nselect 0,col,row,obj,year,{5},sum(decimalvalue) from cell where period in ({0}) and year={1} and col={2} and obj={3} and row in ({4}) group by col,row,obj,year ",
								time.ps, time.y, cobj.c, cobj.o, sprowids, time.p);
					}
				}
			}
			return script;
		}
	}
}