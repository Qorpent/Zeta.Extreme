#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectRole.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Comdiv.Zeta.Model {
	[ForSearch("Роль, тип, отрасль объекта")]
	public interface IMainObjectRole :
		IZoneElement,
		ICanCountObjects,
		IWithIdx,
		IWithMainObjects, IEntityDataPattern {
		bool ShowOnStartPage { get; set; }
		}
}