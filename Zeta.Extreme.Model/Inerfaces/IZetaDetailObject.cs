#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Младший объект, подразделение, связь")]
	public interface IZetaDetailObject : IZoneElement,
		ICanResolveTag,
		IWithDetailObjectType,
		IWithAlternateMainObject,
		IWithOwn,
		IWithDetailObjects, IWithOuterCode, IWithMainObject, IEntity, IWithId, IWithCode, IWithName, IWithTag {
		string Verb { get; set; }
		IZetaDetailObject Parent { get; set; }
		IZetaPoint Location { get; set; }
		string FullName { get; set; }
		bool InverseControl { get; set; }
		string Valuta { get; set; }

		[Map] decimal Number1 { get; set; }

		[Map] decimal Number2 { get; set; }

		[Map] DateTime Start { get; set; }

		[Map] DateTime Finish { get; set; }

		[Map(ReadOnly = true)] string Path { get; set; }

		[Map] DateTime Date1 { get; set; }
		[Map] DateTime Date2 { get; set; }

		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}