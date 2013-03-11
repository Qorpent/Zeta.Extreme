#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithAlternateDetailObjects.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithAlternateDetailObjects {
		IList<IZetaDetailObject> AlternateDetailObjects { get; set; }
	}
}