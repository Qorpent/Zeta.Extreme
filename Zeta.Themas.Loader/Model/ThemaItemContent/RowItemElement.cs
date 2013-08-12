using System.Xml.Linq;
using Zeta.Themas.Loader.Abstracts;
using Zeta.Themas.Loader.Wrap;

namespace Zeta.Themas.Loader.Model.ThemaItemContent {
	public class RowItemElement : ThemaItemElement, IRowItemElement {
		public RowItemElement(XElement e = null) : base(e) {
			Type = "row";
		}

		#region IRowItemElement Members

		public override IThemaItemElementWrapper GetWrapper(IThemaItemWrapper itemwrapper) {
			return new RowItemElementWrapper(this, itemwrapper);
		}

		#endregion
	}
}