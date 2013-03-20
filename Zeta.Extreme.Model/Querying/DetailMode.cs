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
// PROJECT ORIGIN: Zeta.Extreme.Model/DetailMode.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Тип работы с деталями при входном объекте типа "объект" или агрегатный объект,
	/// 	при работе с деталями напрямую - не имеет сущностного смысла
	/// </summary>
	[Flags]
	public enum DetailMode {
		/// <summary>
		/// 	Неопределенный - система сама выбирает способ
		/// </summary>
		None,

		/// <summary>
		/// 	Максимальная защита объекта, технически - четкая проверка, что DETAIL IS NULL при запросах на объект
		/// </summary>
		SafeObject,

		/// <summary>
		/// 	Объект - как сумма, используется оператор SUM(VALUE), не может использоваться как первичная ячейка для ввода
		/// </summary>
		SumObject,

		/// <summary>
		/// 	То же самое что и SumObject, однако производится жесткий контроль, что DETAIL IS NOT NULL
		/// </summary>
		SafeSumObject,

		/// <summary>
		/// 	Означает что дополнительно используется четкий селектор DETAILTYPE (получаемый из других настроек объекта)
		/// </summary>
		TypedSumObject,
	}
}