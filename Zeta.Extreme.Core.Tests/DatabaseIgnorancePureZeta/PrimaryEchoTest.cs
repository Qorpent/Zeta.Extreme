using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	/// <summary>
	/// ��������� ���� ������ �������������
	/// �� ��������� ������
	/// </summary>
	[TestFixture]
	public class PrimaryEchoTest : PureZetaTestFixtureBase {
		/// <summary>
		/// ������ ������ � ���������� ������ �������
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Query> BuildModel() {
			Query q;
			yield return q = new Query();
			Add(q,10);
			yield return q = new Query{Row={Code="x"}};
			Add(q,20);
		}

		protected override void Examinate(Query query) {
			if(query.Row.Code=="x") {
				Assert.AreEqual(20, query.Result.NumericResult);
			}else {
				Assert.AreEqual(10, query.Result.NumericResult);
			}
		}

	}
}