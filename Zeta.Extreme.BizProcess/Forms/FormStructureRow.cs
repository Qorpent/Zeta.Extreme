using Zeta.Extreme.Model.Extensions;
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

		/// <summary>
		/// Присоединенный к элементу структуры объект (для запросов и проч)
		/// </summary>
		public IZetaObject AttachedObject;

		/// <summary>
		/// Признак явного обязательного использования суммирования по деталям
		/// </summary>
		public bool SumObj;
		/// <summary>
		/// Дополнительный фильтр на ID контрагентов
		/// </summary>
		public string AltObjFilter;


		/// <summary>
		/// Дополнительный расчет первичности строки
		/// </summary>
		/// <returns></returns>
		public bool GetIsPrimary() {
			if (Fixed) return false;
			if (!Native.GetIsPrimary()) return false;
			if (SumObj) return false;
			if (!string.IsNullOrWhiteSpace(AltObjFilter)) return false;
			
			return true;
		}
		/// <summary>
		/// Признак фиксированного элемента
		/// </summary>
		public bool Fixed { get; set; }
	}
}