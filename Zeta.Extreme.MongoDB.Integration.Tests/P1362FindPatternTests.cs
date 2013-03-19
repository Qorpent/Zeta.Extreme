using System.Linq;
using MongoDB.Bson;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
	/// <summary>
	/// Фикстура проверяет насколько верно формируется паттерн поиска для атачей по образцу
	/// </summary>
	[TestFixture]
	public class P1362FindPatternTests {

		[TestCase("Comment",null,null)]
		[TestCase("Name", "Filename",null)]
		[TestCase("Uid","_id",null)]
		public void OneElementPatternNoMetadata(string propetyname, string elementname, string value) {
			if(string.IsNullOrWhiteSpace(elementname)) elementname = propetyname;
			if(string.IsNullOrWhiteSpace(value)) value = "TESTVALUE";
			var attachment = new Attachment();
			var property = attachment.GetType().GetProperty(propetyname);
			property.SetValue(attachment,value);
			var searchobject = ConvertToSearchDocument(attachment);
			Assert.AreEqual(1,searchobject.ElementCount);
			Assert.AreEqual(elementname,searchobject.Elements.First().Name);
			Assert.AreEqual(value, searchobject.Elements.First().Value.AsString);
		}

		[TestCase("one")]
		[TestCase("one,two")]
		[TestCase("one,two,three")]
		public void CorrectMetadataSearch(string metaNames) {
			var attachment = new Attachment();
			var names = metaNames.Split(',');
			foreach (var name in names) {
				attachment.Metadata[name] = name+"_value";
			}
			var searchobject = ConvertToSearchDocument(attachment);
			Assert.AreEqual(names.Length, searchobject.ElementCount - 1); // because "deleted" auto-added


			foreach (var name in names) {
				Assert.NotNull(searchobject.Elements.FirstOrDefault(_=>_.Name=="metadata."+name && _.Value==name+"_value"));
			}
		}

		private BsonDocument ConvertToSearchDocument(Attachment attachment) {
            return MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);
		}
	}
}