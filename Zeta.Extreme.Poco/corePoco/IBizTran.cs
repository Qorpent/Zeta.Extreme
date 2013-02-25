using Comdiv.Model;
using Comdiv.Model.Interfaces;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
	public interface IBizTran: IEntityDataPattern {
		[Map]
		string FirstType { get; set; }

		[Map]
		string FirstRole { get; set; }

		[Map]
		string FirstForm { get; set; }

		[Map]
		string SecondType { get; set; }

		[Map]
		string SecondRole { get; set; }

		[Map]
		string SecondForm { get; set; }
	}
}