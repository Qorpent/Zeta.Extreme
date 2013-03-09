using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface ICommonLog : IWithId, IWithVersion, IWithCode, IWithUsr{
		string Type { get; set; }
		string Message { get; set; }
	}
}