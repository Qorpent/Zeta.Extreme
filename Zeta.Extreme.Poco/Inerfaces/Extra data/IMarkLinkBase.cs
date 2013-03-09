using Comdiv.Model;
using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IMarkLinkBase{
		[NoMap]
		IEntityDataPattern MarkLinkTarget { get; set; }

		[NoMap]
		IMark MarkLinkMark { get; set; }
	}
}