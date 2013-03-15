using System.IO;
using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.FormulaStorageTests
{
	[TestFixture]
	public class DllCacheTests
	{
		private string _dir;
		private FormulaStorage _storage;
		private FormulaRequest _request;
		private string[] _files;
		private FormulaRequest _request1;
		private FormulaRequest _request2;
		private FormulaAssemblyCache _cache;

		[SetUp]
		public void Setup() {
			_dir = Path.Combine(Path.GetTempPath(), "DllCacheTests");
			//гарантировать создание и очистку темповой директории для кэша
			Directory.CreateDirectory(_dir);
			Directory.Delete(_dir,true);
			Directory.CreateDirectory(_dir);
			_request = new FormulaRequest {Language = "boo", Formula = "$x?", Key = "row:test:1", Version = "x1"};
			_request1 = new FormulaRequest {Language = "boo", Formula = "$x?", Key = "row:test:2",Version = "x2"};
			_request2 = new FormulaRequest {Language = "boo", Formula = "$x?", Key = "row:test:3", Version= "x3"};
			_storage = new FormulaStorage();
			_storage.Register(_request);
			_storage.CompileAll(_dir);
			_storage.Register(_request1);
			_storage.CompileAll(_dir);
			_storage.Register(_request2);
			_storage.CompileAll(_dir);
			_files = Directory.GetFiles(_dir, "*.dll").OrderBy(File.GetLastWriteTime).ToArray();
			_cache = new FormulaAssemblyCache();
			_cache.Rebuild(_dir);
		}

		[Test]
		public void Internal_Test_Environment_Is_Well() {
			Assert.NotNull(_request.PreparedType);
			Assert.NotNull(_request1.PreparedType);
			Assert.NotNull(_request2.PreparedType);
			Assert.AreEqual(3,_files.Length);
		}

		[Test]
		public void Cache_Can_Validly_Enumerate_Files() {
			var cachedfiles = _cache.GetAssemblyFileNames();
			CollectionAssert.AreEqual(_files,cachedfiles);
		}
		[Test]
		public void Cache_Type_Set_Is_Valid()
		{
			Assert.NotNull(_cache.GetFormulaType(_request.Key));
			Assert.NotNull(_cache.GetFormulaType(_request1.Key));
			Assert.NotNull(_cache.GetFormulaType(_request2.Key));
		}
		

		[Test]
		public void Dont_Load_Older_Files() {
			File.SetLastWriteTime(_files[0],_cache.GetBaseLibraryVersion().AddDays(-1));
			_cache.Rebuild(_dir);
			var corretedFiles = _files.Skip(1).ToArray();
			var cachedfiles = _cache.GetAssemblyFileNames();
			CollectionAssert.AreEqual(corretedFiles, cachedfiles);
			Assert.Null(_cache.GetFormulaType(_request.Key));
			Assert.NotNull(_cache.GetFormulaType(_request1.Key));
			Assert.NotNull(_cache.GetFormulaType(_request2.Key));
		}
	}
}
