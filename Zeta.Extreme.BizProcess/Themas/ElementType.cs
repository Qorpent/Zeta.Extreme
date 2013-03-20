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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/ElementType.cs
#endregion
using System;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Тип элемента темы
	/// </summary>
	[Flags]
	public enum ElementType {
		/// <summary>
		/// 	Неопределенный
		/// </summary>
		None = 0,

		/// <summary>
		/// 	Формы
		/// </summary>
		Form = 1,

		/// <summary>
		/// 	Отчеты
		/// </summary>
		Report = 2,

		/// <summary>
		/// 	Документы
		/// </summary>
		Document = 4,

		/// <summary>
		/// 	Команды
		/// </summary>
		Command = 8,

		/// <summary>
		/// 	Пользовательские нестандартные элементы
		/// </summary>
		Custom = 16,

		/// <summary>
		/// 	Шоткат для описания всех элементов
		/// </summary>
		All = Form | Report | Document | Command | Custom,
	}
}