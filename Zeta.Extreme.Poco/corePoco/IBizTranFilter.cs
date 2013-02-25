using Comdiv.Model;
using Comdiv.Model.Interfaces;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
	public interface IBizTranFilter :IWithId
	{

		[Map] int Action { get; set; }

		[Map] int MainId { get; set; }

		[Map] int ContrId { get; set; }
		
		[Map]
		string TranCode { get; set; }

		[Map]
		string Role { get; set; }

		[Map]
		string Raw { get; set; }

		[Map]
		string FirstForm { get; set; }

		[Map]
		string FirstType { get; set; }

		[Map]
		string SecondType { get; set; }



		[Map]
		string SecondForm { get; set; }
	}
}