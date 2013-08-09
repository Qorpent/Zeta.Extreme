using System.Collections.Generic;

namespace Zeta.Extreme.Developer.CodeMetrics {
	/// <summary>
	/// Реальный поставщик метрик
	/// </summary>
	public interface IMetricProvider {
		/// <summary>
		/// Инициализирует провайдера требуемыми сервисами
		/// </summary>
		/// <param name="services"></param>
		void Initialize(params object[] services);
		/// <summary>
		/// Метод помещения в хаб первичных (приватных метрик)
		/// </summary>
		/// <param name="hub"></param>
		void CollectHub(MetricHub hub);
		/// <summary>
		/// Метод собственно сбора итоговой статистики
		/// </summary>
		/// <param name="options"></param>
		/// <param name="hub"></param>
		/// <returns></returns>
		IEnumerable<MetricResult> Collect(MetricCollectOptions options, MetricHub hub);
	}
}