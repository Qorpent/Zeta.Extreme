using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests
{
	/// <summary>
	/// Проверяем проброс ошибок из формул и суммм
	/// </summary>
	[TestFixture]
	public class ZC459ExceptionPropagationTest
	{
		private Session session;

		/// <summary>
		/// тут еще ловился баг с неправильным резольвингом кодов строк
		/// </summary>
		[Test]
		public void FormulasCanPropagateErrors()
		{
			var q = new Query { Row = { Code = "r2" }, IgnoreCheckPrimaryExistence = true };
			var result = session.AsSerial().Eval(q);
			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
			Assert.IsInstanceOf(typeof(QueryException), result.Error.InnerException);
			Assert.AreEqual("1", result.Error.InnerException.InnerException.Message);
		}
		[SetUp]
		public void Setup() {
			var re = new Row { Code = "re", IsFormula = true, Formula = " this is error ", FormulaType = "boo" };
			var r0 = new Row {Code = "r0", IsFormula = true, Formula = " raise('1') ", FormulaType = "boo"};
			var r1 = new Row {Code = "r1", IsFormula = true, Formula = "123 / $r0? ", FormulaType = "boo"};
			var r2 = new Row {Code = "r2", IsFormula = true, Formula = " $r1? ", FormulaType = "boo"};
			var r3 = new Row { Code = "r3", IsFormula = true, Formula = " $r1? * $r1? ", FormulaType = "boo" };
			var r4 = new Row { Code = "r4", IsFormula = true, Formula = " $re? * $re? ", FormulaType = "boo" };
			var r5 = new Row { Code = "r5", IsFormula = true, Formula = " $re? + $re? ", FormulaType = "boo" };

			var r6 = new Row { Code = "r6", IsFormula = true, Formula = " $r7? * $r7? ", FormulaType = "boo" };
			var r7 = new Row { Code = "r7", IsFormula = true, Formula = " $r6? * $r6? ", FormulaType = "boo" };

			var r8 = new Row { Code = "r8", IsFormula = true, Formula = " $r9? + $r9? ", FormulaType = "boo" };
			var r9 = new Row { Code = "r9", IsFormula = true, Formula = " $r8? + $r8? ", FormulaType = "boo" };
			FormulaStorage.Default.Clear();
			RowCache.Bycode.Remove("RE");
			RowCache.Bycode.Remove("R0");
			RowCache.Bycode.Remove("R1");
			RowCache.Bycode.Remove("R2");
			RowCache.Bycode.Remove("R3");
			RowCache.Bycode.Remove("R4");
			RowCache.Bycode.Remove("R5");
			RowCache.Bycode.Remove("R6");
			RowCache.Bycode.Remove("R7");
			session = new Session();
			session.GetMetaCache().Set(r0).Set(r1).Set(r2).Set(r3).Set(re).Set(r4).Set(r5).Set(r6).Set(r7).Set(r8).Set(r9);
		}

		[Test]
		public void FormulasCanPropagateErrors2() {

			var q = new Query { Row = { Code = "r3" }, IgnoreCheckPrimaryExistence = true };
			var result = session.AsSerial().Eval(q);
			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
			Assert.IsInstanceOf(typeof(QueryException),result.Error.InnerException);
			Assert.AreEqual("1",result.Error.InnerException.InnerException.InnerException.Message);
		}

		[Test]
		public void CompileTimeErrorPropagated()
		{

			var q = new Query { Row = { Code = "r4" }, IgnoreCheckPrimaryExistence = true };
			var result = session.AsSerial().Eval(q);
			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
		}

		[Test]
		public void SumsPropagatesErrors() {
			var q = new Query { Row = { Code = "r5" }, IgnoreCheckPrimaryExistence = true };
			q = (Query) session.Register(q);
			var result = session.AsSerial().Eval(q);

			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
			Assert.IsInstanceOf(typeof(QueryException), result.Error);
		}

		[Test]
		public void CanPreventAndPropagateCircularFormula()
		{
			var q = new Query { Row = { Code = "r7" }, IgnoreCheckPrimaryExistence = true };
			q = (Query) session.Register(q);
			var result = session.AsSerial().Eval(q);
			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
			Assert.AreEqual("circular dependency", result.Error.InnerException.Message);
		}

		[Test]
		public void CanPreventAndPropagateCircularSumma()
		{
			var q = new Query { Row = { Code = "r9" },IgnoreCheckPrimaryExistence = true};
			q = (Query)session.Register(q);
			var result = session.AsSerial().Eval(q);
			Assert.False(result.IsComplete);
			Assert.NotNull(result.Error);
			Assert.AreEqual("circular dependency", result.Error.InnerException.Message);
		}
	}
}
