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

namespace Zeta.Extreme.Model {
	public class UserThemaMap : IUsrThemaMap {
		public virtual int Id { get; set; }
		public virtual IZetaUnderwriter Usr { get; set; }
		public virtual IZetaMainObject Object { get; set; }
		public virtual string System { get; set; }
		public virtual string Thema { get; set; }
		public virtual DateTime Version { get; set; }

		public virtual bool IsPlan {
			get { return Thema.EndsWith("_2"); }
		}

		public virtual string ThemaCode {
			get { return Thema.Replace("_2", ""); }
		}
	}
}