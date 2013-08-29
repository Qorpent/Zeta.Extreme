using Qorpent.Config;
using Qorpent.Log;
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
		/// Опция запрос
		/// </summary>
		public const string QUERY = "query";
		/// <summary>
		/// Опция запрос
		/// </summary>
		public const string LOG = "log";
		/// <summary>
		/// Опция шаблон формы
		/// </summary>
		public const string FORMTEMPLATE = "formtemplate";
		/// <summary>
		/// Опция объект формы
		/// </summary>
		public const string FORMOBJ = "formobj";
		/// <summary>
		/// Опция год формы
		/// </summary>
		public const string FORMYEAR = "formyear";
		/// <summary>
		/// Опция период формы
		/// </summary>
		public const string FORMPERIOD = "formperiod";

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

		/// <summary>
		/// Источник метаданных
		/// </summary>
		public IQuery Query
		{
			get { return Get<IQuery>(QUERY); }
			set { Set(QUERY, value); }
		}

		/// <summary>
		/// Журнал
		/// </summary>
		public IUserLog Log {
			get { return Get<IUserLog>(LOG); }
			set { Set(LOG, value); }
		}

		/// <summary>
		/// Код формы
		/// </summary>
		public string FormTemplate {
			get { return Get<string>(FORMTEMPLATE); }
			set { Set(FORMTEMPLATE, value); }
		}

		/// <summary>
		/// Ид объекта формы
		/// </summary>
		public int FormObj {
			get { return Get<int>(FORMOBJ); }
			set { Set(FORMOBJ, value); }
		}

		/// <summary>
		/// Год формы
		/// </summary>
		public int FormYear {
			get { return Get<int>(FORMYEAR); }
			set { Set(FORMYEAR, value); }
		}

		/// <summary>
		/// Период формы
		/// </summary>
		public int FormPeriod {
			get { return Get<int>(FORMPERIOD); }
			set { Set(FORMPERIOD, value); }
		}
	}
}