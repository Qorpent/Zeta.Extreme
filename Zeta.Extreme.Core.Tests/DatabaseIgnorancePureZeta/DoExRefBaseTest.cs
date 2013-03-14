using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class DoExRefBaseTest : PureZetaTestFixtureBase
	{
		/// <summary>
		/// ������ ������ � ���������� ������ �������
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Query> BuildModel() {
			var exrefrow = new Row {Code = "e", ExRefTo = new Row {Code = "t"}};
			var exrefcol = new Column {Code = "e", MarkCache = "/DOEXREF/"};
			var nexrefcol = new Column {Code = "ne"};
			var q1 = new Query //��� ������ ��������� exref
				{
					Row = {Native = exrefrow},
					Col = {Native = exrefcol}
				};
			yield return q1;
			var q2 = new Query //� ��� ���
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

			Assert.AreEqual(2, _session.Registry.Count); //��� �� ������ ���� ������� �����������

		}
	}
}