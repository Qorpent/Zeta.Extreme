#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithAlternateMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithAlternateMainObject {
		IZetaMainObject AltObject { get; set; }
	}
}