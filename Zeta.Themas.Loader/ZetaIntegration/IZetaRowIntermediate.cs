namespace Zeta.Themas.Loader.ZetaIntegration {
	public interface IZetaRowIntermediate : IZetaEntityIntermediate {
		string Path { get; }
		string ParentCode { get; }
	}
}