using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.CodeMetrics
{
	/// <summary>
	/// Результирующая метрика
	/// </summary>
	[Serialize]
	public class MetricResult {
		/// <summary>
		/// Имя метрики
		/// </summary>
		[SerializeNotNullOnly]
		public string Name { get; set; }
		/// <summary>
		/// Группа метрики
		/// </summary>
		[SerializeNotNullOnly]
		public string Group { get; set; }
		/// <summary>
		/// Индекс в выдаче
		/// </summary>
		[SerializeNotNullOnly]
		public int Idx { get; set; }

		/// <summary>
		/// Комментарий к метрике
		/// </summary>
		[SerializeNotNullOnly]
		public string Comment { get; set; }

		/// <summary>
		/// Подгруппа показателя
		/// </summary>
		[SerializeNotNullOnly]
		public string SubGroup { get; set; }
		/// <summary>
		/// Тип показателя
		/// </summary>
		[SerializeNotNullOnly]
		public string Type { get; set; }
		/// <summary>
		/// Имя показателя
		/// </summary>
		[SerializeNotNullOnly]
		public string ItemName { get; set; }

		/// <summary>
		/// Значение метрики
		/// </summary>
		[SerializeNotNullOnly]
		public decimal Value { get; set; }

		/// <summary>
		/// Единица измерения
		/// </summary>
		[SerializeNotNullOnly]
		public string Measure { get; set; }

		/// <summary>
		/// Минимальное нормальное значени
		/// </summary>
		[SerializeNotNullOnly]
		public decimal MinValue { get; set; }

		/// <summary>
		/// Максимальное нормальное значение
		/// </summary>
		[SerializeNotNullOnly]
		public decimal MaxValue { get; set; }

		/// <summary>
		/// Требуемая тенденция
		/// </summary>
		[SerializeNotNullOnly]
		public MetricTendency Tendency { get; set; }
		/// <summary>
		/// Важность метрики
		/// </summary>
		[SerializeNotNullOnly]
		public MetricImportance Importance { get; set; }
	}
}
