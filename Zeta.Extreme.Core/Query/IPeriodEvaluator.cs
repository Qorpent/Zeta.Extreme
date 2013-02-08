namespace Zeta.Extreme {
	/// <summary>
	/// Интерфейс расчтечика формульных периодов
	/// </summary>
	public interface IPeriodEvaluator {
		/// <summary>
		/// Возвращает временный TimeHandler со скорректированным годом и периодами
		/// </summary>
		/// <param name="basePeriod"></param>
		/// <param name="period"></param>
		/// <param name="year"></param>
		/// <returns></returns>
		TimeHandler Evaluate(int basePeriod, int period,int year);
	}
}