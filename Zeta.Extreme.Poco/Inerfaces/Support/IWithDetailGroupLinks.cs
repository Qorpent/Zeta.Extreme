#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithDetailGroupLinks.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Comdiv.Olap.Model {
	public interface IWithDetailGroupLinks<MainObjectType, DetailObjectType>
		where MainObjectType : IOlapMainObjectBase where DetailObjectType : IOlapDetailObjectBase<MainObjectType> {
		IList<IDetailObjectGroupLink<MainObjectType, DetailObjectType>> DetailGroupLinks { get; set; }
		}
}