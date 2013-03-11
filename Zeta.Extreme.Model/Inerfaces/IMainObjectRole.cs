#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectRole.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Роль, тип, отрасль объекта")]
	public interface IMainObjectRole :
		IZoneElement,
		IWithMainObjects, IEntity {
		bool ShowOnStartPage { get; set; }
		}
}