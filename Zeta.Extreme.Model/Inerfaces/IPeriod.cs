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
// PROJECT ORIGIN: Zeta.Extreme.Model/IPeriod.cs
#endregion
using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Интерфейс периода
	/// </summary>
	public interface IPeriod : IEntity, IWithFormula {
		/// <summary>
		/// Бизнес ИД, используемый в приложении
		/// </summary>
		int BizId { get; set; }
		/// <summary>
		/// Категория периода
		/// </summary>
		string Category { get; set; }
		/// <summary>
		/// Сокращенное имя
		/// </summary>
		string ShortName { get; set; }
		/// <summary>
		/// Признак периода ВНУТРИ дня
		/// </summary>
		bool IsDayPeriod { get; set; }
		/// <summary>
		/// Начальная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		DateTime StartDate { get; set; }
		/// <summary>
		/// Конечная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		DateTime EndDate { get; set; }
		/// <summary>
		/// Количество месяцев в периоде
		/// </summary>
		int MonthCount { get; set; }
	}
}