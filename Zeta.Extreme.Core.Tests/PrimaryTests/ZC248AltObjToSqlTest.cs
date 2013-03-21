using System;
using NUnit.Framework;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests.PrimaryTests
{
	[TestFixture]
	public class ZC248AltObjToSqlTest
	{
		[Test]
		public void QueryWithAltObjWillBeParsedWell() {
			var query = new Query{Row={Id=1},Col={Id=1},Time={Year = 2012,Period = 1},Obj={Id=1,AltObjFilter = "1,2,3"}};
			var script = new SimpleObjectDataScriptGenerator().Generate(new IQuery[] {query}, new PrimaryQueryPrototype());
			Console.WriteLine(script);
			Assert.AreEqual(@"
select 0,col,row,obj,year,period,sum(decimalvalue) , 0
from cell where period=1 and year=2012 and col=1 and obj=1 and row in (1) and altobj in ('1,2,3') group by col,row,obj,year,period ", script);
		}

		[Test]
		public void AltAndNotAltQueriesWillBeSplitted()
		{
			var query = new Query { Row = { Id = 1 }, Col = { Id = 1 }, Time = { Year = 2012, Period = 1 }, Obj = { Id = 1, AltObjFilter = "1,2,3" } };
			var query2 = new Query { Row = { Id = 1 }, Col = { Id = 1 }, Time = { Year = 2012, Period = 1 }, Obj = { Id = 1 } };
			var script = new SimpleObjectDataScriptGenerator().Generate(new IQuery[] { query,query2 }, new PrimaryQueryPrototype());
			Console.WriteLine(script);
			Assert.AreEqual(@"
select 0,col,row,obj,year,period,sum(decimalvalue) , 0
from cell where period=1 and year=2012 and col=1 and obj=1 and row in (1) and altobj in ('1,2,3') group by col,row,obj,year,period 
select id,col,row,obj,year,period,decimalvalue , 0
from cell where period=1 and year=2012 and col=1 and obj=1 and row in (1)", script);
		}
	}
}
