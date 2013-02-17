using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comdiv.Model.Interfaces;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta
{
	[TestFixture]
	public abstract class PureZetaTestFixtureBase
	{
		public Session _session;

		[SetUp]
		public virtual void Setup() {
			_session = new Session();
			var _ps = _session.PrimarySource as DefaultPrimarySource;
			_ps.DoNotExecuteRealSql = true;
			_ps.StubDataGenerator = StubDataGenerator;
			_metacache = new MetaCache();
			_session.MetaCache = _metacache;
			_serial = _session.AsSerial();
			primarycallscount = 0;
			_storedqueries = (BuildModel() ?? new Query[]{}).ToArray();
		}

		/// <summary>
		/// строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<Query> BuildModel();
		/// <summary>
		/// Выполняет полученные ранее из модели запросы
		/// </summary>
		/// <param name="queries"></param>
		protected virtual IEnumerable<Query> Execute(Query[] queries ) {
			foreach (var query in queries) {
				query.Result = _serial.Eval(query);
				yield return query;
			}
		}

		[Test]
		public virtual void MainTest() {
			_examenlist = (Execute(_storedqueries) ?? new Query[]{}).ToArray();
			Examinate(_examenlist);
		}

		protected virtual void Examinate(Query[] queries) {
			IList<Query> bads = new List<Query>();
			foreach (var query in queries) {
				try {
					Examinate(query);
					
				}catch(Exception e) {
					bads.Add(query);
					Console.WriteLine(query);
					Console.WriteLine(e);
				}
			}
			if(0!=bads.Count) {
				Console.WriteLine("что-то не так с запросами:");
				foreach (var query in bads) {
					Console.WriteLine(query+"::"+query.Result);
				}
				Assert.Fail("ошибки, выявленные при проверке");
			}
		}

		protected abstract void Examinate(Query query);

		protected IDictionary<string, decimal> _key_to_value_pseudosql_storage = new Dictionary<string, decimal>();
		private MetaCache _metacache;

		protected void Add(IEntityDataPattern item) {
			_metacache.Set(item);
		}

		protected void Add(Query q, decimal value) {
			_key_to_value_pseudosql_storage[q.GetCacheKey()] = value;
		}

		protected int primarycallscount = 0;
		protected Query[] _storedqueries;
		protected Query[] _examenlist;
		protected ISerialSession _serial;


		protected QueryResult StubDataGenerator(Query query) {
			primarycallscount++;
			var result = CustomStub(query);
			if(null!=result) return result;
			if(_key_to_value_pseudosql_storage.ContainsKey(query.GetCacheKey())) {
				return new QueryResult(_key_to_value_pseudosql_storage[query.GetCacheKey()]);
			}
			
			return new QueryResult{IsComplete = false, Error =new Exception("no data provided with db")};
		}

		protected virtual QueryResult CustomStub(Query query) {
			return null;
		}
	}
}
