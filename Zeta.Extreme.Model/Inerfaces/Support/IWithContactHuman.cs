#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithContactHuman.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithContactHuman {
		bool Boss { get; set; }
		bool Worker { get; set; }
		string Dolzh { get; set; }
		string Contact { get; set; }
	}
}