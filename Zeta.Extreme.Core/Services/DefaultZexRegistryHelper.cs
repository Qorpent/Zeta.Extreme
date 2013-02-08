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

			lock (_session.MainQueryRegistry) {
				if (_session.MainQueryRegistry.ContainsKey(uid)) {
					return _session.MainQueryRegistry[uid];
				}
			}

			lock (_session.ActiveSet) {
				if (_session.ActiveSet.ContainsKey(key)) {
					result = _session.ActiveSet[key];
				}
			}
			if (null == result) {
				lock (_session.ProcessedSet) {
					if (_session.ProcessedSet.ContainsKey(key)) {
						result = _session.ProcessedSet[key];
					}
				}
			}
			if (null == result) {
				lock (_session.ActiveSet) {
					_session.ActiveSet[key] = query;
					result = query;
				}
			}
			lock (_session.MainQueryRegistry) {
				_session.MainQueryRegistry[uid] = result;
			}


			return result;
		}

		private readonly ZexSession _session;
	}
}