#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ControlPointResult.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Результат сверки контрольной точки
	/// </summary>
	public class ControlPointResult {
		/// <summary>
		/// 	Провереная строка
		/// </summary>
		public IZetaRow Row { get; set; }

		/// <summary>
		/// 	Проверенная колонка
		/// </summary>
		public ColumnDesc Col { get; set; }

		/// <summary>
		/// 	Итоговое значение
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// 	Проверка валидности контрольной точки
		/// </summary>
		public bool IsValid {
			get { return Value == 0; }
		}

		/// <summary>
		/// 	Ссылка на исходный запрос, позволяет отсрочить получение значения
		/// </summary>
		public Query Query { get; set; }
	}
}