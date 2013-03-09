#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;

namespace Comdiv.Olap.Model {
	public interface IWithMainObject<MainObjectType> where MainObjectType : IOlapMainObjectBase {
		[Classic("Org")] MainObjectType Object { get; set; }
	}
}