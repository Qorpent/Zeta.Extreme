#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithAlternateDetailObjects.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Comdiv.Zeta.Model {
	public interface IWithAlternateDetailObjects {
		IList<IZetaDetailObject> AlternateDetailObjects { get; set; }
	}
}