#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IDetailObjectType :
		IZoneElement,
		ICanResolveTag,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithDetailObjectClass,
		ICanCountDetailObjects,
		IEntity {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}