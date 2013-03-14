using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
	public abstract class MongoDbAttachmentTestsBase {
		protected const string TEST_STRING = "Test OK!";
		protected static readonly byte[] TEST_DATA = Encoding.ASCII.GetBytes(TEST_STRING);
		protected MongoDbAttachmentsSource _mdb;
		private int _uid = 0;
		protected MongoDatabase _db;
		protected MongoCollection<BsonDocument> _collection;

		public MongoDbAttachmentTestsBase() {
			_mdb = new MongoDbAttachmentsSource {
				DatabaseName = "MongoDbAttachmentTests"
			};
		}

		[SetUp]
		public void Setup() {
			_db = new MongoClient().GetServer().GetDatabase(_mdb.DatabaseName);
			_collection = _db.GetCollection<BsonDocument>(_mdb.CollectionName);
			_collection.Drop();
			_collection = _db.GetCollection<BsonDocument>(_mdb.CollectionName);
		}

		public IEnumerable<Attachment> Find(Attachment attachment) {
			return _mdb.Find(attachment).ToArray();
		}

		public void Save(Attachment attachment, byte[] source = null) {
			_mdb.Save(attachment);
			if (null != source) {
				using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
					stream.Write(source, 0, source.Length);
					stream.Flush();
				}
			}
		}

		public void Delete(Attachment attachment) {
			_mdb.Delete(attachment);
		}

		public Stream Open(Attachment attachment, FileAccess mode) {
			return _mdb.Open(attachment, mode);
		}

		public Attachment GetNewAttach(string uid = null) {
			return new Attachment {
				Uid = uid ?? string.Format("{0}{1}", "Attachment", ++_uid),
				Name = string.Format("{0}{1}", "Name", _uid),
				Comment = string.Format("{0}{1}", "Comment", _uid),
				Revision = _uid,
				User = string.Format("{0}{1}", "User", _uid),
				MimeType = string.Format("{0}{1}", "MimeType", _uid),
				Extension = string.Format("{0}{1}", "Extension", _uid),
				Metadata = {
					{
						string.Format("{0}{1}", "m1", _uid), string.Format("{0}{1}", "v1", _uid)
					}, 
					{
						string.Format("{0}{1}", "m2", _uid), string.Format("{0}{1}", "v2", _uid)
					},
					{
						string.Format("{0}{1}", "m3", _uid), string.Format("{0}{1}", "v3", _uid)
					}
				}
			};
		}
	}
}