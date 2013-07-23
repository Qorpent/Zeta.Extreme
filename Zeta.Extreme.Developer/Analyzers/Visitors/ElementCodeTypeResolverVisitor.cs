using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers.Visitors {
	/// <summary>
	/// Посетитель индексатора типов элемента кода
	/// </summary>
	public class ElementCodeTypeResolverVisitor : ISourceVisitor {
		

		/// <summary>
		/// Словарь соответствия пути элемента и его типа
		/// </summary>
		public readonly IDictionary<string, CodeElementType> PathToTypeMap =
			new Dictionary<string, CodeElementType> {
				{"/paramlib", CodeElementType.ParamLib},
				{"/param", CodeElementType.ParamDefRoot},
				{"/paramlib/param", CodeElementType.ParamDefLib},
				{"/paramset", CodeElementType.ParamSet},
				{"/paramset/use", CodeElementType.ParamUseInParamset},
				{"/paramset/ask", CodeElementType.ParamAskInParamset},

				{"/colset", CodeElementType.Colset},
				{"/colset/col", CodeElementType.ColInColset},
				{"/colset/col/checkrule", CodeElementType.ColCheckRule},
				{"/colset/import", CodeElementType.ColsetImportIntoColset},
				{"/colset/ask", CodeElementType.ParamAskReferenceInColset},

				{"/thema",CodeElementType.ThemaRooted},
				{"/*/out",CodeElementType.ReportDef},
				{"/*/out/col",CodeElementType.ColInReport},
				{"/*/out/ask",CodeElementType.ParamAskInReport},
				{"/*/out/use",CodeElementType.ParamUseInReport},
				{"/*/out/param",CodeElementType.ReportParamDefLocalParam},
				{"/*/out/var",CodeElementType.ReportParamDefLocalVar},
				{"/*/report",CodeElementType.ReportDef},
				{"/*/report/ask",CodeElementType.ParamAskInReport},
				{"/*/report/use",CodeElementType.ParamUseInReport},
				{"/*/report/param",CodeElementType.ReportParamDefLocalParam},
				{"/*/report/var",CodeElementType.ReportParamDefLocalVar},
				{"/*/report/col",CodeElementType.ColInReport},
				{"/*/reportset",CodeElementType.ReportSet},
				{"/*/reportset/col",CodeElementType.ColInReport},
				{"/*/reportsetex",CodeElementType.ReportSetEx},
				{"/*/reportsetex/col",CodeElementType.ColInReport},

				{"/*/in",CodeElementType.FormDef},
				{"/*/form",CodeElementType.FormDef},
				{"/*/formset",CodeElementType.FormSet},
				{"/*/formsetex",CodeElementType.FormSetEx},
				
				
				{"/subst",CodeElementType.SubstDefinition},
				{"/processes",CodeElementType.ContentExtensions},


				{"/*/out/uselib",CodeElementType.UseLibReport},
				{"/*/reportset/var",CodeElementType.ReportParamDefLocalVar},

				{"/*/form/addcols",CodeElementType.ColsetImportIntoForm},
				{"/*/formset/addcols",CodeElementType.ColsetImportIntoForm},
				{"/*/out/addcols",CodeElementType.ColsetImportIntoReport},
				{"/*/reportset/addcols",CodeElementType.ColsetImportIntoReport},
				{"/*/reportsetex/addcols",CodeElementType.ColsetImportIntoReport},
				{"/colset/addcols",CodeElementType.ColsetImportIntoColset},

			};

		/// <summary>
		/// Выполнить посещение источника
		/// </summary>
		/// <param name="source"></param>
		public void Visit(Source source) {
			foreach (var e in source.XmlContent.Elements()) {
				VisitElement(e);
			}
		}
		/// <summary>
		/// Выполнить посещение корневого элемента
		/// </summary>
		/// <param name="e"></param>
		private void VisitElement(XElement e) {
			var key = CodeIndex.GetPath(e);
			
			if (PathToTypeMap.ContainsKey(key)) {
					e.SetAttributeValue("CodeType", PathToTypeMap[key]);
				
			}
			else if (CodeIndex.KnownRoots.Contains(key)) {
				e.SetAttributeValue("CodeType", CodeElementType.Undefined);
			}
			else if (key.LastIndexOf("/") == 0) {
				e.SetAttributeValue("CodeType", CodeElementType.ThemaInherited);
			}
			else {
				e.SetAttributeValue("CodeType", CodeElementType.Undefined);
			}
			foreach (var e_ in e.Elements()) {
				VisitElement(e_);
			}
		}
	}
}