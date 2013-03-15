#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTranFilter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IBizTranFilter : IWithId {
		 int Action { get; set; }

		 int MainId { get; set; }

		 int ContrId { get; set; }

		 string TranCode { get; set; }

		 string Role { get; set; }

		 string Raw { get; set; }

		 string FirstForm { get; set; }

		 string FirstType { get; set; }

		 string SecondType { get; set; }


		 string SecondForm { get; set; }
	}
}