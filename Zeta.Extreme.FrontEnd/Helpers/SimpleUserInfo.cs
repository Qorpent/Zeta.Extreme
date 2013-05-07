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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/SimpleUserInfo.cs
#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Упрощенный описатель пользователя
	/// </summary>
	public class SimpleUserInfo {
		/// <summary>
		/// 	Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// 	Имя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Должность
		/// </summary>
		public string Dolzh { get; set; }

		/// <summary>
		/// 	Контактные данные
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// 	Электронная почта
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 	Идентификатор предприятия
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 	Имя предприятия
		/// </summary>
		public string ObjName { get; set; }

		/// <summary>
		/// 	Признак администратора предприятия
		/// </summary>
		public bool IsObjAdmin { get; set; }

		/// <summary>
		/// 	Признак активности пользователя
		/// </summary>
		public bool Active { get; set; }
		/// <summary>
		/// Список слотов
		/// </summary>
		[SerializeNotNullOnly]
		public string[] Slots { get; set; }

		/// <summary>
		/// Перечень ролей объекта
		/// </summary>
		public string[] ObjRoles { get; set; }
		/// <summary>
		/// Перечень системных ролей
		/// </summary>
		public string[] SysRoles { get; set; }
	}
}