#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaCell.cs
// Project: Zeta.Extreme.Model
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Интерефейс ячейки куба
	/// </summary>
	public interface IZetaCell :
		IWithUsr, IWithId, IWithVersion {
		/// <summary>
		/// Строка
		/// </summary>
		IZetaRow Row { get; set; }
		/// <summary>
		/// Колонка
		/// </summary>
		IZetaColumn Column { get; set; }
		/// <summary>
		/// Старший объект
		/// </summary>
		IZetaMainObject Object { get; set; }
		/// <summary>
		/// Младший объект
		/// </summary>
		IZetaDetailObject Detail { get; set; }
		/// <summary>
		/// Контрагент
		/// </summary>
		IZetaMainObject AltObj { get; set; }
		
		/// <summary>
		/// Период
		/// </summary>
		int Period { get; set; }
		/// <summary>
		/// Год
		/// </summary>
		int Year { get; set; }
		/// <summary>
		/// Прямая дата
		/// </summary>
		DateTime DirectDate { get; set; }

		/// <summary>
		/// Валюта ячейки
		/// </summary>
		string Valuta { get; set; }
		
		/// <summary>
		/// Ид контрагента
		/// </summary>
		int? AltObjId { get; set; }
		/// <summary>
		/// Ид строки
		/// </summary>
		int RowId { get; set; }
		/// <summary>
		/// Ид колонки
		/// </summary>
		int ColumnId { get; set; }
		/// <summary>
		/// Ид объекта
		/// </summary>
		int ObjectId { get; set; }
		/// <summary>
		/// Ид детали
		/// </summary>
		int? DetailId { get; set; }

		/// <summary>
		/// Численное значение
		/// </summary>
		decimal NumericValue { get; set; }
		/// <summary>
		/// Строковое значение
		/// </summary>
		string StringValue { get; set; }
		
		}
}