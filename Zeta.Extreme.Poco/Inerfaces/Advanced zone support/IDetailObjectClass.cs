#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectClass.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Persistence;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IDetailObjectClass :
		IZoneElement,
		ICanResolveTag,
		IWithDetailObjectTypes,
		ICanCountDetailObjects,
		IEntity {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}