#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailObjectClass.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IDetailObjectClass :
		IZetaObject,
		ICanResolveTag,
		IEntity, IWithId, IWithCode, IWithName, IWithTag {
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		IList<IObjectType> Types { get; set; }
		}
}