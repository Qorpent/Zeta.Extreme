using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IUsrThemaMap : IWithId, IWithVersion{
		IZetaMainObject Object { get; set; }
		IZetaUnderwriter Usr { get; set; }
		string System { get; set; }
		string Thema { get; set; }
		bool IsPlan { get; }
		string ThemaCode { get; }
	}
}