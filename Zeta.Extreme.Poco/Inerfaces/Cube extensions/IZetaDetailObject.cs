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
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Model{
    [global::Zeta.Extreme.Poco.Deprecated.ForSearch("Младший объект, подразделение, связь")]
    public interface IZetaDetailObject :
        IZetaObject,
        IZoneElement,
		ICanResolveTag,
        IOlapDetailObject<IZetaMainObject, IZetaDetailObject>,
        IWithMarks<IZetaDetailObject, IZetaDetailObjectMark>,
        IWithDetailObjectType,
        IWithAlternateMainObject,
        IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
        IWithOwn,
        IWithFixRules,
        IWithDetailObjects<IZetaMainObject, IZetaDetailObject>, IWithOuterCode{
        string Verb { get; set; }
        IZetaDetailObject Parent { get; set; }
        IZetaPoint Location { get; set; }
        string FullName { get; set; }
        bool InverseControl { get; set; }
        string Valuta { get; set; }

        [Map]
        decimal Number1 { get; set; }

        [Map]
        decimal Number2 { get; set; }

    	[Map]
    	DateTime Start { get; set; }

    	[Map]
    	DateTime Finish { get; set; }

        [Map(ReadOnly = true)]
        string Path { get; set; }

    	[Map] DateTime Date1 { get; set; }
    	[Map] DateTime Date2 { get; set; }

    	MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
        }
}