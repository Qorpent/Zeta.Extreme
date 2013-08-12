namespace Zeta.Themas.Loader.Abstracts {
	public interface IFormLockDepend {
		string SourceCode { get; set; }
		string TargetCode { get; set; }
		IFormThemaItem Source { get; set; }
		IFormThemaItem Target { get; set; }
	}
}