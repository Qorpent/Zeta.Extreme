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

using System;
using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Zeta user definition
	/// </summary>
	public interface IZetaUser : IContextEntity {
		/// <summary>
		/// Main login
		/// </summary>
		string Login { get; set; }
		/// <summary>
		/// Alt login
		/// </summary>
		string Login2 { get; set; }
		/// <summary>
		/// s-list of slots of underwrite docxs
		/// </summary>
		string SlotList { get; set; }
		/// <summary>
		/// Normalized lost of slots
		/// </summary>
		IList<string> Slots { get; }
		/// <summary>
		/// s-list of roles given to user
		/// </summary>
		string Roles { get; set; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		 string Documents { get; set; }
		 /// <summary>
		 /// reference to container object
		 /// </summary>
		 IZetaMainObject Object { get; set; }
		 /// <summary>
		 /// marks that user is local admin
		 /// </summary>
		 [Obsolete("need be replaced with special role")]
		 bool IsLocalAdmin { get; set; }
		 /// <summary>
		 /// Occupation of user
		 /// </summary>
		string Occupation { get; set; }
		/// <summary>
		/// Contact info
		/// </summary>
		string Contact { get; set; }
	}
}