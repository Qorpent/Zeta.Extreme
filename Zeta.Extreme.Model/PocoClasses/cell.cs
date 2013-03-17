#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : cell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

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
		/// Валюта ячейки
		/// </summary>
		public virtual string Valuta { get; set; }
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