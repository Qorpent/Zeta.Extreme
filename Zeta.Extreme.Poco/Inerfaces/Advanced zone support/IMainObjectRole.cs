#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectRole.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	[ForSearch("Роль, тип, отрасль объекта")]
	public interface IMainObjectRole :
		IZoneElement,
		ICanCountObjects,
		IWithMainObjects, IEntity {
		bool ShowOnStartPage { get; set; }
		}
}