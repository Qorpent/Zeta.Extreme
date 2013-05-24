#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/MockFormulaTest.cs
#endregion
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;
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
			_mquery = new Query {Row = {Code = "r1"},IgnoreCheckPrimaryExistence = true};
			_mquerySs = new Query {Row = {Code = "r2"}, Col = {Code = "PLAN"}, Obj = {Id = 352}, Time = {Year = 2012, Period = 301},IgnoreCheckPrimaryExistence = true};
		}

		private IQuery _mquery;
		private IQuery _mquerySs;

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