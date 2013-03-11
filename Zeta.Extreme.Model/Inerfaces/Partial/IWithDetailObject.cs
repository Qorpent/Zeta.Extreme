#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithDetailObject<DetailObjectType, MainObjectType>
		where DetailObjectType : IOlapDetailObjectBase<MainObjectType>
		where MainObjectType : IOlapMainObjectBase {
		[Classic("Subpart")] DetailObjectType DetailObject { get; set; }
		}
}