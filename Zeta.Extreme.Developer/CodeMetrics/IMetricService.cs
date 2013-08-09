using System.Collections.Generic;

namespace Zeta.Extreme.Developer.CodeMetrics {
	/// <summary>
	/// Сервис расчета метрик
	/// </summary>
	public interface IMetricService {
		/// <summary>
		/// Осуществляет сбор метрик
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		IEnumerable<MetricResult> Collect(MetricCollectOptions options =null);
	}
}