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
using Qorpent.Events;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Config;
using Zeta.Extreme.Developer.Model;
using Zeta.Extreme.Developer.Utils;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Стандартный индекс исходного кода
	/// </summary>
	[RequireReset(All=true,Options=new[]{"zdev.cache"})]
	public class CodeIndex :  ServiceBase, ICodeIndex {
		/// <summary>
		/// /
		/// </summary>
		public CodeIndex() {
			LastResetTime = DateTime.Now;
		}
		/// <summary>
		/// Время перезагрузки
		/// </summary>
		public DateTime LastResetTime { get; private set; }

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
		/// Сброс кэшей
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override object Reset(ResetEventData data) {
			_sourcecache = null;
			_doc=null;
			_attributeResolutionCache.Clear();
			LastResetTime = DateTime.Now;
			return true;
		}

		private IDictionary<string, Documentation> _doc;
		/// <summary>
		/// Вернуть полный список исходных XML
		/// </summary>
		/// <returns></returns>
		public IDictionary<string, Documentation> GetDoc()
		{
			return _doc ?? (_doc = LoadDoc());
		}

		private IDictionary<string, Documentation> LoadDoc()
		{
			if (string.IsNullOrWhiteSpace(Config.DocFolder)) return new Dictionary<string, Documentation>();
			//if (!Directory.Exists(Config.DocFolder)) return new Dictionary<string, Documentation>();
			var files = Directory.GetFiles(Config.DocFolder, "*.doc.bxl");
			var fullcontent = "";
			foreach (var file in files) {
				fullcontent += File.ReadAllText(file)+"\r\n";
			}
			var xml = Bxl.Parse( fullcontent,"doc");
			return DocumentationHelper.LoadDocuments(xml);
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
					              root + "/@*[" + DeveloperConstants.XpathSysAttributesFilter + "]")
				             as IEnumerable)
				.Cast<XAttribute>()
				.Select(_ => new SimpleAttributeDescriptor {
					Name = _.Name.LocalName,
					Value = _.Value,
					LexInfo = new LexInfo {
						File = _.Parent.Attr("_file"),
						Line = _.Parent.Attr("_line").ToInt()
						,
						Context = PrepareContext(_)
					}
				}).ToArray();
			return result;
		}

		private string PrepareContext(XAttribute current) {
			var c = current.Parent;
			var result = "";
			while (c.Parent != null) {	
				var desc = c.Describe(explicitname:true);
				var type = c.Name.LocalName;
				var code = desc.Code;
				var name = desc.Name;
				var str = "/" + type;
				if (!string.IsNullOrWhiteSpace(code)) {
					str += "[" + code;
				}
				if (!string.IsNullOrWhiteSpace(name) && (name != code)) {
					str += "(" + name + ")";
				}
				if (!string.IsNullOrWhiteSpace(code)) {
					str += "]";
				}
				result = str + result;
				c = c.Parent;
			}
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
			var doc = GetDoc();
			var basidx = attributes.GroupBy(_ => _.Name, _ => _);
			foreach (var grp in basidx) {
				var item = new AttributeDescriptor {Name = grp.Key}.SetupDocument(filter,doc);
				var groupedvalues = grp.GroupBy(_ => _.Value);
				
				bool includevariants = 0 == filter.AttributeValueLimit || groupedvalues.Count() <= filter.AttributeValueLimit;
				item.VariantCount = groupedvalues.Count();
				foreach (var valuegrp in groupedvalues) {
					
					var variant = new AttributeValueVariant {Value = valuegrp.Key,Parent=item}.SetupDocument(filter, doc);
					bool includereferences = filter.IncludeReferences && ( 0 == filter.AttributeValueReferenceLimit ||valuegrp.Count() <= filter.AttributeValueReferenceLimit);
					item.ReferenceCount += valuegrp.Count();
					variant.ReferenceCount = valuegrp.Count();
					if (includereferences && includevariants) {
						SetupReferencesForVariant(valuegrp, variant);
					}
					if (includevariants)
					{
						item.ValueVariants.Add(variant);
					}
					
				}

				yield return item;
			}

		}

		private void SetupReferencesForVariant(IGrouping<string, SimpleAttributeDescriptor> valuegrp, AttributeValueVariant variant) {
			var rawrefs = new List<ItemReference>();
			foreach (var ad in valuegrp) {
				var maincontext = ad.LexInfo.Context.SmartSplit(false, true, '/')[0];
				var subcontext = string.Join("/", ad.LexInfo.Context.SmartSplit(false, true, '/').Skip(1));
				rawrefs.Add(
					new ItemReference {
						File = Path.GetFileName(ad.LexInfo.File),
						Line = ad.LexInfo.Line,
						MainContext = maincontext,
						SubContext = subcontext
					}
					);
			}
			foreach (var ir in GroupReferences(rawrefs.ToArray())) {
				variant.References.Add(ir);
			}
		}

		/// <summary>
		/// Группировка ссылок
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public IEnumerable<ItemReference> GroupReferences(ItemReference[] src) {
			var fstlevel = GroupReferencesBySubContext(src);
			var secondlevel = GroupReferencesByMainContext(fstlevel);
			var thdlevel = GroupReferencesByFile(secondlevel);
			return thdlevel;
		}

		private static ItemReference[] GroupReferencesByFile(ItemReference[] secondlevel) {
			var thdlevel = secondlevel.GroupBy(_ => _.File, _ => _)
			                          .Select(_ => new ItemReference {
				                          File = _.First().File,
				                          Children =
					                          _.Select(
						                          __ =>
						                          new ItemReference {
							                          MainContext = __.MainContext,
							                          SubContext = __.SubContext,
							                          Line = __.Line,
							                          Children = __.Children
						                          })
			                          }).ToArray();
			foreach (var s in thdlevel.Where(_ => _.Children.Count() == 1)) {
				s.Line = s.Children.First().Line;
				s.SubContext = s.Children.First().SubContext;
				s.MainContext = s.Children.First().MainContext;
				s.Children = s.Children.First().Children;
			}
			return thdlevel;
		}

		private static ItemReference[] GroupReferencesByMainContext(ItemReference[] fstlevel) {
			var secondlevel = fstlevel.GroupBy(_ => _.File + ":" + _.MainContext, _ => _)
			                          .Select(_ => new ItemReference {
				                          File = _.First().File,
				                          MainContext = _.First().MainContext,
				                          Children =
					                          _.Select(
						                          __ =>
						                          new ItemReference {SubContext = __.SubContext, Line = __.Line, Children = __.Children})
			                          }).ToArray();
			foreach (var s in secondlevel.Where(_ => _.Children.Count() == 1)) {
				s.Line = s.Children.First().Line;
				s.SubContext = s.Children.First().SubContext;
				s.Children = s.Children.First().Children;
			}
			return secondlevel;
		}

		private static ItemReference[] GroupReferencesBySubContext(ItemReference[] src) {
			var fstlevel = src.GroupBy(_ => _.File + ":" + _.MainContext + ":" + _.SubContext, _ => _)
			                  .Select(_ => new ItemReference {
				                  File = _.First().File,
				                  MainContext = _.First().MainContext,
				                  SubContext = _.First().SubContext,
				                  Children =
					                  _.Select(__ => new ItemReference {Line = __.Line})
			                  }).ToArray();
			foreach (var s in fstlevel.Where(_ => _.Children.Count() == 1)) {
				s.Line = s.Children.First().Line;
				s.Children = null;
			}
			return fstlevel;
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