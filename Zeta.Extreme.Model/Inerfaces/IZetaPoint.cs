#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaPoint.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Точка, город")]
	public interface IZetaPoint :
		IZetaObject,
		IWithMainObjects,
		IEntity {
		IZetaRegion Region { get; set; }
		IList<IZetaDetailObject> DetailObjects { get; set; }
		}
}