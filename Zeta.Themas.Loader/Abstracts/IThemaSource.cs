using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IThemaSource {
		IEnumerable<XElement> GetXmlSources(IThemaFactory factory);
	}
}