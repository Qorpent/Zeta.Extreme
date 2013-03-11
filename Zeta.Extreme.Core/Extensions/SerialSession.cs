#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SerialSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� API �����������������, ����������� ������� � ������
	/// </summary>
	public class SerialSession : ISerialSession {
		/// <summary>
		/// 	��������� API � �������� � ������
		/// </summary>
		/// <param name="session"> </param>
		public SerialSession(ISession session) {
			var serial = session as ISerializableSession;
			if (null == serial) {
				throw new Exception("only serializable sessions supproted");
			}
			_session = (ISerializableSession) session;
		}

		/// <summary>
		/// 	����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		public QueryResult Eval(IQuery query, int timeout = -1) {
			lock (_session.SerialSync) {
				if (null != _session.SerialTask) {
					_session.SerialTask.Wait();
				}

				var realquery = (Query) _session.Register(query);

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
		public Task<QueryResult> EvalAsync(IQuery query) {
			lock (_session.SerialSync) {
				if (null != _session.SerialTask) {
					_session.SerialTask.Wait();
				}
				var queryRegister = _session.RegisterAsync(query);
				var task = Task.Run(() =>
					{
						queryRegister.Wait();
						var realquery = (Query) queryRegister.Result;
						if (null == realquery) {
							return null;
						}
						if (realquery.Session != _session) {
							realquery.WaitPrepare(); //it can be from another session
						}
						_session.Execute();
						// but here we not worry about another session
						// because GetResult() will cause evaluation anyway
						_session.SerialTask = null;
						return realquery.GetResult();
					});
				_session.SerialTask = task;
				return task;
			}
		}

		/// <summary>
		/// 	���������� ������ �� �������� ������
		/// </summary>
		/// <returns> </returns>
		public ISession GetUnderlinedSession() {
			return _session;
		}

		private readonly ISerializableSession _session;
	}
}