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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaDetailObject.cs
#endregion
using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	
	public interface IZetaDetailObject : IZetaObject,
		ICanResolveTag,
		IWithDetailObjectType,
		IWithDetailObjects, IWithOuterCode, IEntity,IWithCurrency {
		string Verb { get; set; }
		IZetaDetailObject Parent { get; set; }
		IZetaPoint Location { get; set; }
		string FullName { get; set; }
		bool InverseControl { get; set; }

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