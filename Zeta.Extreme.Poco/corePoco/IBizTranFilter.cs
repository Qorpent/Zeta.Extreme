#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTranFilter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Model.Interfaces;

namespace Zeta.Extreme.Poco {
	public interface IBizTranFilter : IWithId {
		[Deprecated.Map] int Action { get; set; }

		[Deprecated.Map] int MainId { get; set; }

		[Deprecated.Map] int ContrId { get; set; }

		[Deprecated.Map] string TranCode { get; set; }

		[Deprecated.Map] string Role { get; set; }

		[Deprecated.Map] string Raw { get; set; }

		[Deprecated.Map] string FirstForm { get; set; }

		[Deprecated.Map] string FirstType { get; set; }

		[Deprecated.Map] string SecondType { get; set; }


		[Deprecated.Map] string SecondForm { get; set; }
	}
}