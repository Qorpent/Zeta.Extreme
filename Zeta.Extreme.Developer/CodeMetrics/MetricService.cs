using System.Collections.Generic;
using System.Linq;
using Qorpent;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.CodeMetrics {
	/// <summary>
	/// Служба сбора метрик
	/// </summary>
	[ContainerComponent(Name="zdev.codemetric.service",ServiceType = typeof(IMetricService))]
	public class MetricService :ServiceBase, IMetricService {
		/// <summary>
		/// Ссылка на индекс кода
		/// </summary>
		[Inject]protected ICodeIndex CodeIndex;
		/// <summary>
		/// Ссылка на службу анализатора
		/// </summary>
		[Inject]protected IAnalyzer Analyzer;
		/// <summary>
		/// Ссылка на массив провайдеров метрик
		/// </summary>
		[Inject] protected IMetricProvider[] MetricProviders;

		/// <summary>
		/// Осуществляет сбор метрик
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public IEnumerable<MetricResult> Collect(MetricCollectOptions options = null) {
			if (!MetricProviders.ToBool()) {
				return new MetricResult[] {};
			}
			var hub = new MetricHub();
			foreach (var mp in MetricProviders) {
				mp.Initialize(CodeIndex,Analyzer);
				mp.CollectHub(hub);
			}
			return MetricProviders.SelectMany(_ => _.Collect(options, hub));
		}
	}
}