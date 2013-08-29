using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thzip
{
	class Program
	{
		static void Main(string[] args) {
			var folder = @"G:\repos\.app\zefst\themas";
			if (0 != args.Length) {
				folder = args[0];
			}
			var file = Path.Combine(folder, "themas.zip");
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var pkg = Package.Open (file, FileMode.Create);
			var files = Directory.GetFiles(folder, "*.xml");
			foreach (var f in files) {
				var uri = new Uri("/"+ Path.GetFileName(f),UriKind.Relative);
				var part = pkg.CreatePart(uri, "text/xml", CompressionOption.Normal);
				using (var sw = new StreamWriter(part.GetStream(FileMode.Create, FileAccess.Write))) {
					sw.Write(File.ReadAllText(f));
					sw.Flush();
				}
			}
			pkg.Flush();
			pkg.Close();
		}
	}
}
