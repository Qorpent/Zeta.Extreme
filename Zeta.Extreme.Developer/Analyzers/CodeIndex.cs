using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent;
using Qorpent.Bxl;
using Qorpent.Dsl;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Config;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Стандартный индекс исходного кода
	/// </summary>
	public class CodeIndex :  ServiceBase, ICodeIndex {

		private IDeveloperConfig _config;

		/// <summary>
		/// Доступ к конфигурации среды разработки
		/// </summary>
		[Inject]
		public IDeveloperConfig Config
		{
			get { return _config ?? (_config = ResolveService<IDeveloperConfig>()); }
			set { _config = value; }
		}
	
		private IDictionary<string,AttributeDescriptor[]> _attributeResolutionCache = new Dictionary<string, AttributeDescriptor[]>();

		private IBxlParser _bxl;

		/// <summary>
		/// Доступ к конфигурации среды разработки
		/// </summary>
		[Inject]
		public IBxlParser Bxl
		{
			get { return _bxl ?? (_bxl = ResolveService<IBxlParser>()); }
			set { _bxl = value; }
		}

		private IList<Source> _sourcecache = null;
		/// <summary>
		/// Вернуть полный список исходных XML
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Source> GetAllSources()
		{
			return _sourcecache ?? (_sourcecache = InternalReadXml().ToList());
		}
		/// <summary>
		/// Кэш источников в виде пополнянемого списка
		/// </summary>
		protected IList<Source> Sources {
			get { if (null == _sourcecache) GetAllSources();
				return _sourcecache;
			}
		}

		private IEnumerable<Source> InternalReadXml()
		{
			if (null == Config || null==Config.ThemaSourceFolders) {
				return new List<Source>();
			}

			return
				from dir in Config.ThemaSourceFolders
				from file in Directory.GetFiles(dir, "*.bxl")
				let content = File.ReadAllText(file)
				let xelement = Bxl.Parse(content, file)
				select new Source { FileName = file, SourceContent = content, XmlContent = xelement };
		}
		/// <summary>
		/// Ручное добавление источников
		/// </summary>
		/// <param name="source"></param>
		public void AddSource(Source source) {
			Sources.Add(source);
		}

		/// <summary>
		/// Извлекает атрибуты из отдельного источника по руту
		/// </summary>
		/// <param name="source"></param>
		/// <param name="root"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<SimpleAttributeDescriptor> FindAttributes(Source source, string root,SearchFilter filter = null) {
			var result = (
				             source.XmlContent.XPathEvaluate(
					             "//" + root + "/@*[" + DeveloperConstants.XpathSysAttributesFilter + "]")
				             as IEnumerable)
				.Cast<XAttribute>()
				.Select(_ => new SimpleAttributeDescriptor {
					Name = _.Name.LocalName,
					Value = _.Value,
					LexInfo = new LexInfo {
						File = _.Parent.Attr("_file"),
						Line = _.Parent.Attr("_line").ToInt()
						,
						Context = _.Parent.Parent.Name.LocalName + ":" + _.Parent.Parent.ChooseAttr("__code", "code") + "/" + _.Parent.Name.LocalName + ":" + _.Parent.ChooseAttr("__code", "code") + "(" + _.Parent.ChooseAttr("__name", "name") + ")"
					}
				}).ToArray();
			return result;
		}

		/// <summary>
		/// Собирает все атрибуты по рутам  в исходном файле кода
		/// </summary>
		/// <param name="source"></param>
		/// <param name="roots"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<SimpleAttributeDescriptor> FindAttributes(Source source, string[] roots, SearchFilter filter = null) {
			var result = roots.SelectMany(_ => FindAttributes(source, _,filter)).ToArray();
			return result;
		}

		/// <summary>
		/// Собирает индекс упоминаемости и значений атрибутов
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> CollectAttributeIndex(SearchFilter filter, IEnumerable<SimpleAttributeDescriptor> attributes) {

			var basidx = attributes.GroupBy(_ => _.Name, _ => _);
			foreach (var grp in basidx) {
				var item = new AttributeDescriptor();
				var groupedvalues = grp.GroupBy(_ => _.Value);
				item.Name = grp.Key;
				bool includevariants = 0 == filter.AttributeValueLimit || groupedvalues.Count() <= filter.AttributeValueLimit;
				item.VariantCount = groupedvalues.Count();
				foreach (var valuegrp in groupedvalues) {
					
					var variant = new AttributeValueVariant();
					variant.Value = valuegrp.Key;
					bool includereferences = 0 == filter.AttributeValueReferenceLimit ||valuegrp.Count() <= filter.AttributeValueReferenceLimit;
					item.ReferenceCount += valuegrp.Count();
					variant.ReferenceCount = valuegrp.Count();
					if (includereferences && includevariants) {
						foreach (var attributeDescriptor in valuegrp) {
							variant.References.Add(attributeDescriptor.LexInfo);
						}
					}
					if (includevariants)
					{
						item.ValueVariants.Add(variant);
					}
					
				}

				yield return item;
			}

		}



		/// <summary>
		/// Получает список всех атрибутов указанных рутов по всем источникам
		/// </summary>
		/// <param name="roots"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<AttributeDescriptor> GetAttributes(string[] roots, SearchFilter filter = null) {
			filter = filter ?? new SearchFilter();
			var key = filter + ",roots:" + string.Join(",", roots.Distinct().OrderBy(_ => _));
			if (_attributeResolutionCache.ContainsKey(key)) {
				return _attributeResolutionCache[key];
			}
			var result =
				CollectAttributeIndex(filter,GetAllSources().SelectMany(xml => FindAttributes(xml, roots,filter))).ToArray();
			_attributeResolutionCache[key] = result;
			return result;
		}

	}
}