#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : P1358Tests.cs
// Project: Zeta.Extreme.MongoDB.Integration.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using NUnit.Framework;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
	/// <summary>
	/// 	Тесты по тикету P1358 - проблема сохранения не в те коллекции
	/// </summary>
	[TestFixture]
	public class P1358Tests : MongoDbAttachmentTestsBase {
		[Test]
		public void P1358_Only_Valid_Collections_Are_Created_On_Binary_Save()
		{
			Create(data: TEST_DATA);
			TestThatAllCollectionsInDatabaseAreValid();
		}

		[Test]
		public void P1358_Only_Valid_Collections_Are_Created_On_Descriptor_Save()
		{
			Create();
			TestThatAllCollectionsInDatabaseAreValid();
		}




		/// <summary>
		/// 	Перекрываем штатный Setup, так как хотим ГАРАНТИРОВАННО вычистить целевую БД
		/// 	то есть в этих тестах мы полностью гарантировано зачищаем БД от всех коллекций
		/// </summary>
		[SetUp]
		public override void Setup() {
			base.Setup(); //по факту при P13-58 мы имеем недочищенную БД и должны предпринять дополнительные действия
			// надо отобрать те коллекции, которых по идее быть не должно и грохнуть
			var badcollectionNames = (
				                         from c in _db.GetCollectionNames()
				                         where !IsValidCollectionName(c)
				                         select c
			                         );
			foreach (var badcollection in badcollectionNames) {
				_db.DropCollection(badcollection);
			}
		}

		/// <summary>
		/// 	Вспомогательный метод проверки того, что имя коллекции -верное
		/// </summary>
		/// <param name="c"> </param>
		/// <returns> </returns>
		private bool IsValidCollectionName(string c) {
			return c == _filecollection.Name
			       ||
			       c == _blobcollection.Name
			       ||
			       c == _indexcollection.Name;
		}


		/// <summary>
		/// 	Вспомогательный метод проверки того, что все коллекции в базе монго - верные
		/// </summary>
		private void TestThatAllCollectionsInDatabaseAreValid() {
			var wrongCollections = _db.GetCollectionNames().Where(
                collectionName => !IsValidCollectionName(collectionName)
            ).ToList();
			Assert.AreEqual(0,wrongCollections.Count); // there is no collections except chunks, files and indexes
		}

		
	}
}