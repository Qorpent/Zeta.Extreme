#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces.Bases {
	public interface IOlapMainObject<M, D> :
		IDimension,
		IWithDetailObjects<M, D>,
		IOlapMainObjectBase where M : IOlapMainObjectBase where D : IOlapDetailObject<M, D> {}
}