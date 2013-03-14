#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.PocoClasses;


namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Старший объект (предприятие)")]
	public interface IZetaMainObject : IZetaQueryDimension,
		ICanResolveTag,
		IWithDetailObjectType,
		IWithProperties, IWithDetailObjects, IZetaObject {
		[Map] string GroupCache { get; set; }
		[Map] string FullName { get; set; }
		[Map] string Formula { get; set; }
		[Map] string ShortName { get; set; }
		[Map] DateTime Start { get; set; }
		[Map] DateTime Finish { get; set; }
		[Map] string Valuta { get; set; }

		[Map] bool ShowOnStartPage { get; set; }

		IList<IZetaMainObject> Children { get; set; }
		IZetaMainObject Parent { get; set; }
		IObjectType ObjType { get; set; }
		IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		[Map(ReadOnly = true)] string Path { get; set; }

		int Level { get; }
		int? DivId { get; set; }
		[Classic("Holding")] IMainObjectGroup Group { get; set; }
		[Classic("Otrasl")] IMainObjectRole Role { get; set; }
		[Classic("Municipal")] IZetaPoint Location { get; set; }
		string Address { get; set; }
		IList<IZetaUnderwriter> Underwriters { get; set; }

		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		string[] GetConfiguredThemaCodes();
		IUsrThemaMap GetUserMap(string themacode, bool plan);
		IZetaUnderwriter[] GetConfiguredUsers();
		string[] GetConfiguredThemas(IZetaUnderwriter usr, bool plan);
		IEnumerable<IZetaMainObject> AllChildren();
		IEnumerable<IZetaMainObject> AllChildren(int level, string typefilter);
		bool IsMatchZoneAcronim(string s);
		}
}