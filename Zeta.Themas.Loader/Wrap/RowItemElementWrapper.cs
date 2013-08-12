using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public class RowItemElementWrapper : ThemaItemElementWrapper, IRowItemElementWrapper {
		public RowItemElementWrapper(IRowItemElement target, IThemaItemWrapper item) : base(target, item) {
		}
	}
}