#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : QuerySessionRegistrator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Threading;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация хелпера для регистрации запросов в системе
	/// </summary>
	public class QuerySessionRegistrator : IRegistryHelper {
		private static long QUERYID;

		/// <summary>
		/// 	Конструирует хелпер, в присоединении к сессии
		/// </summary>
		/// <param name="session"> </param>
		public QuerySessionRegistrator(Session session) {
			_session = session;
			_stat = session.CollectStatistics;
		}

		/// <summary>
		/// 	Выполняет регистрацию запроса
		/// 	возвращает запрос, в итоге зарегистрированный в системе
		/// </summary>
		/// <param name="srcquery"> исзодный запрос </param>
		/// <param name="uid"> </param>
		/// <returns> итоговый запрос после регистрации </returns>
		public virtual Query Register(Query srcquery, string uid) {
			//lock(ZexSession._register_lock) {
			WriteInitialStatistics(uid);

			var query = srcquery;
			Query result;
			var preloadkey = srcquery.GetCacheKey();

			// мы сразу ловим ключ запроса на тот случай, если он уже 
			// был когда либо запрошен, в этом случае мы минуя все 
			// обработки берем запрос из кэша
			if (TryResolveByKeyMap(uid, preloadkey, out result)) {
				if (result.Session != _session) {
					result.WaitPrepare();
				}
				return result;
			}

			//теперь фаза препроцессинга - приводим запрос к нормализованной форме
			query = PreprocessQuery(query);
			//проверяем, не отвергнут ли запрос препроцессором
			if (CheckNullQuery(query)) {
				return null;
			}


			//теперь определяем пользовательский и реальный ключ запроса
			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();

			//пытаемся вернуть готовый запрос из общего хранилища (ключ - пользовательский)
			if (TryReturnAlreadyRegistered(uid, out result)) {
				if (result.Session != _session) {
					result.WaitPrepare();
				}
				return result;
			}

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

			_session.Registry[uid] = result;
			_session.KeyMap[preloadkey] = uid;

			lock(result) {
				if (null==result.PrepareTask && PrepareState.Prepared != result.PrepareState) {
					result.PrepareState = PrepareState.TaskStarted;
					result.PrepareTask = _session.PrepareAsync(result);
					
				}
			}
			//if (result.Session != _session) result.WaitPrepare();
			return result;
			//	}
		}

		private void CheckUserRegistryStatistics(string uid, string key) {
			//	lock(ZexSession._register_lock) {
			if (_stat && uid != key) {
				Interlocked.Increment(ref _session.Stat_Registry_User);
			}
			//	}
		}

		private Query RegisterRequestInAgendaAndStart(Query query, string key) {
			//lock(ZexSession._register_lock) {
			Query result;
			query.Session = _session; //надо установить сессию раз новый запрос
			query = _session.ActiveSet.GetOrAdd(key, query);
			result = query;
			lock (typeof (QuerySessionRegistrator)) {
				result.UID = ++QUERYID;
			}
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_Registry_New);
			}
			if (_session.TraceQuery) {
				result.TraceList = result.TraceList ?? new List<string>();
				result.TraceList.Add(Environment.TickCount + " " + _session.Id + " r " + result.UID + " " + result.GetCacheKey());
			}


			return result;
			//	}
		}

		private bool TryGetFromActiveAgenda(string key, out Query result) {
			//	lock(ZexSession._register_lock) {
			bool found;
			found = _session.ActiveSet.TryGetValue(key, out result);
			if (_stat && found) {
				_session.Stat_Registry_Resolved_By_Key++;
			}
			return found;
			//	}
		}

		private bool TryReturnAlreadyRegistered(string uid, out Query result) {
			//	lock(ZexSession._register_lock) {
			if (_session.Registry.TryGetValue(uid, out result)) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Uid);
				}
				{
					return true;
				}
			}
			return false;
			//	}
		}

		private bool CheckNullQuery(Query query) {
			//	lock(ZexSession._register_lock) {
			if (null == query) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Ignored);
				}
				return true;
			}
			return false;
			//	}
		}

		private Query PreprocessQuery(Query query) {
			//		lock(ZexSession._register_lock) {
			var preprocessor = _session.GetPreloadProcessor();
			try {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Preprocessed);
				}
				query = preprocessor.Process(query);
			}
			finally {
				_session.Return(preprocessor);
			}
			return query;
			//		}
		}

		private bool TryResolveByKeyMap(string uid, string preloadkey, out Query result) {
			//		lock(ZexSession._register_lock) {
			result = null;
			string mappedkey;
			if (_session.KeyMap.TryGetValue(preloadkey, out mappedkey)) {
				if (_stat) {
					Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Map_Key);
				}

				result = _session.Registry[mappedkey];
				if (!string.IsNullOrWhiteSpace(uid) && mappedkey != uid) {
					if (_stat) {
						Interlocked.Increment(ref _session.Stat_Registry_User);
					}
					_session.Registry[uid] = result;
				}
				return true;
			}
			return false;
			//	}
		}

		private void WriteInitialStatistics(string uid) {
			//		lock(ZexSession._register_lock) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_Registry_Started);
				if (!string.IsNullOrWhiteSpace(uid)) {
					Interlocked.Increment(ref _session.Stat_Registry_Started_User);
				}
			}
			//		}
		}

		private readonly Session _session;
		private readonly bool _stat;
	}
}