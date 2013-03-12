#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDetailObjects.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Zeta.Extreme.Model.Inerfaces.Bases;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithDetailObjects<M, D> where M : IOlapMainObjectBase where D : IOlapDetailObject<M, D> {
		IList<D> DetailObjects { get; set; }
	}
}