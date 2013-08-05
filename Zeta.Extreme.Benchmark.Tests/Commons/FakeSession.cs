using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark.Tests {
	public class FakeSession : ISession, ISerializableSession
	{
		private QueryResult[] _resultstack;
		private int _current = 0;

		/// <summary>
		/// —оздает сессию с заготовками ответов дл€ запросов
		/// </summary>
		/// <param name="resultstack"></param>
		public FakeSession(params QueryResult[] resultstack) {
			this._resultstack = resultstack ?? new QueryResult[] {};
		}

		public QueryResult Get(string key, int timeout = -1) {
			throw new System.NotImplementedException();
		}

		public IQuery Register(IQuery query, string uid = null) {
			query.Result = GetNextResult();
			return query;
		}

		private QueryResult GetNextResult() {
			if (0 == _resultstack.Length) return new QueryResult(0);
			QueryResult result = null;
			if (_current == _resultstack.Length) {
				_current = 0;
			}
			result = _resultstack[_current];
			_current++;
			return result;
		}

		public Task<IQuery> RegisterAsync(IQuery query, string uid = null) {
			return Task.Run(() => Register(query, uid));
		}

		public void Execute(int timeout = -1) {
			
		}

		public ISessionPropertySource PropertySource {
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		public bool UseSyncPreparation {
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		private object sync = new object();

		public object SerialSync {
			get { return sync; }
		}

		private Task<QueryResult> _task;

		public Task<QueryResult> SerialTask {
			get { return _task; }
			set { _task = value; }
		}
	}
}