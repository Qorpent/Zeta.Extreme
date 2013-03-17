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
// PROJECT ORIGIN: Zeta.Extreme.Model/UserThemaMap.cs

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Deprecated {
	/// <summary>
	/// Implements IUserBizCaseMap
	/// </summary>
	[Obsolete]
	public class UserBizCaseMap : IUserBizCaseMap {
		/// <summary>
		/// PK ID in database terms
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		/// Referenced user
		/// </summary>
		public virtual IZetaUser User { get; set; }

		/// <summary>
		/// Referenced obj
		/// </summary>
		public virtual IZetaMainObject Object { get; set; }

		/// <summary>
		/// Target system (if multiple)
		/// </summary>
		public virtual string System { get; set; }

		/// <summary>
		/// Code if biz case (thema)
		/// </summary>
		public virtual string BizCaseCode { get; set; }

		/// <summary>
		/// User's or system's time stamp
		/// </summary>
		public virtual DateTime Version { get; set; }

		/// <summary>
		/// Flag that this maping is about plan biz
		/// </summary>
		public virtual bool IsPlan {
			get { return BizCaseCode.EndsWith("_2"); }
		}

		/// <summary>
		/// Converts biz code (???) to thema code
		/// </summary>
		public virtual string ThemaCode {
			get { return BizCaseCode.Replace("_2", ""); }
		}
	}
}