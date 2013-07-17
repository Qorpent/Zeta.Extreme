﻿using System.Collections.Generic;
using Qorpent;
using Qorpent.IoC;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Базовый анализатор
	/// </summary>
	public  class Analyzer : ServiceBase, IAnalyzer {
		private ICodeIndex _codeindex;

		/// <summary>
		/// Доступ к конфигурации среды разработки
		/// </summary>
		[Inject]
		public ICodeIndex CodeIndex
		{
			get { return _codeindex ?? (_codeindex = ResolveService<ICodeIndex>()); }
			set { _codeindex = value; }
		}

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetParameterAttributes(SearchFilter filter = null) {
			return CodeIndex.GetAttributes(new[] { "paramlib/param", "out/param", "out/var" }, filter);
		}
		/// <summary>
		/// Сбор атрибутов колсетов
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetColsetAttribtes(SearchFilter filter = null) {
			filter = filter ?? new SearchFilter {AttributeValueLimit = 20, AttributeValueReferenceLimit = 50};
			return CodeIndex.GetAttributes(new[] { "colset/col", "out/col", "form/col" }, filter);
		}
	}
}