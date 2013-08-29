using System.Reflection;
using Qorpent.Applications;
using Qorpent.IO;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Benchmark.Tests {
	public static class ThemaSourceHelper {
		public static IFileSource GetSource(string file = "themas.zip") {
			var stream = Assembly.GetExecutingAssembly().OpenManifestResource(file);
			return Application.Current.Container.Get<IFileSource>("zip.file.source", stream);
		}
	}
}