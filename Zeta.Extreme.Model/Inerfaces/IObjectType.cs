#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IObjectType :
		IZetaObject,
		ICanResolveTag,
		IWithDetailObjects,
		IEntity, IWithId, IWithCode, IWithName, IWithTag {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		IDetailObjectClass Class { get; set; }
		}
}