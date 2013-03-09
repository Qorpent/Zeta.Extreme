#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaPoint.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	[ForSearch("Точка, город")]
	public interface IZetaPoint :
		IZoneElement,
		IWithMainObjects,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithRegion,
		ICanCountObjects,
		IEntity {}
}