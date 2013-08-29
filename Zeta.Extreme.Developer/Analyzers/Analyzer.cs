using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
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
		public ICodeIndex Index
		{
			get { return _codeindex ?? (_codeindex = ResolveService<ICodeIndex>()); }
			set { _codeindex = value; }
		}

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetParameterAttributes(SearchFilter filter = null) {
			filter = filter ?? new SearchFilter {  DocRoot = "paramattr" };
			return Index.GetAttributes(new[] { "//*[@CodeCategory='Param']" }, filter);
		}

		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetReportAttributes(SearchFilter filter = null)
		{
			filter = filter ?? new SearchFilter { DocRoot = "outattr" };
			return Index.GetAttributes(new[] { "/*/out", "/*/report" }, filter);
		}


		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetFormAttributes(SearchFilter filter = null)
		{
			filter = filter ?? new SearchFilter { DocRoot = "formattr" };
			return Index.GetAttributes(new[] { "/*/in", "/*[local-name()!='processes']/form" }, filter);
		}


		/// <summary>
		/// Корневые элементы для атрибутов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetThemaAttributes(SearchFilter filter = null)
		{
			filter = filter ?? new SearchFilter { DocRoot = "themaattr", UseParamAsAttribute = true};
			return Index.GetAttributes(
				new[] { "/*[local-name()!='processes' and local-name()!='subst'  and local-name()!='paramlib' and local-name()!='global' and local-name()!='colset' and local-name()!='objset'  and local-name()!='rowset'  and local-name()!='paramset']" },
				filter);
		}
		/// <summary>
		/// Выполняет поиск возможных элементов кода и сопоставляет их тип
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ElementCodeTypeMap> GetElementTypeMap(SearchFilter filter = null)
		{
			filter = filter ?? new SearchFilter { DocRoot = "elementtypemap" ,ReferenceLimit = 300};
			return Index.GetElementCodeMap(filter);
		}


		/// <summary>
		/// сабсты
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetSubstAttributes(SearchFilter filter = null)
		{
			filter = filter ?? new SearchFilter { DocRoot = "substattr" };
			return CodeIndex.GetAttributes(new[] { "/subst" }, filter);
		}


		/// <summary>
		/// Сбор атрибутов колсетов
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetColsetAttribtes(SearchFilter filter = null) {
			filter = filter ?? new SearchFilter {AttributeValueLimit = 40, ReferenceLimit = 50, DocRoot="colattr"};
			return Index.GetAttributes(new[] { "//*[@CodeCategory='Column']" }, filter);
		}
		/// <summary>
		/// Получает элементы, относящиеся к параметрам
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<ParameterDescriptor> GetParameters(SearchFilter filter = null) {
			filter = filter ?? new SearchFilter { DocRoot = "param", BaseSelector = "//*[@CodeCategory='Param' or @CodeCategory='ParamRef']" };
			var elements = Index.SelectElements(filter).ToArray();
			var codegrouped = elements.GroupBy(_ => _.Attr("code"));
			var parameters = codegrouped.Select(_ => {
				var result = new ParameterDescriptor {
					Code = _.Key,
					Name = _.Max(__ => __.Attr("name")),
					Definitions = _.Where(__ => __.Attr("CodeCategory") == "Param").Select(__ => new ElementDescriptor(__)).ToArray(),
					References = _.Where(__ => __.Attr("CodeCategory") == "ParamRef").Select(__ => new ElementDescriptor(__)).ToArray()
				};

				return result;
			});

			return parameters.ToArray();
		} 
	}

}