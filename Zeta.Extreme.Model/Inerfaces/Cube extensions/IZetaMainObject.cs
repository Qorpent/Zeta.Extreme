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
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[ForSearch("Старший объект (предприятие)")]
	public interface IZetaMainObject :
		IZetaObject,
		IZetaQueryDimension,
		ICanResolveTag,
		IOlapMainObject<IZetaMainObject, IZetaDetailObject>,
		IWithAddress, IMainObjectLocators,
		IWithDetailObjectType,
		IWithUnderwriters,
		IWithAlternateDetailObjects,
		IWithProperties {
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
		IDetailObjectType ObjType { get; set; }
		IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		[Map(ReadOnly = true)] string Path { get; set; }

		int Level { get; }
		int? DivId { get; set; }

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