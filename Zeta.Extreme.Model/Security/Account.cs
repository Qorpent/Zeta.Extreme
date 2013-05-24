
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
// Original file : ../Zeta.Data.Model/User_auto.cs
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN VCS

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Qorpent.Serialization;

namespace Zeta.Extreme.Model.Security {
	///<summary></summary>
	///<remarks>Poco class implementation for User entity</remarks>	
	public partial class Account: Entity, IContextEntity, IWithAsFileInfo {
		
		
		
		
		
		private string __AdvLogins= "";
		///<summary></summary>
		///<remarks>defined in entity User </remarks>
		public virtual string AdvLogins {
			get {return __AdvLogins;}
			set { __AdvLogins = value; }
		}
	
		
		
		
		
		private string __SysRoles= "";
		///<summary></summary>
		///<remarks>defined in entity User </remarks>
		public virtual string SysRoles {
			get {return __SysRoles;}
			set { __SysRoles = value; }
		}
	
		
		
		
		
		private string __Contact= "";
		///<summary></summary>
		///<remarks>defined in entity User </remarks>
		public virtual string Contact {
			get {return __Contact;}
			set { __Contact = value; }
		}
	
		
		
		
		
		private bool __IsProtected;
		///<summary></summary>
		///<remarks>defined in entity User </remarks>
		public virtual bool IsProtected {
			get {return __IsProtected;}
			set { __IsProtected = value; }
		}
	
		
		///<summary></summary>
		///<remarks>defined in entity User </remarks>
    
    
    [Serialize]  
    
		public virtual ICollection<AccountCard> UserCards {get;set;}
	
		
		
		
		
		private bool __Active;
		///<summary>Явный признак активности сущности</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual bool Active {
			get {return __Active;}
			set { __Active = value; }
		}
	
		
		
		
		
		private DateTime __Start= new DateTime(1900,1,1);
		///<summary>Временной признак начала активности сущности</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual DateTime Start {
			get {return __Start;}
			set { __Start = value; }
		}
	
		
		
		
		
		private DateTime __Finish= new DateTime(1900,1,1);
		///<summary>Временной признак окончания активности сущности</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual DateTime Finish {
			get {return __Finish;}
			set { __Finish = value; }
		}
	
		
		
		
		
		private DateTime __Created= new DateTime(1900,1,1);
		///<summary>Дата создания</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual DateTime Created {
			get {return __Created;}
			set { __Created = value; }
		}
	
		
		
		
		
		private string __Owner= "";
		///<summary>Создатель/владелец</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual string Owner {
			get {return __Owner;}
			set { __Owner = value; }
		}
	
		
		
		
		
		private DateTime __Updated= new DateTime(1900,1,1);
		///<summary>Дата последней правки</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual DateTime Updated {
			get {return __Updated;}
			set { __Updated = value; }
		}
	
		
		
		
		
		private string __Updater= "";
		///<summary>Автор полсдней правки</summary>
		///<remarks>defined in basis contexted </remarks>
		public virtual string Updater {
			get {return __Updater;}
			set { __Updater = value; }
		}
	
	}
}
		