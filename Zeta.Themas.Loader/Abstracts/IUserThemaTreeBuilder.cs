using Zeta.Themas.Loader.Factory;
using Zeta.Themas.Loader.UI;
using Zeta.Themas.Loader.Wrap;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IUserThemaTreeBuilder {
		ThemaFactory Factory { get; set; }
		UserThemaTree BuildTree(string usr = null, WrapContext context = null);
	}
}