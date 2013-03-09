using System.Collections.Generic;

namespace Comdiv.Olap.Model {
	public interface IWithMarksBase{
		IList<IMarkLinkBase> GetMarkLinks();
		bool IsMarkSeted(string code);
		void RemoveMark(IMark mark);
	}
}