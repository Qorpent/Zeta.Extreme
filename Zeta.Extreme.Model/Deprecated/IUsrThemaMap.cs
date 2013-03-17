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
// PROJECT ORIGIN: Zeta.Extreme.Model/IUsrThemaMap.cs
#endregion

using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Deprecated {
	/// <summary>
	/// Maps user to biz case in obj
	/// </summary>
	[Obsolete("must be replaced with roles")]
	public interface IUserBizCaseMap : IWithId, IWithVersion {
		/// <summary>
		/// Referenced obj
		/// </summary>
		IZetaMainObject Object { get; set; }
		/// <summary>
		/// Referenced user
		/// </summary>
		IZetaUser User { get; set; }
		/// <summary>
		/// Target system (if multiple)
		/// </summary>
		string System { get; set; }
		/// <summary>
		/// Code if biz case (thema)
		/// </summary>
		string BizCaseCode { get; set; }
		/// <summary>
		/// Flag that this maping is about plan biz
		/// </summary>
		bool IsPlan { get; }

		/// <summary>
		/// Converts biz code (???) to thema code
		/// </summary>
		string ThemaCode { get; }
	}
}