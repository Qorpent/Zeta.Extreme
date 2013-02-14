#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SerialSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� API �����������������, ����������� ������� � ������
	/// </summary>
	public class SerialSession : ISerialSession {
		/// <summary>
		/// 	��������� API � �������� � ������
		/// </summary>
		/// <param name="session"> </param>
		public SerialSession(Session session) {
			_session = session;
		}

		/// <summary>
		/// 	����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		public QueryResult Eval(Query query, int timeout =-1) {
			lock (_session._sync_serial_access_lock) {
				if (null != _session._async_serial_acess_task) {
					_session._async_serial_acess_task.Wait();
				}
				Query realquery = null;
				
				realquery = _session.Register(query);
				
				if (null == realquery) {
					return new QueryResult();
				}
				if (realquery.Session != _session) {
					realquery.WaitPrepare(timeout); //it can be from another session
				}
				_session.Execute(timeout);
				return realquery.GetResult(timeout) ?? new QueryResult();
			}
		}

		/// <summary>
		/// 	����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public Task<QueryResult> EvalAsync(Query query) {
			lock (_session._sync_serial_access_lock) {
				if (null != _session._async_serial_acess_task) {
					_session._async_serial_acess_task.Wait();
				}
				var realquery_ = _session.RegisterAsync(query);
				var task = Task.Run(() =>
					{
						realquery_.Wait();
						var realquery = realquery_.Result;
						if (null == realquery) {
							return null;
						}
						if (realquery.Session != _session) {
							realquery.WaitPrepare(-1); //it can be from another session
						}
						_session.Execute(-1);
						// but here we not worry about another session
						// because GetResult() will cause evaluation anyway
						_session._async_serial_acess_task = null;
						return realquery.GetResult(-1);
					});
				_session._async_serial_acess_task = task;
				return task;
			}
		}

		/// <summary>
		/// 	���������� ������ �� �������� ������
		/// </summary>
		/// <returns> </returns>
		public Session GetUnderlinedSession() {
			return _session;
		}

		private readonly Session _session;
	}
}