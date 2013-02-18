using System.Collections.Generic;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class CustomCodeSupport : PureZetaTestFixtureBase
	{
		/// <summary>
		/// � ����� ������ ���� CustomCode �� ����� ��� �������������� ������ 
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			//��������� ����� CustomCols
			_session.MetaCache.Set(new col {Code = "CUSTOM", ForeignCode = "x", Year = 2013, Period = 13});
			//���������� �������� ������
			Add(new Query { Row = { Code = "x" } , Col={Code="x"},Time={Year=2013,Period = 13}}, 5);	
					
			yield return new Query { Row = { Code = "x"}, Time = {Year = 2014,Period = 1}, Col = {Formula = "@CUSTOM? * @CUSTOM?", FormulaType = "boo"}};
		}

		protected override void Examinate(Query query)
		{
			Assert.AreEqual(25, query.Result.NumericResult);
		}
	}
}