// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Model{
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
        Qorpent.Model.IWithProperties{
        [Map]
        string GroupCache { get; set; }
        [Map]
        string FullName { get; set; }
        [Map]
        string Formula { get; set; }
        [Map]
        string ShortName { get; set; }
        [Map]
        DateTime Start { get; set; }
        [Map]
        DateTime Finish { get; set; }
        [Map]
        string Valuta { get; set; }

        [Map]
        bool ShowOnStartPage { get; set; }

    	IList<IZetaMainObject> Children { get; set; }
    	IZetaMainObject Parent { get; set; }
    	IDetailObjectType ObjType { get; set; }
    	IList<IUsrThemaMap> UsrThemaMaps { get; set; }

        [Map(ReadOnly = true)]
        string Path { get; set; }

    	int Level { get; }
	    int? DivId { get; set; }

	    MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system="Default");
    	IList<IZetaObjectGroup> GetGroups();
    	IZetaDetailObject[] GetDetails(string classcode, string typecode);
    	IZetaMainObject[] GetChildren(string classcode, string typecode);
    	string[] GetConfiguredThemaCodes();
    	IUsrThemaMap GetUserMap(string themacode, bool plan);
    	IZetaUnderwriter[] GetConfiguredUsers();
    	string [] GetConfiguredThemas(IZetaUnderwriter usr, bool plan);
    	IEnumerable<IZetaMainObject> AllChildren ();
		IEnumerable<IZetaMainObject> AllChildren(int level,string typefilter);
	    bool IsMatchZoneAcronim(string s);
        }
}