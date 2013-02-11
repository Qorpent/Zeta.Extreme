namespace Zeta.Extreme {
	/// <summary>
	/// Специальная заглушка для возврата псевдозначения
	/// </summary>
	public class CompileErrorFormulaStub : DeltaFormulaBase {
		
		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override object EvaluateExpression() {
			return "error in compilation";
		}
	}
}