using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public class ObjItemElementWrapper : ThemaItemElementWrapper, IObjItemElementWrapper {
		public ObjItemElementWrapper(IObjItemElement target, IThemaItemWrapper item) : base(target, item) {
		}
	}
}