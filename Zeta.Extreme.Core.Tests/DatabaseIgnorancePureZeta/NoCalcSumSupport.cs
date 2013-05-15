using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class NoCalcSumSupport : PureZetaTestFixtureBase
	{
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			var row = new Row {Code = "a",MarkCache = "/0SA/"};
			row.Children.Add(new Row{Code="b"});
			row.Children.Add(new Row { Code = "c" });
			var row2 = new Row { Code = "x",MarkCache = "/0SA/CALCSUM/"};
			row2.Children.Add(new Row { Code = "y" });
			row2.Children.Add(new Row { Code = "z" });
			Add(new Query { Row = { Code = "b" } ,Col={Code="a"}}, 5);
			Add(new Query { Row = { Code = "c" }, Col = { Code = "a" } }, 5);
			Add(new Query { Row = { Code = "b" }, Col = { Code = "b" } }, 5);
			Add(new Query { Row = { Code = "c" }, Col = { Code = "b" } }, 5);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "a" } }, 5);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "a" } }, 5);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "b" } }, 5);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "b" } }, 5);
			var usc = new Column {Code = "a"};
			var ncsc = new Column {Code = "b", MarkCache = "/NOCALCSUM/"};
			yield return new Query { Row={Native = row},Col={Native= usc} };
			yield return new Query { Row = { Native = row2 }, Col = { Native = usc} };
			yield return new Query { Row = { Native = row }, Col = { Native = ncsc, } };
			yield return new Query { Row = { Native = row2 }, Col = { Native = ncsc} };
		}

		protected override void Examinate(Query query)
		{
			if (query.Col.Code == "a") {
				Assert.AreEqual(10,query.Result.NumericResult);
			}
			if (query.Col.Code == "b" && query.Row.Code == "a") {
				Assert.AreEqual(0, query.Result.NumericResult);
			}
			if (query.Col.Code == "b" && query.Row.Code == "x")
			{
				Assert.AreEqual(10, query.Result.NumericResult);
			}
		}
	}
}