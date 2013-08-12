namespace Zeta.Themas.Loader.Abstracts {
	public interface ILibraryLink {
		string SourceCode { get; set; }
		string TargetCode { get; set; }
		IThemaItem Source { get; set; }
		IThemaItem Target { get; set; }
	}
}