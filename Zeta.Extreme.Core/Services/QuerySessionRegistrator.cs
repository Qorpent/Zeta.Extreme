#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/QuerySessionRegistrator.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
		public virtual IQuery Register(IQuery srcquery, string uid) {
			var registry = _session.GetRegistry();
			var keymap = _session.GetRegistryKeyMap();

			WriteInitialStatistics(uid);

			var query = srcquery;
			IQuery result;
			var preloadkey = srcquery.GetCacheKey();

			// мы сразу ловим ключ запроса на тот случай, если он уже 
			// был когда либо запрошен, в этом случае мы минуя все 
			// обработки берем запрос из кэша
			if (TryResolveByKeyMap(uid, preloadkey, out result)) {
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
				return result;
			}

			//пытаемся найти в рабочей агенде (ключ-кэш запроса)
			var found = TryGetFromActiveAgenda(key, out result);
			if (!found) {
				// вот теперь понятно, что запрос новый - регистрируем и запускаем
				result = RegisterRequestInAgendaAndStart((IQueryWithProcessing) query, key);
			}
			_session.StatIncRegistryUser();

			// и в завершении прописываем запрос собственно в основных регистраторах
			// пользовательский мапинг uid->запрос
			// и KeyMap для быстрого кэширования

			registry[uid] = result;
			keymap[preloadkey] = uid;

			var processable = result as IQueryWithProcessing;
			if(null!=processable) {
				if (null == processable.PrepareTask && PrepareState.Prepared != processable.PrepareState)
				{
					processable.PrepareState = PrepareState.TaskStarted;
					processable.PrepareTask = _session.PrepareAsync(processable);
				}
			}
			return result;
		}


		private IQuery RegisterRequestInAgendaAndStart(IQuery query, string key) {
			query.Session = _session; //надо установить сессию раз новый запрос
			query = _session.GetRegistryActiveSet().GetOrAdd(key, query);
			var result = query;
			lock (typeof (QuerySessionRegistrator)) {
				result.Uid = ++QUERYID;
			}
			_session.StatIncRegistryNew();

			return result;
		}

		private bool TryGetFromActiveAgenda(string key, out IQuery result) {
			var found = _session.GetRegistryActiveSet().TryGetValue(key, out result);
			_session.StatIncRegistryResolvedByKey();
			return found;
		}

		private bool TryReturnAlreadyRegistered(string uid, out IQuery result) {
			if (_session.GetRegistry().TryGetValue(uid, out result)) {
				_session.StatIncRegistryResolvedByUid();
				return true;
			}
			return false;
		}

		private bool CheckNullQuery(IQuery query) {
			if (null == query) {
				_session.StatIncRegistryIgnored();
				return true;
			}
			return false;
		}

		private IQuery PreprocessQuery(IQuery query) {
			//		lock(ZexSession._register_lock) {
			var preprocessor = _session.GetPreloadProcessor();
			try {
				_session.StatIncRegistryPreprocessed();
				query = preprocessor.Process(query);
			}
			finally {
				_session.Return(preprocessor);
			}
			return query;
		}

		private bool TryResolveByKeyMap(string uid, string preloadkey, out IQuery result) {
			result = null;
			var registry = _session.GetRegistry();
			var keymap = _session.GetRegistryKeyMap();
			string mappedkey;
			if (keymap.TryGetValue(preloadkey, out mappedkey)) {
				_session.StatIncRegistryResolvedByMapKey();
				result = registry[mappedkey];
				if (!string.IsNullOrWhiteSpace(uid) && mappedkey != uid) {
					_session.StatIncRegistryUser();
					registry[uid] = result;
				}
				return true;
			}
			return false;
		}

		private void WriteInitialStatistics(string uid) {
			if (!_stat) {
				return;
			}
			_session.StatIncRegistryStarted();
			if (!string.IsNullOrWhiteSpace(uid)) {
				_session.StatIncRegistryStartedUser();
			}
		}

		private readonly ISessionWithExtendedServices _session;
		private readonly bool _stat;
	}
}