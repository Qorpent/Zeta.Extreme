#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveResult.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Общий результат запроса на сохранения
	/// </summary>
	public class SaveResult {
		/// <summary>
		/// 	Ячейки, переданные для сохранения
		/// </summary>
		public OutCell[] SaveCells { get; set; }

		/// <summary>
		/// 	Скрипт сохранения ячеек
		/// </summary>
		public string SaveSqlScript { get; set; }

		/// <summary>
		/// 	Признак успешного завершения вызова AfterSave
		/// </summary>
		public bool AfterSaveCalled { get; set; }
	}
}