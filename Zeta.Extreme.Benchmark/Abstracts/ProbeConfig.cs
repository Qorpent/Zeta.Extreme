using Qorpent.Config;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Конфигурация для проб Zeta.Extreme
	/// </summary>
	public class ProbeConfig : ConfigBase, IProbeConfig {
		/// <summary>
		/// Опция текущей сессии
		/// </summary>
		public const string SESSION = "session";
		/// <summary>
		/// Опция мета-кэша
		/// </summary>
		public const string METACACHE = "metacache";
		/// <summary>
		/// Опция фабрики тем
		/// </summary>
		public const string THEMAFACTORY = "themafactory";

		/// <summary>
		/// Текущая контекстная сессия
		/// </summary>
		public ISession Session {
			get { return Get<ISession>(SESSION); }
			set { Set(SESSION, value); }
		}

		/// <summary>
		/// Источник метаданных
		/// </summary>
		public IMetaCache MetaCache
		{
			get { return Get<IMetaCache>(METACACHE); }
			set { Set(METACACHE, value); }
		}

		/// <summary>
		/// Источник метаданных
		/// </summary>
		public IThemaFactory ThemaFactory
		{
			get { return Get<IThemaFactory>(THEMAFACTORY); }
			set { Set(THEMAFACTORY, value); }
		}
		
		
	}
}