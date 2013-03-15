#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTran.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IBizTran : IEntity {
		 string FirstType { get; set; }

		 string FirstRole { get; set; }

		 string FirstForm { get; set; }

		 string SecondType { get; set; }

		 string SecondRole { get; set; }

		 string SecondForm { get; set; }
	}
}