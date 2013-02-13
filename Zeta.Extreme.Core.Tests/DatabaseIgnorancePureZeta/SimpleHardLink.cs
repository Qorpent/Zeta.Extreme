#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SimpleHardLink.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class SimpleHardLink : PureZetaTestFixtureBase {
		/// <summary>
		/// 	������ ������ � ���������� ������ �������
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			var q1 = new Query {Row = {Code = "x"}};
			Add(q1, 100);
			var q2 = new Query
				{
					Row =
						{
							Native = new row
								{
									Code = "y",
									RefTo = new row
										{
											Code = "z",
											RefTo = new row {Code = "x"}
										}
								}
						}
				};
			yield return q2;
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(100, query.Result.NumericResult);
			Assert.AreEqual(1, _session.Registry.Count); //��� �� ������ ���� ������� �����������
		}
	}

	[TestFixture]
	public class SimpleFormula : PureZetaTestFixtureBase {
		/// <summary>
		/// 	������ ������ � ���������� ������ �������
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query {Row = {Code = "x"}}, 5);
			Add(new Query {Row = {Code = "y"}}, 5);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "u"}}, 6);
			yield return new Query { Row = { Code = "PureZetaTestFixtureBase", Formula = "$x? * $y@u?", FormulaType = "boo" } };
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(30, query.Result.NumericResult);
		}
	}
}