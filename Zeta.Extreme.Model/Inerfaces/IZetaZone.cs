#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaZone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Зона, страна")]
	public interface IZetaZone :
		IZoneElement,
		IWithRegions, IEntity {}
}