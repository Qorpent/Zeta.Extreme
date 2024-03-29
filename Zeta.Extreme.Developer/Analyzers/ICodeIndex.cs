﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Интерфейс индексатора кода
	/// </summary>
	public interface ICodeIndex {
		/// <summary>
		/// Ищет все атрибуты, соответствующие заданным элементам 
		/// </summary>
		/// <param name="roots"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		IEnumerable<AttributeDescriptor> GetAttributes(string[] roots, SearchFilter filter = null);
		

		/// <summary>
		/// Вернуть полный список исходных XML
		/// </summary>
		/// <returns></returns>
		IEnumerable<Source> GetAllSources();

		/// <summary>
		/// Время перезагрузки
		/// </summary>
		DateTime LastResetTime { get; }

		/// <summary>
		/// Выдает индекс элементов, сопоставленных их типу по коду
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		IEnumerable<ElementCodeTypeMap> GetElementCodeMap(SearchFilter filter);


		/// <summary>
		/// Выполняет поиск элементов по заданым условиям
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		IEnumerable<XElement> SelectElements(SearchFilter filter);
	}
}