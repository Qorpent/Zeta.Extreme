#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IDetailObjectGroup<M, D> :
		IZoneElement,
		IWithDetailGroupLinks<M, D>,
		IEntityDataPattern
		where M : IOlapMainObjectBase where D : IOlapDetailObjectBase<M> {}
}