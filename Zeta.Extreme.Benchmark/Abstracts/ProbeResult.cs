using System;
using System.Collections.Generic;
using Qorpent.Config;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Общая реализация результата пробы
	/// </summary>
	public class ProbeResult : ConfigBase, IProbeResult {
		/// <summary>
		/// Опция обратной ссылки на пробу
		/// </summary>
		public const string PROBE = "probe";
		/// <summary>
		/// 
		/// </summary>
		public const string TOTALDURATION = "totalduration";
		/// <summary>
		/// 
		/// </summary>
		public const string SUBRESULTS = "subresults";

		/// <summary>
		/// Обратная ссылка на пробу
		/// </summary>
		public IProbe Probe {
			get { return Get<IProbe>(PROBE); }
			set { Set(PROBE, value); }
		}

		/// <summary>
		/// Полное время выполнения
		/// </summary>
		public TimeSpan TotalDuration {
			get { return Get<TimeSpan>(TOTALDURATION); }
			set { Set(TOTALDURATION, value); }
		}

		/// <summary>
		/// Подрезультаты
		/// </summary>
		public IList<IProbeResult> SubResults {
			get { return Get<IList<IProbeResult>>(SUBRESULTS); }
			set { Set(SUBRESULTS, value); }
		}
	}
}