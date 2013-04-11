using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Промежуточная строка структуры
	/// </summary>
	public class FormStructureRow {
		/// <summary>
		/// Полное определение строки
		/// </summary>
		public IZetaRow Native;
		/// <summary>
		/// Номер
		/// </summary>
		public int Idx;
		/// <summary>
		/// Уровень
		/// </summary>
		public int Level;
	}
}