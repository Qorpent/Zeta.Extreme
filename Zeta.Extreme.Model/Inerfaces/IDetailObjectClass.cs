#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectClass.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IDetailObjectClass :
		IZoneElement,
		ICanResolveTag,
		IWithDetailObjectTypes,
		IEntity {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}