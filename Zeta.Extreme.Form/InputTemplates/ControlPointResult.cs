using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Результат сверки контрольной точки
	/// </summary>
	public class ControlPointResult {
		/// <summary>
		/// Провереная строка
		/// </summary>
		public IZetaRow Row { get; set; }
		/// <summary>
		/// Проверенная колонка
		/// </summary>
		public ColumnDesc Col { get; set; }
		/// <summary>
		/// Итоговое значение 
		/// </summary>
		public decimal Value { get; set; }
		/// <summary>
		/// Проверка валидности контрольной точки
		/// </summary>
		public bool IsValid {
			get { return Value == 0; }
		}
	}
}