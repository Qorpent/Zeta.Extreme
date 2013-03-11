#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SessionTestBase.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

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