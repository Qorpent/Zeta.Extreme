#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[Classic("Holding")]
	[ForSearch("Группа, дивизион, холдинг")]
	public interface IMainObjectGroup :
		IZetaObject,
		IWithMainObjects, IEntity, IWithId, IWithCode, IWithName, IWithTag {}
}