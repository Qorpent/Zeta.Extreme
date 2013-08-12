using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IThemaLoader {
		IThemaFactory Factory { get; set; }
		List<XElement> ThemaElements { get; }
		List<XElement> XmlSources { get; }
		IThemaLoader Load(params IThemaSource[] sources);
	}
}