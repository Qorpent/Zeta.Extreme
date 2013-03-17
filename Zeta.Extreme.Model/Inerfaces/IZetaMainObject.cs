#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaMainObject.cs
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