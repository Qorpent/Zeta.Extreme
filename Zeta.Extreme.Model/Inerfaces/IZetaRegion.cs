#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaRegion.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Регион")]
	public interface IZetaRegion :
		IZetaObject,
		IWithMainObjects,
		IEntity{
		IList<IZetaPoint> Points { get; set; }
		IZetaZone Zone { get; set; }
		}
}