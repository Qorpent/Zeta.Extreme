using System;

namespace Comdiv.Zeta.Model {
	[Flags]
	public enum FormStates{
		None = 0,
		Open = 1,
		Closed = 2,
		Accepted = 4,
		Rejected = 8
	}
}