#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	
	
	public interface IMainObjectGroup :
		IZetaObject,
		IWithMainObjects, IEntity, IWithId, IWithCode, IWithName, IWithTag {}
}