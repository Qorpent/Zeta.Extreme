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
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.PocoClasses;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class MockFormulaTest : SessionTestBase {
		[SetUp]
		public override void setup() {
			base.setup();
			FormulaStorage.Default.Register(new FormulaRequest {Key = "row:r1", PreparedType = typeof (MockFormula)});
			//need uppercase code for RowCache propose
			RowCache.Bycode["R1"] = new Row {Id = -123, Code = "r1", IsFormula = true, Formula = "f1", FormulaType = "mock"};
			RowCache.Bycode["R2"] = new Row {Id = -124, Code = "r2", IsFormula = true, Formula = "f2", FormulaType = "mock"};
			_mquery = new Query {Row = {Code = "r1"}};
			_mquery_ss = new Query
				{Row = {Code = "r2"}, Col = {Code = "PLAN"}, Obj = {Id = 352}, Time = {Year = 2012, Period = 301}};
		}

		private IQuery _mquery;
		private IQuery _mquery_ss;

		public class MockFormula : IFormula {
			/// <summary>
			/// 	Настраивает формулу на конкретный переданный запрос
			/// </summary>
			/// <param name="query"> </param>
			public void Init(IQuery query) {
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
			public void Playback(IQuery query) {}

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

		

		[Test]
		public void CanEvalMockFormula() {
			var result = _serial.Eval(_mquery);
			Assert.AreEqual(-123, result.NumericResult);
		}

	
	}
}