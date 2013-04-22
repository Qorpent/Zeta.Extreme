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
// PROJECT ORIGIN: FormStateOperationDenyReasonType.cs

#endregion

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Тип причин для запрета установки статуса
	/// </summary>
	public enum FormStateOperationDenyReasonType {
		/// <summary>
		///     Пользовательский тип
		/// </summary>
		Custom = 0,

		/// <summary>
		///     Программное исключение
		/// </summary>
		Exception = 1,

		/// <summary>
		///     Какая-то общаяошибка безопасности
		/// </summary>
		GenericSecurity = 2,

		/// <summary>
		///     Недостаток требуемой роли
		/// </summary>
		InsufficientRole = 3,

		/// <summary>
		///     Исходный статус формы не подходит
		/// </summary>
		InvalidBaseState = 4,

		/// <summary>
		///     Проблема в контрольных точках
		/// </summary>
		ControlPoint = 5,

		/// <summary>
		///     Проблема в зависимостях от других форм
		/// </summary>
		Dependency = 6,

		/// <summary>
		///     Проблема в зависимости от "шляпы"
		/// </summary>
		FinalDependency = 7,

		/// <summary>
		///     Проблема в присоединенных файлах
		/// </summary>
		Files = 8,

		/// <summary>
		///     Проблема в каком-то унаследованном статусе
		/// </summary>
		Parent = 9,
		/// <summary>
		/// Статус уже выставлен относительно формы
		/// </summary>
		AlreadySet =10,
	}
}