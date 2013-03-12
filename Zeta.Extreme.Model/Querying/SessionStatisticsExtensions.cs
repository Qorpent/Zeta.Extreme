using System;
using System.Threading;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Вспомогательный класс нивилирующий проблемы сбора статистики сессии
	/// </summary>
	public static class SessionStatisticsExtensions {
		/// <summary>
		/// Увеличивает статистику времени выполнения SQL
		/// </summary>
		/// <param name="session"></param>
		/// <param name="timespan"></param>
		public static void StatIncStatBatchTime(this ISession session, TimeSpan timespan) {
			if(session.IsCollectStatistics()) {
				session.GetStatistics().BatchTime += timespan;
			}
		}

		/// <summary>
		/// Увеличивает статистику "отловленных" первичных значений
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryCatched(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref session.GetStatistics().PrimaryCatched);
			}
		}	
		/// <summary>
		/// Увеличивает статистику "примененных" первичных значений
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryAffected(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().PrimaryAffected);
			}
		}
		/// <summary>
		/// Увеличивает статистику количества выполненных запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncBatchCount(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().BatchCount);
			}
		}

		/// <summary>
		/// Увеличивает статистику количества формульных запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypeFormula(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypeFormula);
			}
		}

		/// <summary>
		/// Увеличивает статистику количества первичных  запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypePrimary(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypePrimary);
			}
		}

		/// <summary>
		/// Увеличивает статистику количества суммовых  запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypeSum(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypeSum);
			}
		}
		/// <summary>
		/// Увеличивает статистику проигнорированных запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryIgnored(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryIgnored);
			}
		}
		/// <summary>
		/// Увеличивает статистику уникальных запросов
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryNew(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryNew);
			}
		}
		/// <summary>
		/// Увеличивает статистику вызовов препроцессора
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryPreprocessed(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryPreprocessed);
			}
		}
		/// <summary>
		/// Увеличивает статистику резолюций запросов по внутреннему кэш-ключу
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByKey(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByKey);
			}
		}
		/// <summary>
		/// Увеличивает статистику резолюций запросов по быстрому мапингу
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByMapKey(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByMapKey);
			}
		}

		/// <summary>
		/// Увеличивает статистику резолюций запросов по начальному ключу
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByUid(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByUid);
			}
		}
		/// <summary>
		/// Увеличивает статистику обращений на регистрацию
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryStarted(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryStarted);
			}
		}
		/// <summary>
		/// Увеличивает статистику пользовательских обращений на регистрацию
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryStartedUser(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryStartedUser);
			}
		}

		/// <summary>
		/// Увеличивает статистику пользовательских обращений на регистрацию
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryUser(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryUser);
			}
		}

		/// <summary>
		/// Количество переводов строк
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRowRedirections(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RowRedirections);
			}
		}

		/// <summary>
		/// Количество переводов строк
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncTimeTotal(this ISession session,TimeSpan span)
		{
			if (session.IsCollectStatistics()) {
				session.GetStatistics().TimeTotal += span;
			}
		}



	}
}