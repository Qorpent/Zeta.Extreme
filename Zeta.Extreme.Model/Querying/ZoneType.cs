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
// PROJECT ORIGIN: Zeta.Extreme.Model/ObjType.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Перечисление типов зон
	/// </summary>
	[Flags]
	public enum ZoneType {
		/// <summary>
		/// 	Не определено
		/// </summary>
		None = 0,

		/// <summary>
		/// 	Старший объект
		/// </summary>
		Obj = 1,

		/// <summary>
		/// 	Младший объект
		/// </summary>
		Detail = 2,

		/// <summary>
		/// 	Синоним Detail
		/// </summary>
		Det = Detail,

		/// <summary>
		/// 	Синоним Detail
		/// </summary>
		Sp = Detail,

		/// <summary>
		/// 	Группа объектов
		/// </summary>
		Grp = 4,

		/// <summary>
		/// 	Синони Grp
		/// </summary>
		Og = Grp,

		/// <summary>
		/// 	Дивизион
		/// </summary>
		Div = 8,

		/// <summary>
		/// 	Синоним DIV
		/// </summary>
		H = Div,

		/// <summary>
		/// 	Неизвестный тип
		/// </summary>
		Unknown = 128
	}
}