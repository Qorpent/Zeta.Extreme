#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexRegistryHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация хелпера для регистрации запросов в системе
	/// </summary>
	public class DefaultZexRegistryHelper : IZexRegistryHelper {
		/// <summary>
		/// 	Конструирует хелпер, в присоединении к сессии
		/// </summary>
		/// <param name="session"> </param>
		public DefaultZexRegistryHelper(ZexSession session) {
			_session = session;
		}

		/// <summary>
		/// 	Выполняет регистрацию запроса
		/// 	возвращает запрос, в итоге зарегистрированный в системе
		/// </summary>
		/// <param name="srcquery"> исзодный запрос </param>
		/// <param name="uid"> </param>
		/// <returns> итоговый запрос после регистрации </returns>
		public virtual ZexQuery Register(ZexQuery srcquery, string uid) {
			var query = srcquery;

			var preprocessor = _session.GetPreloadProcessor();
			try {
				preprocessor.Process(query);
			}
			finally {
				_session.ReturnPreloadPreprocessor(preprocessor);
			}

			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();
			ZexQuery result = null;


			if (_session.MainQueryRegistry.TryGetValue(uid, out result)) {
				return result;
			}

			var found = false;
			found = _session.ActiveSet.TryGetValue(key, out result);
			if (!found) {
				found = _session.ProcessedSet.TryGetValue(key, out result);
			}
			if (!found) {
				query.Session = _session; //надо установить сессию раз новый запрос
				query = _session.ActiveSet.GetOrAdd(key, query);
				result = query;
			}
			_session.MainQueryRegistry[uid] = result;
			return result;
		}

		private readonly ZexSession _session;
	}
}