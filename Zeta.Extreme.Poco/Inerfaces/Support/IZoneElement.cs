#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZoneElement.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IZoneElement : IWithId, IWithCode, IWithName, IWithNewTags {}
}