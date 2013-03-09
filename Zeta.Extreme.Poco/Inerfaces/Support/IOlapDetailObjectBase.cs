#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapDetailObjectBase.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Zeta.Extreme.Poco.Deprecated;

namespace Comdiv.Olap.Model {
	[Classic("Subpart")]
	public interface IOlapDetailObjectBase<MainObjectType> :
		IWithMainObject<MainObjectType>,
		IEntityDataPattern
		where MainObjectType : IOlapMainObjectBase {}
}