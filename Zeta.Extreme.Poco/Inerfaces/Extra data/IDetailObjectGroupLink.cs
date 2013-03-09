#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectGroupLink.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IDetailObjectGroupLink<MainObjectType, DetailObjectType> :
		IWithId, IWithVersion,
		IWithDetailObject<DetailObjectType, MainObjectType>,
		IWithDetailObjectGroup<MainObjectType, DetailObjectType>
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType>
		where MainObjectType : IOlapMainObjectBase {}
}