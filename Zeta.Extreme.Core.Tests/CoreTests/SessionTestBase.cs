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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/SessionTestBase.cs
#endregion
using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	public class SessionTestBase {
		private static Task loadrowcahe;
		private static bool wascallnhibernate;

		[SetUp]
		public virtual void setup() {
			session = new Session(true);
			_serial = session.AsSerial();
		}

		[TearDown]
		public void teardown() {
			if (null != session) {
				Console.WriteLine(session.GetStatisticString());
			}
		}

		[TestFixtureSetUp]
		public virtual void FixtureSetup() {
			if (!wascallnhibernate) {
				Qorpent.Applications.Application.Current.Container.Register(new BasicComponentDefinition { Lifestyle = Lifestyle.Singleton, ImplementationType = typeof(DatabaseConnectionProvider), ServiceType = typeof(IDatabaseConnectionProvider) });
				Qorpent.Applications.Application.Current.DatabaseConnections.Register(new ConnectionDescriptor{PresereveCleanup=true, ConnectionString = "Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta-test3",Name = "Default"},false);
				Periods.Get(12);
				RowCache.start();
				ColumnCache.Start();
				ObjCache.Start();
				FormulaStorage.Default.AutoBatchCompile = false;
				FormulaStorage.Default.LoadDefaultFormulas(null);
				FormulaStorage.Default.AutoBatchCompile = true;
				ColumnCache.Start();
				wascallnhibernate = true;
			}
		}

		protected ISerialSession _serial;
		protected Session session;
	}
}