#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IDetailObjectGroup<M, D> :
		IZoneElement,
		IEntity
		where M : IOlapMainObjectBase where D : IOlapDetailObjectBase<M> {}
}