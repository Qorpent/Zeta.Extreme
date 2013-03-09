using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Описатель запроса с поддержкой обработки
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		/// <summary>
		/// 	Обеспечивает возврат результата запроса
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);
	}
}