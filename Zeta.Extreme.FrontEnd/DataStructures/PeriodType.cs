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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/PeriodType.cs
#endregion
using System;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Тип периода
	/// </summary>
	[Flags]
	public enum PeriodType {
		/// <summary>
		/// 	Неопределенный
		/// </summary>
		None,

		/// <summary>
		/// 	Месяц
		/// </summary>
		Month,

		/// <summary>
		/// 	Основные периоды с начала года
		/// </summary>
		FromYearStartMain,

		/// <summary>
		/// 	Дополнительные периоды с начала года
		/// </summary>
		FromYearStartExt,

		/// <summary>
		/// 	Плановые периоды
		/// </summary>
		Plan,

		/// <summary>
		/// 	Плановые периоды (месячные)
		/// </summary>
		MonthPlan,

		/// <summary>
		/// 	Коррективы
		/// </summary>
		Corrective,

		/// <summary>
		/// 	Ожидаемые периоды
		/// </summary>
		Awaited,

		/// <summary>
		/// 	Период в середине года
		/// </summary>
		InYear,

		/// <summary>
		/// 	Дополнительные периоды
		/// </summary>
		Ext,
	}
}