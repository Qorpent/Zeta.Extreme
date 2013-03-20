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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IMvcBasedInputTemplate.cs
#endregion
using System;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Устарелая часть интерфейса для форм, ориентирующаяся на серверную обработку и MVC
	/// </summary>
	//[Obsolete("не будет больше серверной генерации в таком виде")] 
	public interface IMvcBasedInputTemplate {
		/// <summary>
		/// 	Непонятно что для MVC
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string Controller { get; set; }

		/// <summary>
		/// 	Признак использования нестандартного вида
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] bool IsCustomView { get; }

		/// <summary>
		/// 	Нестандартный пользовательский вид
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string CustomView { get; set; }

		/// <summary>
		/// 	Нестандартный тип контроллера
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string CustomControllerType { get; set; }

		/// <summary>
		/// 	Вид таблицы
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string TableView { get; set; }
	}
}