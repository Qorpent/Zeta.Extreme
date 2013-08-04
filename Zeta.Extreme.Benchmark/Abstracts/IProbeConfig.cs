using Qorpent.Config;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Интерфейс конфигурации проб
	/// </summary>
	public interface IProbeConfig:IConfig {
		/// <summary>
		/// Текущая контекстная сессия
		/// </summary>
		ISession Session { get; set; }

		/// <summary>
		/// Источник метаданных
		/// </summary>
		IMetaCache MetaCache { get; set; }

		/// <summary>
		/// Источник метаданных
		/// </summary>
		IThemaFactory ThemaFactory { get; set; }
		/// <summary>
		/// Запрос
		/// </summary>
		IQuery Query { get; set; }
	}
}