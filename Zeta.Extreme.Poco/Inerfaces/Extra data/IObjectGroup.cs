#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Model;

namespace Comdiv.Olap.Model {
	public interface IObjectGroup<M> :
		IZoneElement,
		IWithMainObjects,
		IEntityDataPattern where M : IOlapMainObjectBase {}
}