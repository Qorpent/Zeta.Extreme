
#region LICENSE

// Copyright 2007-2013 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Original file : ../Zeta.Data.Model/RolePolicy_auto.cs
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN VCS

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Security {
	///<summary></summary>
	///<remarks>Poco class implementation for RolePolicy entity</remarks>	
	public partial class RolePolicy: Entity {
		
		
		
		
		
		private int __ObjMult;
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual int ObjMult {
			get {return __ObjMult;}
			set { __ObjMult = value; }
		}
	
		
		
		
		
		private int __SystemMult;
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual int SystemMult {
			get {return __SystemMult;}
			set { __SystemMult = value; }
		}
	
		
		
		
		
		private bool __IsObjRole;
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual bool IsObjRole {
			get {return __IsObjRole;}
			set { __IsObjRole = value; }
		}
	
		
		
		
		
		private bool __IsSystemRole;
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual bool IsSystemRole {
			get {return __IsSystemRole;}
			set { __IsSystemRole = value; }
		}
	
		
		
		
		
		private bool __IsAssignable;
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual bool IsAssignable {
			get {return __IsAssignable;}
			set { __IsAssignable = value; }
		}
	
		
		
		
		
		private string __AssignerRole= "";
		///<summary></summary>
		///<remarks>defined in entity RolePolicy </remarks>
		public virtual string AssignerRole {
			get {return __AssignerRole;}
			set { __AssignerRole = value; }
		}
	
	}
}
		