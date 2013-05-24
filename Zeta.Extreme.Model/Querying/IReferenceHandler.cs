using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Интерфейс измерения "контрагент"
	/// </summary>
	public interface IReferenceHandler:IQueryDimension {
		/// <summary>
		/// Фильтр по контрагентам
		/// </summary>
		string Contragents { get; set; }
		/// <summary>
		/// Фильтр по счетам
		/// </summary>
		string Types { get; set; }

		/// <summary>
		/// Копирование при копировании запроса
		/// </summary>
		/// <returns></returns>
		IReferenceHandler Copy();
	}
}