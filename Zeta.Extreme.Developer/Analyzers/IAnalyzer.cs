using System.Collections.Generic;
using Qorpent.IoC;
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

		/// <summary>
		/// Доступ к конфигурации среды разработки
		/// </summary>
		[Inject]
		ICodeIndex CodeIndex { get; set; }

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetReportAttributes(SearchFilter filter = null);

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetFormAttributes(SearchFilter filter = null);

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetThemaAttributes(SearchFilter filter = null);
	}
}