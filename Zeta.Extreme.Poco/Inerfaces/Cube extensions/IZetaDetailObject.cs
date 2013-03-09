#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[ForSearch("Младший объект, подразделение, связь")]
	public interface IZetaDetailObject :
		IZetaObject,
		IZoneElement,
		ICanResolveTag,
		IOlapDetailObject<IZetaMainObject, IZetaDetailObject>,
		IWithDetailObjectType,
		IWithAlternateMainObject,
		IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithOwn,
		IWithFixRules,
		IWithDetailObjects<IZetaMainObject, IZetaDetailObject>, IWithOuterCode {
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