#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QuerySessionRegistrator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	����������� ���������� ������� ��� ����������� �������� � �������
	/// </summary>
	public class QuerySessionRegistrator : IRegistryHelper {
		private static long QUERYID;

		/// <summary>
		/// 	������������ ������, � ������������� � ������
		/// </summary>
		/// <param name="session"> </param>
		public QuerySessionRegistrator(Session session) {
			_session = session;
			_stat = session.CollectStatistics;
		}

		/// <summary>
		/// 	��������� ����������� �������
		/// 	���������� ������, � ����� ������������������ � �������
		/// </summary>
		/// <param name="srcquery"> �������� ������ </param>
		/// <param name="uid"> </param>
		/// <returns> �������� ������ ����� ����������� </returns>
		public virtual IQuery Register(IQuery srcquery, string uid) {
			var registry = _session.GetRegistry();
			var keymap = _session.GetRegistryKeyMap();

			WriteInitialStatistics(uid);

			var query = srcquery;
			IQueryWithProcessing result;
			var preloadkey = srcquery.GetCacheKey();

			// �� ����� ����� ���� ������� �� ��� ������, ���� �� ��� 
			// ��� ����� ���� ��������, � ���� ������ �� ����� ��� 
			// ��������� ����� ������ �� ����
			if (TryResolveByKeyMap(uid, preloadkey, out result)) {
				return result;
			}

			//������ ���� �������������� - �������� ������ � ��������������� �����
			query = PreprocessQuery(query);
			//���������, �� ��������� �� ������ ��������������
			if (CheckNullQuery(query)) {
				return null;
			}


			//������ ���������� ���������������� � �������� ���� �������
			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();

			//�������� ������� ������� ������ �� ������ ��������� (���� - ����������������)
			if (TryReturnAlreadyRegistered(uid, out result)) {
				return result;
			}

			//�������� ����� � ������� ������ (����-��� �������)
			var found = TryGetFromActiveAgenda(key, out result);
			if (!found) {
				// ��� ������ �������, ��� ������ ����� - ������������ � ���������
				result = RegisterRequestInAgendaAndStart((IQueryWithProcessing) query, key);
			}
			_session.StatIncRegistryUser();

			// � � ���������� ����������� ������ ���������� � �������� �������������
			// ���������������� ������ uid->������
			// � KeyMap ��� �������� �����������

			registry[uid] = result;
			keymap[preloadkey] = uid;


			if (null == result.PrepareTask && PrepareState.Prepared != result.PrepareState) {
				result.PrepareState = PrepareState.TaskStarted;
				result.PrepareTask = _session.PrepareAsync(result);
			}
			return result;
		}


		private IQueryWithProcessing RegisterRequestInAgendaAndStart(IQueryWithProcessing query, string key) {
			query.Session = _session; //���� ���������� ������ ��� ����� ������
			query = _session.GetRegistryActiveSet().GetOrAdd(key, query);
			var result = query;
			lock (typeof (QuerySessionRegistrator)) {
				result.Uid = ++QUERYID;
			}
			_session.StatIncRegistryNew();

			return result;
		}

		private bool TryGetFromActiveAgenda(string key, out IQueryWithProcessing result) {
			var found = _session.GetRegistryActiveSet().TryGetValue(key, out result);
			_session.StatIncRegistryResolvedByKey();
			return found;
		}

		private bool TryReturnAlreadyRegistered(string uid, out IQueryWithProcessing result) {
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

		private bool TryResolveByKeyMap(string uid, string preloadkey, out IQueryWithProcessing result) {
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