using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IFormState : IWithId, IWithVersion, IWithComment{
		IForm Form { get; set; }
		string State { get; set; }
		string Usr { get; set; }
		IFormState Parent { get; set; }
		string GetReadableState();
	}
}