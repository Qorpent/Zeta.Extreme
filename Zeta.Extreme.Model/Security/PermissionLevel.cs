#region LICENSE

// Copyright 2007-2012 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
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
// Solution: Qorpent
// Original file : PermissionLevel.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	Уровень доступа по операции
	/// </summary>
	[Flags]
	public enum PermissionLevel {
		/// <summary>
		/// 	Минимальные полномочия
		/// </summary>
		None = 0,

		/// <summary>
		/// 	На увроне карты, но только на уровне только первичной карты
		/// </summary>
		OrgadminNoSecondary = 1,

		/// <summary>
		/// 	На уровне карты, включая вторичные
		/// </summary>
		OrgadminSecondary = 2,

		/// <summary>
		/// 	На уровне системы при ORGADMIN на первичной карте
		/// </summary>
		OrgadminOwner = 4,

		/// <summary>
		/// 	Требуются системные полномочия SYS_SECURITYMASTER
		/// </summary>
		System = 8,

		/// <summary>
		/// 	Административные полномочия
		/// </summary>
		Admin = 16
	}
}