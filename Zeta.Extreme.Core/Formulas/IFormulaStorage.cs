using System;

namespace Zeta.Extreme {
	/// <summary>
	/// Интерфейс коллекции формул
	/// </summary>
	public interface IFormulaStorage {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		///<returns>Возвращает полученный ключ формулы</returns>
		string Register(FormulaRequest request);

		/// <summary>
		/// Возвращает экземпляр формулы по ключу 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="throwErrorOnNotFound">false если надо возвращать NULL при отсутствии формулы </param>
		/// <returns></returns>
		IFormula GetFormula(string key ,bool throwErrorOnNotFound = true);

		/// <summary>
		/// Возвращает формулу обратно хранилищу, может использовать для реализации кэша формул
		/// </summary>
		/// <param name="key"></param>
		/// <param name="formula"></param>
		void Return(string key, IFormula formula);
	}
}