using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public interface IFormThemaItemWrapper : IThemaItemWrapper {
		IFormThemaItem Form { get; }
	}
}