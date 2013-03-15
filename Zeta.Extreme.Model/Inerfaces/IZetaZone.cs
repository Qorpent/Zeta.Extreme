#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaZone.cs
// Project: Zeta.Extreme.Model
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IZetaZone :
		IZetaObject, IEntity {
		IList<IZetaRegion> Regions { get; set; }
		}
}