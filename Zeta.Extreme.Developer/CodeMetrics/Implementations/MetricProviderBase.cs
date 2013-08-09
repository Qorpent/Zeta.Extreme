using System;
using System.Collections.Generic;
using System.Linq;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.CodeMetrics {
	/// <summary>
	/// Базовый проводник метрик
	/// </summary>
	public abstract class MetricProviderBase : IMetricProvider {
		/// <summary>
		/// 
		/// </summary>
		protected ICodeIndex CodeIndex;

		/// <summary>
		/// 
		/// </summary>
		protected IAnalyzer Analyzer;

		/// <summary>
		/// Текущий хаб
		/// </summary>
		protected MetricHub CurrentHub { get; set; }

		/// <summary>
		/// Инициализирует провайдера требуемыми сервисами
		/// </summary>
		/// <param name="services"></param>
		public void Initialize(params object[] services) {
			CodeIndex = services.OfType<ICodeIndex>().FirstOrDefault();
			Analyzer = services.OfType<IAnalyzer>().FirstOrDefault();
		}

		/// <summary>
		/// Метод помещения в хаб первичных (приватных метрик)
		/// </summary>
		/// <param name="hub"></param>
		public virtual void CollectHub(MetricHub hub){}

		/// <summary>
		/// Метод собственно сбора итоговой статистики
		/// </summary>
		/// <param name="options"></param>
		/// <param name="hub"></param>
		/// <returns></returns>
		public virtual IEnumerable<MetricResult> Collect(MetricCollectOptions options, MetricHub hub) {
			yield break;
		}

		/// <summary>
		/// Метод подготовки типовых численных метрик
		/// </summary>
		/// <param name="data"></param>
		/// <param name="prefix"></param>
		/// <param name="mname"></param>
		/// <param name="get"></param>
		protected void CollectMetric<T>(T[] data, string prefix, string mname, Func<T, int> get) {
			CurrentHub.Set(prefix + "total" + mname, data.Select(get).Sum());
			CurrentHub.Set(prefix + "avg" + mname, data.Select(get).Average());
			CurrentHub.Set(prefix + "max" + mname, data.Select(get).Max());
			CurrentHub.Set(prefix + "min" + mname, data.Select(get).Min());
		}
	}
}