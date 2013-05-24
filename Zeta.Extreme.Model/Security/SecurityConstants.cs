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
// Original file : SecurityConstants.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	Константы безопасности
	/// </summary>
	public static class SecurityConstants {
		/// <summary>
		/// 	Маска - проверка валидности пользователя
		/// </summary>
		public const string USER_LOGIN_MASK = @"^[\w_\.\d\-]+\\[\w_\.\d\-]+$";

		/// <summary>
		/// 	System administrative role with full access to security
		/// </summary>
		public const string SYS_SECURITYMASTER_ROLE = "SYS_SECURITYMASTER";

		/// <summary>
		/// 	Role for local organization administrator
		/// </summary>
		public const string ORGADMIN_ROLE = "ORGADMIN";

		/// <summary>
		/// 	Special sytem proxy user that keep template info
		/// </summary>
		public const string ZETASYS_INTERNAL_TEMPLATE_USER = @"ZETASYS_INTERNAL\TEMPLATE";

		/// <summary>
		/// 	Имя строки подключения к БД по безопасности ZETA
		/// </summary>
		public const string DEFAULT_ZETA_SECURITY_CONNECTION_NAME = "zeta_security_db";
	}
}