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
	/// 	����������� ���������� ������� ��� ����������� �������� � �������
	/// </summary>
	public class DefaultZexRegistryHelper : IZexRegistryHelper {
		private static long QUERYID;

		/// <summary>
		/// 	������������ ������, � ������������� � ������
		/// </summary>
		/// <param name="session"> </param>
		public DefaultZexRegistryHelper(ZexSession session) {
			_session = session;
			this._stat = session.CollectStatistics;
		}

		/// <summary>
		/// 	��������� ����������� �������
		/// 	���������� ������, � ����� ������������������ � �������
		/// </summary>
		/// <param name="srcquery"> �������� ������ </param>
		/// <param name="uid"> </param>
		/// <returns> �������� ������ ����� ����������� </returns>
		public virtual ZexQuery Register(ZexQuery srcquery, string uid) {
			WriteInitialStatistics(uid);
			
			var query = srcquery;
			ZexQuery result;
			var preloadkey = srcquery.GetCacheKey();

			// �� ����� ����� ���� ������� �� ��� ������, ���� �� ��� 
			// ��� ����� ���� ��������, � ���� ������ �� ����� ��� 
			// ��������� ����� ������ �� ����
			if (TryResolveByKeyMap(uid, preloadkey, out result)) return result;

			//������ ���� �������������� - �������� ������ � ��������������� �����
			query = PreprocessQuery(query);
			//���������, �� ��������� �� ������ ��������������
			if (CheckNullQuery(query)) return null;


			//������ ���������� ���������������� � �������� ���� �������
			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();

			//�������� ������� ������� ������ �� ������ ��������� (���� - ����������������)
			if (TryReturnAlreadyRegistered(uid, out result)) return result;

			//�������� ����� � ������� ������ (����-��� �������)
			var found = TryGetFromActiveAgenda(key, out result);
			if (!found) {
				// ��� ������ �������, ��� ������ ����� - ������������ � ���������
				result = RegisterRequestInAgendaAndStart(query, key);
			}
			CheckUserRegistryStatistics(uid, key);

			// � � ���������� ����������� ������ ���������� � �������� �������������
			// ���������������� ������ uid->������
			// � KeyMap ��� �������� �����������
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
			query.Session = _session; //���� ���������� ������ ��� ����� ������
			query = _session.ActiveSet.GetOrAdd(key, query);
			result = query;
			lock (typeof (DefaultZexRegistryHelper)) {
				result.UID = ++QUERYID;
			}
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_Registry_New);
			}
			_session.PrepareAsync(result); //���������� ��������� ���� ���������
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