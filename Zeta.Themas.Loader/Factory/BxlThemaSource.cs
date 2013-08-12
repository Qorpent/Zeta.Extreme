using System.Collections.Generic;
using System.Xml.Linq;
using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Factory {
	public class BxlThemaSource : IThemaSource {
		private readonly string bxl;

		public BxlThemaSource(string bxl) {
			this.bxl = bxl;
		}

		#region IThemaSource Members

		public IEnumerable<XElement> GetXmlSources(IThemaFactory factory) {
			yield return new BxlXmlParser().Parse(bxl, "direct");
		}

		#endregion
	}
}