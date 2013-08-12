using Zeta.Themas.Loader.Abstracts;
using Zeta.Themas.Loader.Wrap;

namespace Zeta.Themas.Loader.UI {
	public class utTerminal {
		private IThemaWrapper _wrap;
		public string Idx { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public IThema Target { get; set; }
		public string Usr { get; set; }

		public utThemaRoot Parent { get; set; }

		public IThemaWrapper GetWrap(WrapContext context = null) {
			return _wrap ??
			       (_wrap =
			        new ThemaWrapperFactory(Target.Factory, Usr).WrapThema(Target.Code, context ?? Parent.Group.Tree.Context));
		}
	}
}