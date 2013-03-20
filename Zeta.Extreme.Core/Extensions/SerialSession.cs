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
// PROJECT ORIGIN: Zeta.Extreme.Core/SerialSession.cs
#endregion
using System;
using System.Threading;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
			var serial = session as ISerializableSession;
			if (null == serial) {
				throw new Exception("only serializable sessions supproted");
			}
			_session = (ISerializableSession) session;
		}

		/// <summary>
		/// 	Гарантирует синхронный, последовательный доступ к сессии, вычисляет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		public QueryResult Eval(IQuery query, int timeout = -1) {
			lock (_session.SerialSync) {
				Thread nativeThread = null;
				if (0 >= timeout) {
					return DoEval(query, -1,out nativeThread);
				}
				else {
					
					var task = Task.Run(()=>DoEval(query, -1,out nativeThread));
					var complete = task.Wait(timeout);
					if (!complete) {

						nativeThread.Abort();
						throw new Exception("timeout");
						
					}
					return task.Result;
				}
			}
		}

		private QueryResult DoEval(IQuery query, int timeout, out Thread currentThread) {
			currentThread = Thread.CurrentThread;
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
		/// 	Возвращает ссылку на реальную сессию
		/// </summary>
		/// <returns> </returns>
		public ISession GetUnderlinedSession() {
			return _session;
		}

		private readonly ISerializableSession _session;
	}
}