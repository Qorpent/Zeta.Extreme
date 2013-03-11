#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithLookup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithLookup {
		string Lookup { get; set; }

		bool IsDynamicLookup { get; set; }
	}
}