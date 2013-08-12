using Zeta.Themas.Loader.Abstracts;
using Zeta.Themas.Loader.ZetaIntegration;

namespace Zeta.Themas.Loader.Wrap {
	public interface IThemaItemElementWrapper : IThemaItemElement {
		IThemaItemWrapper ItemWrap { get; set; }
		IThemaItemElement Target { get; set; }
		IThemaWrapperFactory Factory { get; }
		IZetaEntityIntermediate ZetaObject { get; }
		bool IsValid();
	}
}