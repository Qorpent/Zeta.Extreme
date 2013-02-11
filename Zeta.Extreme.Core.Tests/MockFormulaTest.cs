using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests
{
	[TestFixture]
	public class MockFormulaTest:SessionTestBase
	{
		private ZexQuery _mquery;
		private ZexQuery _mquery_ss;

		public class MockFormula : IFormula {
			
			private int _result;

			/// <summary>
			/// Настраивает формулу на конкретный переданный запрос
			/// </summary>
			/// <param name="query"></param>
			public void Init(ZexQuery query) {
				_result = query.Row.Id;
			}

			/// <summary>
			/// Команда вычисления результата
			/// </summary>
			/// <returns></returns>
			/// <remarks>В принципе кроме вычисления результата формула не должна ничего уметь</remarks>
			public QueryResult Eval() {
				return new QueryResult(_result);
			}

			/// <summary>
			/// Выполняет очистку ресурсов формулы после использования
			/// </summary>
			public void CleanUp() {
				
			}
		}

		public class MockSubsessionFormula : IFormula {
			private ZexQuery _subquery;
			private ISerialSession _session;
			private ZexSession _mastersession;


			/// <summary>
			/// Настраивает формулу на конкретный переданный запрос
			/// </summary>
			/// <param name="query"></param>
			public void Init(ZexQuery query) {
				_subquery = new ZexQueryDelta {RowCode = "m1122350",ObjId = 1046}.Apply(query);
				if(null==query.Session) {
					_session = new ZexSession().AsSerial();
				}else {
					_mastersession = query.Session;
					_session = query.Session.GetSubSession();
				}
			}

			/// <summary>
			/// Команда вычисления результата
			/// </summary>
			/// <returns></returns>
			/// <remarks>В принципе кроме вычисления результата формула не должна ничего уметь</remarks>
			public QueryResult Eval() {
				return _session.Eval(_subquery);
			}

			public void CleanUp() {
				if(null!=_mastersession && null!=_session) {
					_mastersession.ReturnSubSession(_session);
				}
			}
		}

		[SetUp]
		public override void setup() {
			base.setup();
			FormulaStorage.Default.Register(new FormulaRequest {Key = "f1", PreparedType = typeof (MockFormula)});
			FormulaStorage.Default.Register(new FormulaRequest { Key = "f2", PreparedType = typeof(MockSubsessionFormula) });
			//need uppercase code for RowCache propose
			RowCache.bycode["R1"] = new row {Id = -123, Code = "r1", IsFormula = true, Formula = "f1",FormulaEvaluator = "mock"};
			RowCache.bycode["R2"] = new row { Id = -124, Code = "r1", IsFormula = true, Formula = "f2", FormulaEvaluator = "mock" };
			_mquery = new ZexQuery {Row = {Code = "r1"}};
			_mquery_ss = new ZexQuery { Row = { Code = "r2" }, Col={Code="PLAN"},Obj={Id=352},Time={Year=2012,Period=301} };
			
		}

		[Test]
		public void CanEvalMockFormula() {
			var result = _serial.Eval(_mquery);
			Assert.AreEqual(-123,result.NumericResult);
		}

		[Test]
		public void CanEvalMockSubsessionFormula()
		{
			var result = _serial.Eval(_mquery_ss);
			Assert.AreEqual(11840m, result.NumericResult);
		}

	}
}
