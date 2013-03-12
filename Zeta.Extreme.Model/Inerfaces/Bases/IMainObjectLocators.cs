#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectLocators.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces.Bases {
	public interface IMainObjectLocators {
		[Classic("Holding")] IMainObjectGroup Group { get; set; }

		[Classic("Otrasl")] IMainObjectRole Role { get; set; }

		[Classic("Municipal")] IZetaPoint Location { get; set; }
	}
}