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
		/// <summary>
		/// 	������������ ������, � ������������� � ������
		/// </summary>
		/// <param name="session"> </param>
		public DefaultZexRegistryHelper(ZexSession session) {
			_session = session;
		}

		/// <summary>
		/// 	��������� ����������� �������
		/// 	���������� ������, � ����� ������������������ � �������
		/// </summary>
		/// <param name="srcquery"> �������� ������ </param>
		/// <param name="uid"> </param>
		/// <returns> �������� ������ ����� ����������� </returns>
		public virtual ZexQuery Register(ZexQuery srcquery, string uid) {
			var stat = _session.CollectStatistics;
			if(stat)System.Threading.Interlocked.Increment(ref _session.Stat_Registry_Started);
			var query = srcquery;
			ZexQuery result = null;

			var preloadkey = srcquery.GetCacheKey();
			string mappedkey;
			if(_session.KeyMap.TryGetValue(preloadkey, out mappedkey)) {
				if (stat)System.Threading.Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Map_Key);
			
				result = _session.MainQueryRegistry[mappedkey];
				if(!string.IsNullOrWhiteSpace(uid) &&mappedkey!=uid) {
					_session.MainQueryRegistry[uid] = result;
				}
				return result;
			}
			

			var preprocessor = _session.GetPreloadProcessor();
			try {
				if (stat) Interlocked.Increment(ref _session.Stat_Registry_Preprocessed);
				query = preprocessor.Process(query);
				
			}
			finally {
				_session.ReturnPreloadPreprocessor(preprocessor);
			}

			if(null==query) {
				if (stat) System.Threading.Interlocked.Increment(ref _session.Stat_Registry_Ignored);
				return null;
			}

			if (string.IsNullOrWhiteSpace(uid)) {
				uid = query.GetCacheKey();
			}
			var key = query.GetCacheKey();
			


			if (_session.MainQueryRegistry.TryGetValue(uid, out result)) {
				if (stat) System.Threading.Interlocked.Increment(ref _session.Stat_Registry_Resolved_By_Uid);
				return result;
			}

			var found = false;
			found = _session.ActiveSet.TryGetValue(key, out result);
			if (!found) {
				found = _session.ProcessedSet.TryGetValue(key, out result);
			}
			if (stat && found) _session.Stat_Registry_Resolved_By_Key++;
			if (!found) {
				query.Session = _session; //���� ���������� ������ ��� ����� ������
				query = _session.ActiveSet.GetOrAdd(key, query);
				result = query;
				if (stat) System.Threading.Interlocked.Increment(ref _session.Stat_Registry_New);
			}
			_session.MainQueryRegistry[uid] = result;
			_session.KeyMap[preloadkey] = uid;
			return result;
		}

		private readonly ZexSession _session;
	}
}