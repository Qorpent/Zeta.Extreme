using System;
using NUnit.Framework;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests.PrimaryTests {
	[TestFixture]
	public class AF575TypeToSqlTest {
		[Test]
		public void QueryWithTypeWillBeParsedWell()
		{
			var query = new Query { Row = { Id = 1 }, Col = { Id = 1 }, Time = { Year = 2012, Period = 1 }, Obj = { Id = 1 }, Reference = {Contragents  = "352", Types = "1,2,3" } };
			var script = new Zeta2SqlScriptGenerator().Generate(new IQuery[] { query }, new PrimaryQueryPrototype());
			Console.WriteLine(script);
			Assert.AreEqual(@"
SELECT id,col,row,obj,year,1, decimalvalue , 1 ,'352:1,2,3'   FROM cell  WHERE period=1 and year=2012 and  col in ( 1) and obj=1 and row in (1) and altobj in (352) and dtypecode in ( '1','2','3' )", script);
		}
	}
}