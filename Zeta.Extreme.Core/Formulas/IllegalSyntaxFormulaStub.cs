using System;

namespace Zeta.Extreme {
	/// <summary>
	/// —пециальна€ заглушка дл€ возврата по неверно укомплектованным формулам
	/// </summary>
	public class IllegalSyntaxFormulaStub : FormulaBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		public override void SetContext(FormulaRequest request)
		{
			base.SetContext(request);
			Result = new QueryResult { IsComplete = false, Error = new InvalidOperationException("formula syntax error "+request.Formula) };
		}
		/// <summary>
		///  онстантный результат дл€ данной формулы
		/// </summary>
		protected QueryResult Result;

		/// <summary>
		/// 	ћетод дл€ перекрыти€ при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval()
		{
			return Result;
		}
	}
}