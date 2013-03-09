#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithRegions.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithRegions {
		IList<IZetaRegion> Regions { get; set; }
	}
}