#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithZone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithZone {
		IZetaZone Zone { get; set; }
	}
}