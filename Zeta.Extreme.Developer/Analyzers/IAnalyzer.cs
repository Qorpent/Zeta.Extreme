using System.Collections.Generic;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Интерфейс анализатора
	/// </summary>
	public interface IAnalyzer {
		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetParameterAttributes(SearchFilter filter = null);

		/// <summary>
		/// Сбор атрибутов колсетов
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetColsetAttribtes(SearchFilter filter = null);
	}
}