namespace Zeta.Extreme {
	/// <summary>
	/// Базовый интерфейс формулы
	/// </summary>
	public interface IFormula {

		/// <summary>
		/// Настраивает формулу на конкретный переданный запрос
		/// </summary>
		/// <param name="query"></param>
		void Init(ZexQuery query);
		/// <summary>
		/// Команда вычисления результата
		/// </summary>
		/// <returns></returns>
		/// <remarks>В принципе кроме вычисления результата формула не должна ничего уметь</remarks>
		QueryResult Eval();
		/// <summary>
		/// Выполняет очистку ресурсов формулы после использования
		/// </summary>
		void CleanUp();
	}
}