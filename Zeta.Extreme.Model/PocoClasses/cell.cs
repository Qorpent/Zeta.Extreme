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
// PROJECT ORIGIN: Zeta.Extreme.Model/Cell.cs
#endregion
using System;
using System.Globalization;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 
	/// </summary>
	public partial class Cell : IZetaCell {
		/// <summary>
		/// Строка
		/// </summary>
		public virtual IZetaRow Row { get; set; }

		/// <summary>
		/// Колонка
		/// </summary>
		public virtual IZetaColumn Column { get; set; }

		/// <summary>
		/// Старший объект
		/// </summary>
		public virtual IZetaMainObject Object { get; set; }

		/// <summary>
		/// Младший объект
		/// </summary>
		public virtual IZetaDetailObject Detail { get; set; }


		/// <summary>
		///Currency of entity
		/// </summary>
		public virtual string Currency { get; set; }
		/// <summary>
		/// Признак неизменяемой ячейки
		/// </summary>
		public virtual bool Finished { get; set; }

		/// <summary>
		/// 	Целочисленный уникальный идентификатор
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		/// 	Название
		/// </summary>
		public virtual DateTime Version { get; set; }

		/// <summary>
		/// Год
		/// </summary>
		public virtual int Year { get; set; }

		/// <summary>
		/// Период
		/// </summary>
		public virtual int Period { get; set; }

		/// <summary>
		/// Прямая дата
		/// </summary>
		public virtual DateTime DirectDate { get; set; }

		/// <summary>
		/// Численное значение
		/// </summary>
		public decimal NumericValue { get; set; }

		/// <summary>
		/// Строковое значение
		/// </summary>
		public string StringValue { get; set; }

		/// <summary>
		/// Контрагент
		/// </summary>
		public virtual IZetaMainObject AltObj { get; set; }

		/// <summary>
		/// Ид контрагента
		/// </summary>
		public virtual int? AltObjId { get; set; }

		/// <summary>
		/// Ид строки
		/// </summary>
		public virtual int RowId { get; set; }

		/// <summary>
		/// Ид колонки
		/// </summary>
		public virtual int ColumnId { get; set; }

		/// <summary>
		/// Ид объекта
		/// </summary>
		public virtual int ObjectId { get; set; }

		/// <summary>
		/// Ид детали
		/// </summary>
		public virtual int? DetailId { get; set; }
		/// <summary>
		/// Признак автоматически заполненной ячейки
		/// </summary>
		public virtual bool IsAuto { get; set; }
		/// <summary>
		/// Пользователь
		/// </summary>
		public virtual string Usr { get; set; }

		/// <summary>
		/// Простой акцессор до значения
		/// </summary>
		public string Value {
			get { return NumericValue.ToString(CultureInfo.InvariantCulture); }
		}

		public override string ToString() {
			return string.Format("cell:{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}",
			                     Row.Code, Column.Code, Object.Code, null == DetailId ? (object) "" : DetailId.Value, Year, Period,
			                     DirectDate.ToString("yyyy-MM-dd"), Value
				);
		}
	}
}