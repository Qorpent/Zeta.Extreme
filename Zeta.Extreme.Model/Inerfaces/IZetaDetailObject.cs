#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces {
	
	public interface IZetaDetailObject : IZetaObject,
		ICanResolveTag,
		IWithDetailObjectType,
		IWithDetailObjects, IWithOuterCode, IEntity {
		string Verb { get; set; }
		IZetaDetailObject Parent { get; set; }
		IZetaPoint Location { get; set; }
		string FullName { get; set; }
		bool InverseControl { get; set; }
		string Valuta { get; set; }

		 decimal Number1 { get; set; }

		 decimal Number2 { get; set; }

		 DateTime Start { get; set; }

		 DateTime Finish { get; set; }

		 string Path { get; set; }

		 DateTime Date1 { get; set; }
		 DateTime Date2 { get; set; }
		 IZetaMainObject Object { get; set; }
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		}
}