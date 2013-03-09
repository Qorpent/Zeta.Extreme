#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDetailObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Comdiv.Olap.Model {
	public interface IWithDetailObjectGroup<M, D> where M : IOlapMainObjectBase where D : IOlapDetailObjectBase<M> {
		IDetailObjectGroup<M, D> DetailObjectGroup { get; set; }
	}
}