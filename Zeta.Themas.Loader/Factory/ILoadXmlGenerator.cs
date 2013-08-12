using System.Xml.Linq;
using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Factory {
	public interface ILoadXmlGenerator {
		XElement Generate(XElement sourceElement, IThemaLoader loader);
	}
}