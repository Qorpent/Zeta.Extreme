#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveStage.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Статус сохранения
	/// </summary>
	[Flags]
	public enum SaveStage {
		/// <summary>
		/// 	Изначальное состояние
		/// </summary>
		None,

		/// <summary>
		/// 	Загрузка задачи сохранения
		/// </summary>
		Load,

		/// <summary>
		/// 	Проверка возможности сохранения по аспектам безопасности
		/// </summary>
		Authorize,

		/// <summary>
		/// 	Подготовка входных данных - переработка справочников
		/// </summary>
		Prepare,

		/// <summary>
		/// 	Проверка целостности запрошенного сохранения
		/// </summary>
		Validate,

		/// <summary>
		/// 	Проверка доступности соединений, хранимых процедур и проч
		/// </summary>
		Test,

		/// <summary>
		/// 	Собственно сохранение ячеек
		/// </summary>
		Save,

		/// <summary>
		/// 	Выполнение специальных процедур после выполнения сохранения, бизнес-тригеры
		/// </summary>
		AfterSave,

		/// <summary>
		/// 	Успешное завершение
		/// </summary>
		Finished,
	}
}