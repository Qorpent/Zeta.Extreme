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
// PROJECT ORIGIN: FormStateType.cs

#endregion

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Статусы формы
	/// </summary>
	public enum FormStateType {
		/// <summary>
		/// Неопределенный статус
		/// </summary>
		None = 0,
		/// <summary>
		///     Открыта
		/// </summary>
		Open = 1,

		/// <summary>
		///     Закрыта
		/// </summary>
		Closed = 2,

		/// <summary>
		///     Проверена
		/// </summary>
		Checked = 3,

		/// <summary>
		///     Принята (на вырост)
		/// </summary>
		Accepted = 4,

		/// <summary>
		/// Зарезервированный статус
		/// </summary>
		Reserved1 = 5,

		/// <summary>
		/// Зарезервированный статус
		/// </summary>
		
		Reserved2 = 6,
		/// <summary>
		/// Зарезервированный статус
		/// </summary>
		Reserved3 = 7,
		/// <summary>
		/// Неизвестный (при парсе)
		/// </summary>
		Unknown =-1
	}
}