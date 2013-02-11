using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// Реализует API последовательного, синхронного доступа к сессии
	/// </summary>
	public class SerialSession : ISerialSession {
		private ZexSession _session;

		/// <summary>
		/// Формирует API в привязке к сессии
		/// </summary>
		/// <param name="session"></param>
		public SerialSession(ZexSession session) {
			_session = session;
		}

		/// <summary>
		/// Гарантирует синхронный, последовательный доступ к сессии, вычисляет значение
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public QueryResult Eval(ZexQuery query) {
			lock(_session._sync_serial_access_lock) {
				if(null!=_session._async_serial_acess_task) {
					_session._async_serial_acess_task.Wait();
				}
				var realquery = _session.Register(query);
				if(null==realquery) return new QueryResult();
				if (realquery.Session != _session)
				{
					realquery.WaitPrepare(); //it can be from another session
				}
				_session.Execute();
				return realquery.GetResult() ?? new QueryResult();
			}
		}

		/// <summary>
		/// Гарантирует синхронный, последовательный доступ к сессии, вычисляет значение
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public Task<QueryResult> EvalAsync(ZexQuery query)
		{
			lock (_session._sync_serial_access_lock)
			{
				if (null != _session._async_serial_acess_task)
				{
					_session._async_serial_acess_task.Wait();
				}
				var realquery_ = _session.RegisterAsync(query);
				var task = Task.Run(() =>
					{
						realquery_.Wait();
						var realquery = realquery_.Result;
						if (null == realquery)
						{
							return null;
						}
						if(realquery.Session!=_session) {
							realquery.WaitPrepare(); //it can be from another session
						}
						_session.Execute(); 
						// but here we not worry about another session
						// because GetResult() will cause evaluation anyway
						_session._async_serial_acess_task = null;
						return realquery.GetResult();
					});
				_session._async_serial_acess_task = task;
				return task;

			}
		}
	}
}