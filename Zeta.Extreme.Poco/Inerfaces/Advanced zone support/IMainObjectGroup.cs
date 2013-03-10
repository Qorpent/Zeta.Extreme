#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Classic("Holding")]
	[ForSearch("������, ��������, �������")]
	public interface IMainObjectGroup :
		IZoneElement,
		ICanCountObjects,
		IWithMainObjects, IEntity {}
}