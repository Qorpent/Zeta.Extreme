using System;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Инкапсулирует статистические данные по сессии
	/// </summary>
	public class SessionStatistics {
		
		/// <summary>
		/// 	Статистика батчей
		/// </summary>
		public int BatchCount;

		/// <summary>
		/// 	Статистика времени батчей
		/// </summary>
		public TimeSpan BatchTime;

		/// <summary>
		/// 	Статистика использованных значений
		/// </summary>
		public int PrimaryAffected;

		/// <summary>
		/// 	Статистика возвращеных ячеек
		/// </summary>
		public int PrimaryCatched;

		/// <summary>
		/// 	Счетчик формул
		/// </summary>
		public int QueryTypeFormula;

		/// <summary>
		/// 	Счетчик первичных запросов
		/// </summary>
		public int QueryTypePrimary;

		/// <summary>
		/// 	Счетчик сумм
		/// </summary>
		public int QueryTypeSum;

		/// <summary>
		/// 	Счетчик игнорируемых запросов
		/// </summary>
		public int RegistryIgnored;

		/// <summary>
		/// 	Статистика действительно уникальных регистраций
		/// </summary>
		public int RegistryNew;

		/// <summary>
		/// 	Статистика вызовов препроцессора
		/// </summary>
		public int RegistryPreprocessed;

		/// <summary>
		/// 	Статистика резольвинга по внутреннему ключу
		/// </summary>
		public int RegistryResolvedByKey;

		/// <summary>
		/// 	Статистика количества дублированных запросов без препроцессинга
		/// </summary>
		public int RegistryResolvedByMapKey;

		/// <summary>
		/// 	Статистика резольвинга по наличию в кэше
		/// </summary>
		public int RegistryResolvedByUid;

		/// <summary>
		/// 	Статистика количества вызовов регистрации
		/// </summary>
		public int RegistryStarted;

		/// <summary>
		/// 	Статистика пользовтельских регистраций
		/// </summary>
		public int RegistryStartedUser;

		/// <summary>
		/// 	Счетчик результативных клиентских запросов
		/// </summary>
		public int RegistryUser;

		/// <summary>
		/// 	Счетчик переводов строки
		/// </summary>
		public int RowRedirections;

	

		/// <summary>
		/// 	Статистика общего времени выполнения
		/// </summary>
		public TimeSpan TimeTotal;
	}
}