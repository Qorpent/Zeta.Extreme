using System.Xml.Linq;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IElementBasis {
		string Role { get; set; }
		string Code { get; set; }
		string Name { get; set; }
		XElement XmlSource { get; set; }
		bool Authorized(string usr = null);
	}
}