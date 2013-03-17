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


namespace Zeta.Extreme.Model.Inerfaces {
	
	public interface IZetaMainObject : IZetaQueryDimension,
		ICanResolveTag,
		IWithDetailObjectType,
		IWithProperties, IWithDetailObjects, IZetaObject {
		 string GroupCache { get; set; }
		 string FullName { get; set; }
		 string Formula { get; set; }
		 string ShortName { get; set; }
		 DateTime Start { get; set; }
		 DateTime Finish { get; set; }
		 string Valuta { get; set; }

		 bool ShowOnStartPage { get; set; }

		IList<IZetaMainObject> Children { get; set; }
		IZetaMainObject Parent { get; set; }
		IObjectType ObjType { get; set; }
		IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		 string Path { get; set; }

		int Level { get; }
		int? DivId { get; set; }
		 IMainObjectGroup Group { get; set; }
		 IMainObjectRole Role { get; set; }
		 IZetaPoint Location { get; set; }
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