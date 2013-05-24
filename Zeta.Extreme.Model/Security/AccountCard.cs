
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
// Original file : ../Zeta.Data.Model/UserCard_auto.cs
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN VCS

#endregion

using System;
using Qorpent.Model;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Security {
	///<summary>Карта доступа к предприятию</summary>
	///<remarks>Poco class implementation for UserCard entity</remarks>	
	public partial class AccountCard: object, IWithId, IWithVersion, IWithActive, IWithDateRange {


		/// <summary>
		/// 
		/// </summary>
		public AccountCard()
		{
			ObjectRole = "";
			ObjectRoles = "";
			SlotList = "";
			Tag = "";
		}
		
		
		private bool __IsSecondary;
		///<summary></summary>
		///<remarks>defined in entity UserCard </remarks>
		public virtual bool IsSecondary {
			get {return __IsSecondary;}
			set { __IsSecondary = value; }
		}
	
		
		
		
		
		private string __ObjectRole= "";
		///<summary>Должность на предприятии</summary>
		///<remarks>defined in entity UserCard </remarks>
		public virtual string ObjectRole {
			get {return __ObjectRole;}
			set { __ObjectRole = value; }
		}
	
		
		
		
		
		private string __ObjectRoles= "";
		///<summary>Закрепленные роли по карте</summary>
		///<remarks>defined in entity UserCard </remarks>
		public virtual string ObjectRoles {
			get {return __ObjectRoles;}
			set { __ObjectRoles = value; }
		}
	
		
		
		
		
		private string __SlotList= "";
		///<summary></summary>
		///<remarks>defined in entity UserCard </remarks>
		public virtual string SlotList {
			get {return __SlotList;}
			set { __SlotList = value; }
		}
	
		
		
		
		public virtual IZetaMainObject Obj {get;set;}

		///<summary>Foreign key for Obj</summary>
		[SerializeNotNullOnly]		
		public virtual int ObjId {get;set;}
	
		
		
		
		///<summary></summary>
		///<remarks>defined in entity UserCard </remarks>
		public virtual Account Account {get;set;}

		///<summary>Foreign key for User</summary>
		[SerializeNotNullOnly]		
		public virtual int UserId {get;set;}
	
		
		
		
		
		private int __Id;
		///<summary></summary>
		///<remarks>defined in basis sys </remarks>
		public virtual int Id {
			get {return __Id;}
			set { __Id = value; }
		}
	
		
		
		
		
		private DateTime __Version= new DateTime(1900,1,1);
		///<summary></summary>
		///<remarks>defined in basis sys </remarks>
		public virtual DateTime Version {
			get {return __Version;}
			set { __Version = value; }
		}
	
		
		
		
		
		private string __ZPkg= "";
		///<summary></summary>
		///<remarks>defined in basis sys </remarks>
		public virtual string ZPkg {
			get {return __ZPkg;}
			set { __ZPkg = value; }
		}
	
		
		
		
		
		private bool __Active;
		///<summary>Явный признак активности сущности</summary>
		///<remarks>defined in basis activity </remarks>
		public virtual bool Active {
			get {return __Active;}
			set { __Active = value; }
		}
	
		
		
		
		
		private DateTime __Start= new DateTime(1900,1,1);
		///<summary>Временной признак начала активности сущности</summary>
		///<remarks>defined in basis activity </remarks>
		public virtual DateTime Start {
			get {return __Start;}
			set { __Start = value; }
		}
	
		
		
		
		
		private DateTime __Finish= new DateTime(1900,1,1);
		///<summary>Временной признак окончания активности сущности</summary>
		///<remarks>defined in basis activity </remarks>
		public virtual DateTime Finish {
			get {return __Finish;}
			set { __Finish = value; }
		}
	
		
		
		
		
		private string __Tag= "";
		///<summary>Отдельное поле тега</summary>
		///<remarks>defined in basis tag </remarks>
		public virtual string Tag {
			get {return __Tag;}
			set { __Tag = value; }
		}
	
	}
}
		