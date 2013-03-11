#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithUnderwriters.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithUnderwriters {
		IList<IZetaUnderwriter> Underwriters { get; set; }
	}
}