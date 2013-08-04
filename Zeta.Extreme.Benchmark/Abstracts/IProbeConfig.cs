using Qorpent.Config;
using Qorpent.Log;
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
		/// <summary>
		/// Журнал
		/// </summary>
		IUserLog Log { get; set; }
		/// <summary>
		/// Код формы
		/// </summary>
		string FormTemplate { get; set; }
		/// <summary>
		/// Ид объекта формы
		/// </summary>
		int FormObj { get; set; }
		/// <summary>
		/// Год формы
		/// </summary>
		int FormYear { get; set; }
		/// <summary>
		/// Период формы
		/// </summary>
		int FormPeriod { get; set; }
	}
}