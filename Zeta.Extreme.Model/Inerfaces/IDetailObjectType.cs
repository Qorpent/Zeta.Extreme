#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IDetailObjectType :
		IZoneElement,
		ICanResolveTag,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithDetailObjectClass,
		IEntity {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}