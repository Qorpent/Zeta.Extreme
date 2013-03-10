#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaRegion.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[ForSearch("Регион")]
	public interface IZetaRegion :
		IZoneElement,
		IWithMainObjects,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithPoints,
		IWithZone,
		ICanCountObjects,
		IEntity {}
}