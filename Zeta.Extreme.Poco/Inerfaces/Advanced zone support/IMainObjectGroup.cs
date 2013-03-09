#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Classic("Holding")]
	[ForSearch("Группа, дивизион, холдинг")]
	public interface IMainObjectGroup :
		IZoneElement,
		ICanCountObjects,
		IWithMainObjects, IEntity {}
}