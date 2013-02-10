#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexRegistryHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация хелпера для регистрации запросов в системе
	/// </summary>
	public class DefaultZexRegistryHelper : IZexRegistryHelper {
		private static long QUERYID;

		/// <summary>
		/// 	Конструирует хелпер, в присоединении к сессии
		/// </summary>
		/// <param name="session"> </param>
		public DefaultZexRegistryHelper(ZexSession session) {
			_session = session;
			this._stat = session.CollectStatistics;
		}

		/// <summary>
		/// 	Выполняет регистрацию запроса
		/// 	возвращает запрос, в итоге зарегистрированный в системе
		/// </summary>
		/// <param name="srcquery"> исзодный запрос </param>
		/// <param name="uid"> </param>
		/// <returns> итоговый запрос после регистрации </returns>
		public virtual ZexQuery Register(ZexQuery srcquery, string uid) {
			WriteInitialStatistics(uid);
			
			var query = srcquery;
			ZexQuery result;
			var preloadkey = srcquery.GetCacheKey();

			// мы сразу ловим ключ запроса на тот случай, если он уже 
			// был когда либо запрошен, в этом случае мы минуя все 
			// обработки берем запрос из кэша
			if (TryResolveByKeyMap(uid, preloadkey, out result)) return result;

			//теперь фаза препроцессинга - приводим запрос к нормализованной форме
			query = PreprocessQuery(query);
			//проверяем, не отвергнут ли запрос препроцессором
			if (CheckNullQuery(query)) return null;


			//теперь определяем пользовательский и реальный ключ запроса
			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();

			//пытаемся вернуть готовый запрос из общего хранилища (ключ - пользовательский)
			if (TryReturnAlreadyRegistered(uid, out result)) return result;

			//пытаемся найти в рабочей агенде (ключ-кэш запроса)
			var found = TryGetFromActiveAgenda(key, out result);
			if (!found) {
				// вот теперь понятно, что запрос новый - регистрируем и запускаем
				result = RegisterRequestInAgendaAndStart(query, key);
			}
			CheckUserRegistryStatistics(uid, key);

			// и в завершении прописываем запрос собственно в основных регистраторах
			// пользовательский мапинг uid->запрос
			// и KeyMap для быстрого кэширования
			_session.MainQueryRegistry[uid] = result;
			_session.KeyMap[preloadkey] = uid;
			return result;
		}

		private void CheckUserRegistryStatistics(string uid, string key) {
			if (_stat && uid != key) {
				Interlocked.Increment(ref _session.Stat_Registry_User);
			}
		}

		private ZexQuery RegisterRequestInAgendaAndStart(ZexQuery query, string key) {
			ZexQuery result;
			query.Session = _session; //надо установить сессию раз новый запрос
			query = _session.ActiveSet.GetOrAdd(key, query);
			result = query;
			lock (typeof (DefaultZexRegistryHelper)) {
				result.UID = ++QUERYID;
			}
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_Registry_New);
			}
			_session.PrepareAsync(result); //инициируем следующую фазу обработки
			return result;
		}

		private bool TryGetFromActiveAgenda(string key, out ZexQuery result) {
			bool found;
			found = _session.ActiveSet.TryGetValue(key, out result);
			if (_stat && found) {
				_session.Stat_Registry_Resolved_By_Key++;
			}
			return found;
		}

		private bool TryReturnAlreadyRegistered(string uid, out ZexQuery result) {
			if (_session.MainQueryRegistry.TryGetValue(uid, out result)) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Uid);
				}
				{
					
					return true;
				}
			}
			return false;
		}

		private bool CheckNullQuery(ZexQuery query) {
			if (null == query) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Ignored);
				}
				return true;
			}
			return false;
		}

		private ZexQuery PreprocessQuery(ZexQuery query) {
			var preprocessor = _session.GetPreloadProcessor();
			try {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Preprocessed);
				}
				query = preprocessor.Process(query);
			}
			finally {
				_session.ReturnPreloadPreprocessor(preprocessor);
			}
			return query;
		}

		private bool TryResolveByKeyMap(string uid, string preloadkey,  out ZexQuery result) {
			result = null;
			string mappedkey;
			if (_session.KeyMap.TryGetValue(preloadkey, out mappedkey)) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Map_Key);
				}

				result = _session.MainQueryRegistry[mappedkey];
				if (!string.IsNullOrWhiteSpace(uid) && mappedkey != uid) {
					if (_stat) {
						Interlocked.Increment(ref _session.Stat_Registry_User);
					}
					_session.MainQueryRegistry[uid] = result;
				}
				return true;
				
			}
			return false;
		}

		private void WriteInitialStatistics(string uid) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_Registry_Started);
				if (!string.IsNullOrWhiteSpace(uid)) {
					Interlocked.Increment(ref _session.Stat_Registry_Started_User);
				}
			}
		}

		private readonly ZexSession _session;
		private readonly bool _stat;
	}
}