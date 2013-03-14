#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaZone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Зона, страна")]
	public interface IZetaZone :
		IZetaObject, IEntity, IWithId, IWithCode, IWithName, IWithTag {
		IList<IZetaRegion> Regions { get; set; }
		}
}