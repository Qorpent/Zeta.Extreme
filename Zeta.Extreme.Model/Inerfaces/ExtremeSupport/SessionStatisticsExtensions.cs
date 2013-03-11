using System;
using System.Threading;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ¬спомогательный класс нивилирующий проблемы сбора статистики сессии
	/// </summary>
	public static class SessionStatisticsExtensions {
		/// <summary>
		/// ”величивает статистику времени выполнени€ SQL
		/// </summary>
		/// <param name="session"></param>
		/// <param name="timespan"></param>
		public static void StatIncStatBatchTime(this ISession session, TimeSpan timespan) {
			if(session.IsCollectStatistics()) {
				session.GetStatistics().Stat_Batch_Time += timespan;
			}
		}

		/// <summary>
		/// ”величивает статистику "отловленных" первичных значений
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryCatched(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref session.GetStatistics().Stat_Primary_Catched);
			}
		}	
		/// <summary>
		/// ”величивает статистику "примененных" первичных значений
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryAffected(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().Stat_Primary_Affected);
			}
		}
		/// <summary>
		/// ”величивает статистику количества выполненных запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncBatchCount(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().Stat_Batch_Count);
			}
		}	
	}
}