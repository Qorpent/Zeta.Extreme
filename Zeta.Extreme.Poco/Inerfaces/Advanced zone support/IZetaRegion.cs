#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaRegion.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Comdiv.Zeta.Model {
	[ForSearch("Регион")]
	public interface IZetaRegion :
		IZoneElement,
		IWithMainObjects,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithPoints,
		IWithZone,
		ICanCountObjects,
		IWithIdx,
		IEntityDataPattern {}
}