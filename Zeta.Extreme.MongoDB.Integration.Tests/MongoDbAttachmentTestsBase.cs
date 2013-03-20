using System;
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
        protected MongoDbAttachmentSource _mdb;
		private int _uid = 0;
		protected MongoDatabase _db;
		/// <summary>
		/// После P13-58 - коллекция для дескрипторов
		/// </summary>
		protected MongoCollection<BsonDocument> _filecollection;
		/// <summary>
		/// После P13-58 - коллекция для чанков
		/// </summary>
		protected MongoCollection<BsonDocument> _blobcollection;
		/// <summary>
		/// После P13-58 - коллекция для индекса
		/// </summary>
		
		protected MongoCollection<BsonDocument> _indexcollection;

		public MongoDbAttachmentTestsBase() {
            _mdb = new MongoDbAttachmentSource
            {
				Database = "MongoDbAttachments"
			};
		}

		/// <summary>
		/// Вспомогательный метод - создать и сохранить с uid и возможно настройками
		/// </summary>
		/// <param name="uid">UID создававемого атача или Null</param>
		/// <param name="setup">опциональная процедура дополнительно настройки атача</param>
		/// <param name="data">опциональный массив данных в качестве блоба</param>
		protected Attachment Create(string uid=null, Action<Attachment> setup = null, byte[] data = null) {
			var attachment = GetNewAttach(uid);
			if(null!=setup) {
				setup(attachment);
			}

			Save(attachment, data);
			return attachment;
		}

		[SetUp]
		public virtual void Setup() {
			_db = new MongoClient().GetServer().GetDatabase(_mdb.Database);
			// По идее MondoDbAS должна использовать имя коллекции как базис для GridFS
			_filecollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".files");
			_blobcollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".chunks");
			_indexcollection = _db.GetCollection<BsonDocument>("system.indexes");
			_filecollection.Drop();
			_blobcollection.Drop();
			//NOTICE: we haven't drop SYSTEM.INDEXES - MongoDB prevent it!!!!

			// Это выражает простую мысль - при работе компоненты должны существовать только эти коллекции
			_filecollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".files");
			_blobcollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".chunks");
			_indexcollection = _db.GetCollection<BsonDocument>("system.indexes");

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