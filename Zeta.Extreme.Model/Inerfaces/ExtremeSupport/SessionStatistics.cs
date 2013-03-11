using System;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Инкапсулирует статистические данные по сессии
	/// </summary>
	public class SessionStatistics {
		
		/// <summary>
		/// 	Статистика батчей
		/// </summary>
		public int Stat_Batch_Count;

		/// <summary>
		/// 	Статистика времени батчей
		/// </summary>
		public TimeSpan Stat_Batch_Time;

		/// <summary>
		/// 	Статистика использованных значений
		/// </summary>
		public int Stat_Primary_Affected;

		/// <summary>
		/// 	Статистика возвращеных ячеек
		/// </summary>
		public int Stat_Primary_Catched;

		/// <summary>
		/// 	Счетчик формул
		/// </summary>
		public int Stat_QueryType_Formula;

		/// <summary>
		/// 	Счетчик первичных запросов
		/// </summary>
		public int Stat_QueryType_Primary;

		/// <summary>
		/// 	Счетчик сумм
		/// </summary>
		public int Stat_QueryType_Sum;

		/// <summary>
		/// 	Счетчик игнорируемых запросов
		/// </summary>
		public int Stat_Registry_Ignored;

		/// <summary>
		/// 	Статистика действительно уникальных регистраций
		/// </summary>
		public int Stat_Registry_New;

		/// <summary>
		/// 	Статистика вызовов препроцессора
		/// </summary>
		public int Stat_Registry_Preprocessed;

		/// <summary>
		/// 	Статистика резольвинга по внутреннему ключу
		/// </summary>
		public int Stat_Registry_Resolved_By_Key;

		/// <summary>
		/// 	Статистика количества дублированных запросов без препроцессинга
		/// </summary>
		public int Stat_Registry_Resolved_By_Map_Key;

		/// <summary>
		/// 	Статистика резольвинга по наличию в кэше
		/// </summary>
		public int Stat_Registry_Resolved_By_Uid;

		/// <summary>
		/// 	Статистика количества вызовов регистрации
		/// </summary>
		public int Stat_Registry_Started;

		/// <summary>
		/// 	Статистика пользовтельских регистраций
		/// </summary>
		public int Stat_Registry_Started_User;

		/// <summary>
		/// 	Счетчик результативных клиентских запросов
		/// </summary>
		public int Stat_Registry_User;

		/// <summary>
		/// 	Счетчик переводов строки
		/// </summary>
		public int Stat_Row_Redirections;

		/// <summary>
		/// 	Статистика созданных под-сессий
		/// </summary>
		public int Stat_SubSession_Count;

		/// <summary>
		/// 	Статистика общего времени выполнения
		/// </summary>
		public TimeSpan Stat_Time_Total;
	}
}