#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapDetailObjectBase.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Classic("Subpart")]
	public interface IOlapDetailObjectBase<MainObjectType> :
		IWithMainObject<MainObjectType>,
		IEntity
		where MainObjectType : IOlapMainObjectBase {}
}