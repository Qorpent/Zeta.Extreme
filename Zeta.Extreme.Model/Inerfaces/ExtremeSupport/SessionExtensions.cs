using System;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {



	/// <summary>
	/// Хелпер для работы с сессиями
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// Вспопомогательный метод определения возможности записи статистики
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static bool IsCollectStatistics(this ISession session) {
			if(null==session) return false;
			var sessionWithCollect = session as IWithSessionStatistics;
			return null != sessionWithCollect && sessionWithCollect.CollectStatistics;
		}
		/// <summary>
		/// Вспомогательный акцессор к статистике сессии
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static SessionStatistics GetStatistics(this ISession session) {
			if(!session.IsCollectStatistics()) throw new ArgumentException("this session cannot support statistics");
			return ((IWithSessionStatistics) session).Statistics;
		}
		/// <summary>
		/// null-safe and type safe accessor to global or session-bound metacache
		/// </summary>
		/// <param name="session"></param>
		/// <returns>resultat is guaranted from nulls</returns>
		public static IMetaCache GetMetaCache (this ISession session) {
			if(null==session) return MetaCache.Default;
			var sessionAsDataSession = session as IWithDataServices;
			if(null==sessionAsDataSession) return MetaCache.Default;
			return sessionAsDataSession.MetaCache ?? MetaCache.Default;
		}
		/// <summary>
		/// Wrapper for session to wait data
		/// </summary>
		/// <param name="session"></param>
		/// <param name="timeout">if session is not IWithDataServices returns emidiatly </param>
		public static void WaitForPrimary(this ISession session, int timeout=-1) {
			if(null==session)return;
			var sessionAsDataSession = session as IWithDataServices;
			if(null==sessionAsDataSession)return;
			sessionAsDataSession.WaitPrimarySource(timeout); 
		}
		/// <summary>
		/// null-safe обертка для запросов с IWithSession
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static ISession GetSession(this IQuery query) {
			if(null==query) return null;
			var queryAsWithSession = query as IWithSession;
			return null==queryAsWithSession ? null : queryAsWithSession.Session;
		}
	}
}