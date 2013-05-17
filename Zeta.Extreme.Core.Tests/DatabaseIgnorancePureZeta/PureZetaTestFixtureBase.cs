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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/PureZetaTestFixtureBase.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Qorpent.Model;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

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
		protected virtual IEnumerable<Query> Execute(IQuery[] queries ) {
			foreach (var query in queries) {
				((Query) query).IgnoreCheckPrimaryExistence = true;
				var _q = (Query)_session.Register(query);
				_q.Result = _serial.Eval(_q);
				yield return _q;
			}
		}

		[Test]
		public virtual void MainTest() {
			_examenlist = (Execute(_storedqueries) ?? new Query[]{}).ToArray();
			Examinate(_examenlist);
		}

		protected virtual void Examinate(IQuery[] queries) {
			IList<Query> bads = new List<Query>();
			foreach (var query in queries) {
				try {
					Examinate((Query) query);
					
				}catch(Exception e) {
					bads.Add((Query) query);
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

		protected void Add(IEntity item) {
			_metacache.Set(item);
		}

		protected void Add(IQuery q, decimal value) {
			var subq = q.Copy(true);
			subq.Normalize(_session);
			_key_to_value_pseudosql_storage[subq.GetCacheKey()] = value;
		}

		protected int primarycallscount = 0;
		protected IQuery[] _storedqueries;
		protected IQuery[] _examenlist;
		protected ISerialSession _serial;


		protected QueryResult StubDataGenerator(IQuery query) {
			primarycallscount++;
			var result = CustomStub(query);
			if(null!=result) return result;
			if(_key_to_value_pseudosql_storage.ContainsKey(query.GetCacheKey())) {
				return new QueryResult(_key_to_value_pseudosql_storage[query.GetCacheKey()]);
			}
			
			return new QueryResult{IsComplete = false, Error =new Exception("no data provided with db")};
		}

		protected virtual QueryResult CustomStub(IQuery query) {
			return null;
		}
	}
}
