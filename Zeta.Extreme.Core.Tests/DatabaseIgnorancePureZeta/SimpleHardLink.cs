#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SimpleHardLink.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class SimpleHardLink : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			var q1 = new Query {Row = {Code = "x"}};
			Add(q1, 100);
			var q2 = new Query
				{
					Row =
						{
							Native = new Row
								{
									Code = "y",
									RefTo = new Row
										{
											Code = "z",
											RefTo = new Row {Code = "x"}
										}
								}
						}
				};
			yield return q2;
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(100, query.Result.NumericResult);
			Assert.AreEqual(1, _session.Registry.Count); //тут не должно быть никаких подзапросов
		}
	}
}