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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaUnderwriter.cs
#endregion
using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IZetaUnderwriter :
		IEntity {
		string Login2 { get; set; }
		string SlotList { get; set; }
		IList<string> Slots { get; }
		string Roles { get; set; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		 string Documents { get; set; }

		 IZetaMainObject Object { get; set; }

		bool IsFor(string slot);
		}
}