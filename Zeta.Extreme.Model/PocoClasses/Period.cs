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
// PROJECT ORIGIN: Zeta.Extreme.Model/Period.cs
#endregion
using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Имплементация временного периода
	/// </summary>
	public class Period : IPeriod {
		/// <summary>
		/// 	Строка тегов
		/// </summary>
		public virtual string Tag { get; set; }

		/// <summary>
		/// Бизнес ИД, используемый в приложении
		/// </summary>
		public virtual int BizId { get; set; }

		/// <summary>
		/// Категория периода
		/// </summary>
		public virtual string Category { get; set; }

		/// <summary>
		/// Сокращенное имя
		/// </summary>
		public virtual string ShortName { get; set; }

		/// <summary>
		/// Признак периода ВНУТРИ дня
		/// </summary>
		public virtual bool IsDayPeriod { get; set; }

		/// <summary>
		/// Начальная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		public virtual DateTime StartDate { get; set; }

		/// <summary>
		/// Конечная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		public virtual DateTime EndDate { get; set; }

		/// <summary>
		/// Количество месяцев в периоде
		/// </summary>
		public virtual int MonthCount { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		/// 	Признак активности формулы
		/// </summary>
		public virtual bool IsFormula { get; set; }

		/// <summary>
		/// 	Строка формулы
		/// </summary>
		public virtual string Formula { get; set; }


		/// <summary>
		/// 	Строковый уникальный идентификатор
		/// </summary>
		public virtual string Code { get; set; }

		/// <summary>
		/// 	Комментарий
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// 	Целочисленный уникальный идентификатор
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public virtual int Index { get; set; }

		/// <summary>
		/// 	Название/имя
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Название
		/// </summary>
		public virtual DateTime Version { get; set; }
	}
}