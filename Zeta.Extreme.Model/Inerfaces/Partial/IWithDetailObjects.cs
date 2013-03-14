#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDetailObjects.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IWithDetailObjects {
		IList<IZetaDetailObject> DetailObjects { get; set; }
	}
}