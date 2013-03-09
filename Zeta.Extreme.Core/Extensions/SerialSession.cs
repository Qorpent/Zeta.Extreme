#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SerialSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Реализует API последовательного, синхронного доступа к сессии
	/// </summary>
	public class SerialSession : ISerialSession {
		/// <summary>
		/// 	Формирует API в привязке к сессии
		/// </summary>
		/// <param name="session"> </param>
		public SerialSession(ISession session) {
			var _serial = session as ISerializableSession;
			if(null==_serial) {
				throw new Exception("only serializable sessions supproted");
			}
			_session = (ISerializableSession)session;
		}

		/// <summary>
		/// 	Гарантирует синхронный, последовательный доступ к сессии, вычисляет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		public QueryResult Eval(IQuery query, int timeout = -1) {
			lock (_session.SerialSync) {
				if (null != _session.SerialTask) {
					_session.SerialTask.Wait();
				}
				Query realquery = null;

				realquery = (Query)_session.Register(query);

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
		/// 	Гарантирует синхронный, последовательный доступ к сессии, вычисляет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public Task<QueryResult> EvalAsync(IQuery query) {
			lock (_session.SerialSync) {
				if (null != _session.SerialTask) {
					_session.SerialTask.Wait();
				}
				var realquery_ = _session.RegisterAsync(query);
				var task = Task.Run(() =>
					{
						realquery_.Wait();
						var realquery = (Query)realquery_.Result;
						if (null == realquery) {
							return null;
						}
						if (realquery.Session != _session) {
							realquery.WaitPrepare(-1); //it can be from another session
						}
						_session.Execute(-1);
						// but here we not worry about another session
						// because GetResult() will cause evaluation anyway
						_session.SerialTask = null;
						return realquery.GetResult(-1);
					});
				_session.SerialTask = task;
				return task;
			}
		}

		/// <summary>
		/// 	Возвращает ссылку на реальную сессию
		/// </summary>
		/// <returns> </returns>
		public ISession GetUnderlinedSession() {
			return _session;
		}

		private readonly ISerializableSession _session;
	}
}