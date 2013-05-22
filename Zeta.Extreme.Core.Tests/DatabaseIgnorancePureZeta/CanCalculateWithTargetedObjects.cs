using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class CanCalculateWithTargetedObjects : PureZetaTestFixtureBase
	{
		private Obj obj1;
		private Obj obj2;
		private Detail det1;
		private Detail det2;

		protected override IEnumerable<Query> BuildModel() {
			obj1 = new Obj {Id = 1};
			obj2 = new Obj { Id = 2 };
			det1 = new Detail {Id = 1};
			det2 = new Detail { Id = 2 };
			Add(new Query { Row = { Code = "x",TargetObject = obj1 }, Col = { Code = "a" }, Obj = { Id = 1 } }, 1);
			Add(new Query { Row = { Code = "x", TargetObject = obj2 }, Col = { Code = "a" }, Obj = { Id = 2 } }, 2);
			Add(new Query { Row = { Code = "x", TargetObject = det1 }, Col = { Code = "a" }, Obj = { Id = 1, Type = ZoneType.Detail } }, 3);
			Add(new Query { Row = { Code = "x", TargetObject = det2 }, Col = { Code = "a" }, Obj = { Id = 2, Type = ZoneType.Detail } }, 4);
			
			var sumobjrow = new Row {Code = "sum", MarkCache = "/0SA/"};		
			var fstobjrow = new Row { Code = "x", TargetObject = obj1}; //both rows same code
			var secobjrow = new Row { Code = "x",TargetObject = obj2};
			sumobjrow.Children.Add(fstobjrow);
			sumobjrow.Children.Add(secobjrow);
			var sumdetrow = new Row { Code = "sum", MarkCache = "/0SA/" };
			var fstdetrow = new Row { Code = "x", TargetObject = det1 }; //both rows same code
			var secdetrow = new Row { Code = "x", TargetObject = det2 };
			sumdetrow.Children.Add(fstdetrow);
			sumdetrow.Children.Add(secdetrow);
			yield return new Query
				{
					Row={Native = sumobjrow},
					Col={Native = new Column {Code = "a"}},
					Obj={Native = new Obj{ Id = 3}}
				};
			yield return new Query
				{
					Row = { Native = sumdetrow },
					Col = { Native = new Column { Code = "a" } },
					Obj = { Native = new Detail { Id = 3 } }
				};
		}

	
		
		protected override void Examinate(Query query) {
			if (query.Obj.IsForObj) {
				Assert.AreEqual(3, query.Result.NumericResult);
			}
			else {
				Assert.AreEqual(7, query.Result.NumericResult);
			}
			
		}
	}
}