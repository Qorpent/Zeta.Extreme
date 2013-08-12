using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.ExtensionPoints {
	public interface IThemaLoaderExtension {
		int Idx { get; set; }
		void Process(IThemaFactory factory);
	}
}