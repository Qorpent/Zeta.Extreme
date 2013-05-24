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
// Original file : PolicyResult.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	Результат запроса о соответствии политике
	/// </summary>
	public class PolicyResult {
		/// <summary>
		/// 	True - политика выполнена
		/// </summary>
		public bool IsAllowed { get; set; }

		/// <summary>
		/// 	Сообщение об ошибке
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 	Код ошибки
		/// </summary>
		public string ErrorCode { get; set; }

		/// <summary>
		/// 	Уровень доступа
		/// </summary>
		public PermissionLevel Level { get; set; }

		/// <summary>
		/// 	Целевой объект
		/// </summary>
		public Obj Object { get; set; }

		/// <summary>
		/// 	Текущий пользователь
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// 	Текущая карточка
		/// </summary>
		public AccountCard Card { get; set; }
	}
}