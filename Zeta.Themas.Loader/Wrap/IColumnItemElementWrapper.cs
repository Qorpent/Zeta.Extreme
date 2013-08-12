using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public interface IColumnItemElementWrapper : IThemaItemElementWrapper, IColumnItemElement {
		int[] Periods { get; set; }
	}
}