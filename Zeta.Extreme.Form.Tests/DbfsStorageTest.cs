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
