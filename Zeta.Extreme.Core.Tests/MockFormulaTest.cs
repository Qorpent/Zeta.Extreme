#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : MockFormulaTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class MockFormulaTest : SessionTestBase {
		[SetUp]
		public override void setup() {
			base.setup();
			FormulaStorage.Default.Register(new FormulaRequest {Key = "row:r1", PreparedType = typeof (MockFormula)});
			FormulaStorage.Default.Register(new FormulaRequest {Key = "row:r2", PreparedType = typeof (MockSubsessionFormula)});
			//need uppercase code for RowCache propose
			RowCache.Bycode["R1"] = new row {Id = -123, Code = "r1", IsFormula = true, Formula = "f1", FormulaEvaluator = "mock"};
			RowCache.Bycode["R2"] = new row {Id = -124, Code = "r2", IsFormula = true, Formula = "f2", FormulaEvaluator = "mock"};
			_mquery = new Query {Row = {Code = "r1"}};
			_mquery_ss = new Query
				{Row = {Code = "r2"}, Col = {Code = "PLAN"}, Obj = {Id = 352}, Time = {Year = 2012, Period = 301}};
		}

		private Query _mquery;
		private Query _mquery_ss;

		public class MockFormula : IFormula {
			/// <summary>
			/// 	Настраивает формулу на конкретный переданный запрос
			/// </summary>
			/// <param name="query"> </param>
			public void Init(Query query) {
				_result = query.Row.Id;
			}

			/// <summary>
			/// Устанавливает контекст использования формулы
			/// </summary>
			/// <param name="request"></param>
			public void SetContext(FormulaRequest request) {
				
			}

			/// <summary>
			/// 	Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
			/// </summary>
			/// <param name="query"> </param>
			public void Playback(Query query) {}

			/// <summary>
			/// 	Команда вычисления результата
			/// </summary>
			/// <returns> </returns>
			/// <remarks>
			/// 	В принципе кроме вычисления результата формула не должна ничего уметь
			/// </remarks>
			public QueryResult Eval() {
				return new QueryResult(_result);
			}

			/// <summary>
			/// 	Выполняет очистку ресурсов формулы после использования
			/// </summary>
			public void CleanUp() {}

			private int _result;
		}

		public class MockSubsessionFormula : IFormula {
			/// <summary>
			/// 	Настраивает формулу на конкретный переданный запрос
			/// </summary>
			/// <param name="query"> </param>
			public void Init(Query query) {
				_subquery = new QueryDelta {RowCode = "m1122350", ObjId = 1046}.Apply(query);
				if (null == query.Session) {
					_session = new Session().AsSerial();
				}
				else {
					_mastersession = query.Session;
					_session = query.Session.GetSubSession();
				}
			}

			/// <summary>
			/// Устанавливает контекст использования формулы
			/// </summary>
			/// <param name="request"></param>
			public void SetContext(FormulaRequest request) {
				
			}

			/// <summary>
			/// 	Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
			/// </summary>
			/// <param name="query"> </param>
			public void Playback(Query query) {}

			/// <summary>
			/// 	Команда вычисления результата
			/// </summary>
			/// <returns> </returns>
			/// <remarks>
			/// 	В принципе кроме вычисления результата формула не должна ничего уметь
			/// </remarks>
			public QueryResult Eval() {
				return _session.Eval(_subquery);
			}

			public void CleanUp() {
				if (null != _mastersession && null != _session) {
					_mastersession.Return(_session);
				}
			}

			private Session _mastersession;
			private ISerialSession _session;
			private Query _subquery;
		}

		[Test]
		public void CanEvalMockFormula() {
			var result = _serial.Eval(_mquery);
			Assert.AreEqual(-123, result.NumericResult);
		}

		[Test]
		[Ignore("Subsessions are not actual feature")]
		public void CanEvalMockSubsessionFormula() {
			var result = _serial.Eval(_mquery_ss);
			Assert.AreEqual(11840m, result.NumericResult);
		}
	}
}