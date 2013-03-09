#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Model {
	public interface IDetailObjectType :
		IZoneElement,
		ICanResolveTag,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>,
		IWithDetailObjectClass,
		ICanCountDetailObjects,
		IEntityDataPattern, IWithIdx {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}