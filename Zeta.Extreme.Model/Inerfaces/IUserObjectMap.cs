#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IUserObjectMap.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IUserObjectMap :
		IEntity,
		IWithMainObject<IZetaMainObject>,
		IWithAllOrgs {
		string Domain { get; set; }
		string Role { get; set; }
		}
}