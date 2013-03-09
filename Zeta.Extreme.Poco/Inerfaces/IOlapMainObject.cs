#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Comdiv.Olap.Model {
	public interface IOlapMainObject<M, D> :
		IDimension,
		IWithDetailObjects<M, D>,
		IWithRange,
		IOlapMainObjectBase where M : IOlapMainObjectBase where D : IOlapDetailObject<M, D> {}
}