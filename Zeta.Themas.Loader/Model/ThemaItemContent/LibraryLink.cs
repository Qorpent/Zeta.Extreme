using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Model.ThemaItemContent {
	public class LibraryLink : ILibraryLink {
		#region ILibraryLink Members

		public string SourceCode { get; set; }
		public string TargetCode { get; set; }
		public IThemaItem Source { get; set; }
		public IThemaItem Target { get; set; }

		#endregion
	}
}