#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaMainObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Persistence;
using Qorpent.Model;

namespace Comdiv.Zeta.Model {
	[global::Zeta.Extreme.Poco.Deprecated.ForSearch("Старший объект (предприятие)")]
	public interface IZetaMainObject :
		IZetaObject,
		IZetaQueryDimension,
		ICanResolveTag,
		IOlapMainObject<IZetaMainObject, IZetaDetailObject>,
		IWithMarks<IZetaMainObject, IZetaMainObjectMark>,
		IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithAddress, IMainObjectLocators,
		IWithDetailObjectType,
		IWithUnderwriters,
		IWithFixRules,
		IWithAlternateDetailObjects,
		IWithProperties {
		[global::Zeta.Extreme.Poco.Deprecated.Map] string GroupCache { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string FullName { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string Formula { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string ShortName { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] DateTime Start { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] DateTime Finish { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string Valuta { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.Map] bool ShowOnStartPage { get; set; }

		IList<IZetaMainObject> Children { get; set; }
		IZetaMainObject Parent { get; set; }
		IDetailObjectType ObjType { get; set; }
		IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.Map(ReadOnly = true)] string Path { get; set; }

		int Level { get; }
		int? DivId { get; set; }

		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		IList<IZetaObjectGroup> GetGroups();
		IZetaDetailObject[] GetDetails(string classcode, string typecode);
		IZetaMainObject[] GetChildren(string classcode, string typecode);
		string[] GetConfiguredThemaCodes();
		IUsrThemaMap GetUserMap(string themacode, bool plan);
		IZetaUnderwriter[] GetConfiguredUsers();
		string[] GetConfiguredThemas(IZetaUnderwriter usr, bool plan);
		IEnumerable<IZetaMainObject> AllChildren();
		IEnumerable<IZetaMainObject> AllChildren(int level, string typefilter);
		bool IsMatchZoneAcronim(string s);
		}
}