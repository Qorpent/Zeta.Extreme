using System.Xml.Linq;
using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Model.Thema {
	public class ThemaLink : IThemaLink {
		#region IThemaLink Members

		public string Type { get; set; }
		public string SourceCode { get; set; }
		public string TargetCode { get; set; }
		public IThema Source { get; set; }
		public IThema Target { get; set; }

		public XElement XmlSource { get; set; }

		#endregion
	}
}