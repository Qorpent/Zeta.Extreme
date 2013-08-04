using System;
using System.Collections.Generic;
using Qorpent.Config;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Интерфейс результата пробы
	/// </summary>
	public interface IProbeResult : IConfig {
		/// <summary>
		/// Обратная ссылка на пробу
		/// </summary>
		IProbe Probe { get; set; }

		/// <summary>
		/// Полное время выполнения
		/// </summary>
		TimeSpan TotalDuration { get; set; }

		/// <summary>
		/// Подрезультаты
		/// </summary>
		IList<IProbeResult> SubResults { get; set; }
	}
}