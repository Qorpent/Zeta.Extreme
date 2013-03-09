#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithOlapObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Comdiv.Olap.Model {
	public interface IWithOlapObject<MainObjectType, DetailObjectType> :
		IWithMainObject<MainObjectType>,
		IWithDetailObject<DetailObjectType, MainObjectType>
		where MainObjectType : IOlapMainObjectBase
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType> {}
}