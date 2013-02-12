using System.Collections.Generic;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class DoExRefBaseTest : PureZetaTestFixtureBase
	{
		/// <summary>
		/// строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Query> BuildModel() {
			var exrefrow = new row {Code = "e", ExRefTo = new row {Code = "t"}};
			var exrefcol = new col {Code = "e", MarkCache = "/DOEXREF/"};
			var nexrefcol = new col {Code = "ne"};
			var q1 = new Query //тут должен произойти exref
				{
					Row = {Native = exrefrow},
					Col = {Native = exrefcol}
				};
			yield return q1;
			var q2 = new Query //а тут нет
				{
					Row = { Native = exrefrow },
					Col = { Native = nexrefcol }
				};
			yield return q2;

			Add(new Query{Row={Code="t"},Col = {Code="e"}}, 1);
			Add(new Query{Row={Code="t"},Col = {Code="ne"}}, 2);
			Add(new Query{Row={Code="e"},Col = {Code="ne"}}, 3);
			yield return q2;
		}

		protected override void Examinate(Query query)
		{
			if(query.Col.Code=="e") {
				Assert.AreEqual(1,query.Result.NumericResult);
			}
			if (query.Col.Code == "ne")
			{
				Assert.AreEqual(3, query.Result.NumericResult);
			}

			Assert.AreEqual(2, _session.Registry.Count); //тут не должно быть никаких подзапросов

		}
	}
}