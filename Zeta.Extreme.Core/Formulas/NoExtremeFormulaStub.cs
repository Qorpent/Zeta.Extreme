using System;

namespace Zeta.Extreme {
	/// <summary>
	/// Специальная заглушка для возврата псевдозначения
	/// </summary>
	public class NoExtremeFormulaStub : FormulaBase
	{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="request"></param>
		public override void SetContext(FormulaRequest request)
		{
			base.SetContext(request);
			Result = new QueryResult { IsComplete = false, Error = new NotSupportedException("formula ignored by noextreme tag") };
		}
		/// <summary>
		/// Константный результат для данной формулы
		/// </summary>
		protected QueryResult Result;

		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval()
		{
			return Result;
		}
	}
}