#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTranFilter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IBizTranFilter : IWithId {
		[Map] int Action { get; set; }

		[Map] int MainId { get; set; }

		[Map] int ContrId { get; set; }

		[Map] string TranCode { get; set; }

		[Map] string Role { get; set; }

		[Map] string Raw { get; set; }

		[Map] string FirstForm { get; set; }

		[Map] string FirstType { get; set; }

		[Map] string SecondType { get; set; }


		[Map] string SecondForm { get; set; }
	}
}