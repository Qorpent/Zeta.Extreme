using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
	[TestFixture]
    public class MongoDbAttachmentTests : MongoDbAttachmentTestsBase {
		private void TestFileValidlySaved(Attachment attachment, string name) {
            attachment.Name = name;
            Save(attachment);

            var attachtoFind = attachment;
            var testAttach = Find(attachtoFind).FirstOrDefault();


            Assert.NotNull(testAttach);             // Проверим, не NULL ли результат поиска
            Assert.AreEqual(name, testAttach.Name); // Проверим, соответствует ли имя найденного указанному
        }
        [Test]
        public void CanDelete() {
            Attachment attachment = GetNewAttach();

            Save(attachment);                                   // save an attachment 
			var found = Find(attachment).FirstOrDefault();  
			Assert.NotNull(found);
            Delete(attachment);

            found = Find(attachment).FirstOrDefault();      // try to find it
            Assert.Null(found);                                 // compare with null
        }

		[Test]
		public void DeletedFilesCanBeRestored()
		{
			Attachment attachment = GetNewAttach();

			Save(attachment);                                   // save an attachment 
			Delete(attachment);
			var stillExists = _collection.FindOneById(attachment.Uid);
			Assert.NotNull(stillExists);
			Assert.True(stillExists["Deleted"].AsBoolean);
			// compare with null
		}

        [Test]
        public void CanCreateAndSaveAnEmptyAttachment() {
            var attachment = GetNewAttach();

            Save(attachment);
            var found = Find(attachment);

            Assert.NotNull(found);
        }

        [Test]
        public void CanDoubleSave() {
            Save(GetNewAttach("x"), TEST_DATA);
            Save(GetNewAttach("x"), TEST_DATA);
        }

		[Test]
		public void CanCreateAndReturnNewUid() {
			var attach = GetNewAttach();
			attach.Uid = null;
			Save(attach);
			Assert.NotNull(attach.Uid);
			var found = Find(new Attachment {Uid = attach.Uid}).First();
			Assert.AreEqual(attach.Name,found.Name);
		}

        [Test]
        public void CanFind() {
            var attachment = GetNewAttach();
            Find(attachment);
        }

        [Test]
        public void CanConfigure() {
            
        }

        [Test]
        public void CanFindByUid() {
            var attachment1 = new Attachment { Uid = "CanFindByUid1", Name = "tuponame1" };
            var attachment2 = new Attachment { Uid = "CanFindByUid2", Name = "tuponame2" };
            var query = new Attachment { Uid = "CanFindByUid1" };

            Save(attachment1);
            Save(attachment2);

            var result = Find(query);
            Assert.NotNull(result);

            var found = result.FirstOrDefault();


            Assert.AreEqual(1, result.Count());
            Assert.NotNull(found);
            Assert.AreEqual("CanFindByUid1", found.Uid);
            Assert.AreEqual("tuponame1", found.Name);
        }

        [Test]
        public void CanFindByOtherItem() {
            var attachment1 = new Attachment { Uid = "CanFindByOtherItem1", Name = "tuponame1" };
            var attachment2 = new Attachment { Uid = "CanFindByOtherItem2", Name = "tuponame2" };
            var attachment3 = new Attachment { Uid = "CanFindByOtherItem3", Name = "tuponame3" };
            var attachment4 = new Attachment { Uid = "CanFindByOtherItem4", Name = "tuponame3" };

            var query = new Attachment { Name = "tuponame3" };

            Save(attachment1);
            Save(attachment2);
            Save(attachment3);
            Save(attachment4);

            var result = Find(query);
            Assert.NotNull(result);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void CanRewriteProperties() {
            Attachment attachment = GetNewAttach();

            TestFileValidlySaved(attachment, "First Name");
            TestFileValidlySaved(attachment, "Second Name");
            TestFileValidlySaved(attachment, "Third Name");
        }

        [Test]
        public void CanSave() {
            var attachment = GetNewAttach();

            Save(attachment, TEST_DATA);

	        var found = Find(new Attachment {Uid = attachment.Uid}).First();
	        string result;
			using(var s = new StreamReader(Open(found,FileAccess.Read),Encoding.ASCII)) {
				result = s.ReadToEnd();
			}
			Assert.AreEqual(TEST_STRING,result);

			
        }

		[Test]
		public void CanRewriteBinary()
		{
			var attachment = GetNewAttach();

			Save(attachment, TEST_DATA);

			var found = Find(new Attachment { Uid = attachment.Uid }).First();
			using (var s = Open(found, FileAccess.Write))
			{
				s.Write(TEST_DATA,0,TEST_DATA.Length);
				s.Write(TEST_DATA,0,TEST_DATA.Length);
				s.Flush();
			}
			string result;
			using (var s = new StreamReader(Open(found, FileAccess.Read), Encoding.ASCII))
			{
				result = s.ReadToEnd();
			}
			Assert.AreEqual(TEST_STRING+TEST_STRING, result);

		}

        [Test]
        public void CanSaveEmptyFile() {
            var attachment = GetNewAttach();
            Save(attachment);
        }
    }
}