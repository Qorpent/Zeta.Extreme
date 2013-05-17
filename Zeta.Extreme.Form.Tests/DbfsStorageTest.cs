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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/DbfsStorageTest.cs
#endregion
using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.DbfsAttachmentSource;

namespace Zeta.Extreme.Form.Tests
{
	[TestFixture]
	[Ignore("DBFS on SQL not more supported")]
	public class DbfsStorageTest
	{
		private DbfsAttachmentStorage _dbfs;

		[TestFixtureSetUp]
		public void setupfixture() {	
			if(null==Application.Current.DatabaseConnections) {
				Application.Current.Container.Register(new BasicComponentDefinition { Lifestyle = Lifestyle.Singleton, ImplementationType = typeof(DatabaseConnectionProvider), ServiceType = typeof(IDatabaseConnectionProvider) });
			}
			if(!Application.Current.DatabaseConnections.Exists("_dbfs_test")) {
				Application.Current.DatabaseConnections.Register(
					new ConnectionDescriptor
						{
							ConnectionString = "Data Source=assoibdx;Initial Catalog=dbfs;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Min Pool Size=5;Application Name=local-debug",
							Name = "_dbfs_test"
						},
					false
				);
			}
		}


		[SetUp]
		public void setup() {
			_dbfs = new DbfsAttachmentStorage {ConnectionName = "_dbfs_test"};
		}

		[Test]
		public void CanReadAttachments() {
			var attachments =
				_dbfs.Find(
					new FormAttachment { TemplateCode = "balans2011A.in", Year = 2012, Period = 13, ObjId = 352 })
					.ToArray();
			Assert.AreEqual(2, attachments.Length);

		}

		[Test]
		public void CanReadBinary()
		{
			var attachment =
				_dbfs.Find(
					new FormAttachment { TemplateCode = "balansA.in", Year = 2010, Period = 16, ObjId = 538 })
					.First();
			var ms = new MemoryStream();
			using (var s = _dbfs.Open(attachment,FileAccess.Read)) {
				s.CopyTo(ms);
			}
			Assert.AreEqual(ms.Length,attachment.Size);

		}

		[Test]
		public void CanWriteAttachments() {
			var attachment = new FormAttachment {Uid = Guid.NewGuid().ToString(),  Type = "dbfs-test"};
			_dbfs.Save(attachment);
			var testattachment = _dbfs.Find(new Attachment {Uid = attachment.Uid}).First();
			Assert.AreEqual(testattachment.Type,attachment.Type);
			
		}

		[Test]
		public void CanWriteBinary()
		{
			var attachment = new FormAttachment { Uid = Guid.NewGuid().ToString(), Type = "dbfs-test" };
			_dbfs.Save(attachment);
			var data = new byte[] {1, 2, 3, 4, 5};
			using (var s = _dbfs.Open(attachment,FileAccess.Write)) {
				s.Write(data,0,5);
				s.Flush();
			}
			var testattachment = _dbfs.Find(new Attachment { Uid = attachment.Uid }).First();
			Assert.AreEqual(5,testattachment.Size);
			var ms = new MemoryStream();
			using (var s = _dbfs.Open(attachment,FileAccess.Read))
			{
				s.CopyTo(ms);
			}
			Assert.AreEqual(ms.Length, testattachment.Size);
			Assert.AreEqual(1,ms.GetBuffer()[0]);
			Assert.AreEqual(5, ms.GetBuffer()[4]);

		}

		
	}
}
