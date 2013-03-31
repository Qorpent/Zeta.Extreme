using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests
{
    /// <summary>
    /// Ôèêñòóðà ïðîâåðÿåò íàñêîëüêî âåðíî ôîðìèðóåòñÿ ïàòòåðí ïîèñêà äëÿ àòà÷åé ïî îáðàçöó
    /// </summary>
    [TestFixture]
    public class MongoDbAttachmentSourceTestsP1362FindPattern : MongoDbAttachmentSourceTestsBase
    {

        [TestCase("Comment", "comment", null)]
        [TestCase("Name", "filename", null)]
        [TestCase("Uid", "_id", null)]
        public void OneElementPatternNoMetadata(string propetyname, string elementname, string value)
        {
            if (string.IsNullOrWhiteSpace(elementname)) elementname = propetyname;
            if (string.IsNullOrWhiteSpace(value)) value = "TESTVALUE";
            var attachment = new Attachment();
            var property = attachment.GetType().GetProperty(propetyname);
            property.SetValue(attachment, value);
            var searchobject = ConvertToSearchDocument(attachment);
            Assert.AreEqual(false, searchobject["deleted"].AsBoolean);
            Assert.AreEqual(2, searchobject.ElementCount);
            Assert.AreEqual(elementname, searchobject.Elements.First().Name);
            Assert.AreEqual(value, searchobject.Elements.First().Value.AsString);
        }

        [TestCase("one")]
        [TestCase("one,two")]
        [TestCase("one,two,three")]
        public void CorrectMetadataSearch(string metaNames)
        {
            var attachment = new Attachment();
            var names = metaNames.Split(',');
            foreach (var name in names)
            {
                attachment.Metadata[name] = name + "_value";
            }
            var searchobject = ConvertToSearchDocument(attachment);
            Assert.AreEqual(names.Length, searchobject.ElementCount - 1); // because "deleted" auto-added


            foreach (var name in names)
            {
                Assert.NotNull(searchobject.Elements.FirstOrDefault(_ => _.Name == "metadata." + name && _.Value == name + "_value"));
            }
        }

        private BsonDocument ConvertToSearchDocument(Attachment attachment)
        {
            return MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);
        }

        [Test]
        public void CanFindByMetadata()
        {
            var attachment1 = new Attachment { Uid = "1" };
            var attachment2 = new Attachment { Uid = "2" };
            var attachment3 = new Attachment { Uid = "3" };
            attachment1.Metadata["x"] = 1;
            attachment2.Metadata["x"] = 2;
            attachment3.Metadata["x"] = 3;
            attachment1.Metadata["y"] = 1;
            attachment2.Metadata["y"] = 1;
            attachment3.Metadata["y"] = 3;
            Save(attachment1);
            Save(attachment2);
            Save(attachment3);
            //search 1
            var search = new Attachment();
            search.Metadata["x"] = 2;
            var result = _mdb.Find(search).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("2", result.First().Uid);
            //search 2
            search = new Attachment();
            search.Metadata["y"] = 1;
            result = _mdb.Find(search).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.NotNull(result.FirstOrDefault(_ => _.Uid == "1"));
            Assert.NotNull(result.FirstOrDefault(_ => _.Uid == "2"));
        }
    }
}