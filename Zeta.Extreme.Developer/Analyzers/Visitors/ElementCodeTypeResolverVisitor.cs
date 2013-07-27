using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers.Visitors
{
	/// <summary>
	/// Посетитель индексатора типов элемента кода
	/// </summary>
	public class ElementCodeTypeResolverVisitor : ISourceVisitor
	{


		/// <summary>
		/// Словарь соответствия пути элемента и его типа
		/// </summary>
		public readonly IDictionary<string, CodeElementType> PathToTypeMap =
			new Dictionary<string, CodeElementType> {
				{"/paramlib", CodeElementType.ParamLib},
				
					
				
				{"/paramset", CodeElementType.ParamSet},
				
				
				

				{"/colset", CodeElementType.Colset},
				
				{"/colset/col/checkrule", CodeElementType.ColCheckRule},
				
				{"/colset/import", CodeElementType.ColsetImportIntoColset},
				{"/colset/imports", CodeElementType.ColsetImportIntoColset},
				{"/paramset/imports", CodeElementType.ImportParamset},
				{"/*/imports", CodeElementType.Imports},
				
				{"/extension", CodeElementType.Extension},
				
				
				
				

				{"/thema",CodeElementType.ThemaRooted},
				{"/*/out",CodeElementType.ReportDef},
				
				
				
				
				{"/*/report",CodeElementType.ReportDef},
				
				
				
				
				{"/*/report/col",CodeElementType.ColInReport},
				{"/*/reportset",CodeElementType.ReportSet},
				
				{"/*/reportsetex",CodeElementType.ReportSetEx},
				

				{"/*/in",CodeElementType.FormDef},
				
				
				
				
				{"/subst",CodeElementType.SubstDefinition},
				{"/processes",CodeElementType.ContentExtensions},


				
				
				
				
				

				{"/*/form/addcols",CodeElementType.ColsetImportIntoForm},
				{"/*/formset/addcols",CodeElementType.ColsetImportIntoForm},
				{"/*/out/addcols",CodeElementType.ColsetImportIntoReport},
				{"/*/reportset/addcols",CodeElementType.ColsetImportIntoReport},
				{"/*/reportsetex/addcols",CodeElementType.ColsetImportIntoReport},
				{"/colset/addcols",CodeElementType.ColsetImportIntoColset},
				
				
				{"/paramlib/param", CodeElementType.ParamDefLib},
				{"/*/out/param",CodeElementType.ReportParamDefLocalParam},
				{"/*/report/param",CodeElementType.ReportParamDefLocalParam},
				{"/param", CodeElementType.ParamDefRoot},
				{"/*/form/param", CodeElementType.ParamInForm},
				{"/*/formset/param", CodeElementType.ParamInForm},
				{"/*/formsetex/param", CodeElementType.ParamInForm},
				{"/*/out/show/param", CodeElementType.ParamShowReport},
				{"/*/param", CodeElementType.ParamDefRoot},
				{"/*/reportset/param", CodeElementType.ReportParamDefLocalParam},
				{"/*/reportsetex/param", CodeElementType.ReportParamDefLocalParam},
				{"/paramset/param", CodeElementType.ParamInParamset},
				
					
				
				
				
				
				{"/*/form/col", CodeElementType.ColInForm},
				{"/*/formset/col", CodeElementType.ColInForm},
				{"/*/formsetex/col", CodeElementType.ColInForm},
				{"/*/out/col", CodeElementType.ColInReport},
				{"/*/reportset/col",CodeElementType.ColInReport},
				{"/*/reportsetex/col",CodeElementType.ColInReport},
				{"/colset/col", CodeElementType.ColInColset},
				
				
				{"/*/form",CodeElementType.FormDef},
				{"/processes/form",CodeElementType.ContentExtensionsForm},
				
			
				
				{"/*/form/row",CodeElementType.RowInForm},
				{"/*/formset/row",CodeElementType.RowInForm},
				{"/*/out/row",CodeElementType.RowInReport},
				{"/*/reportset/row",CodeElementType.RowInReport},
				{"/*/reportsetex/row",CodeElementType.RowInReport},
				{"/*/rows/row",CodeElementType.RowInRows},
				{"/rowset/row",CodeElementType.RowInRowSet},
				{"/*/rows",CodeElementType.Rows},
				
				{"/*/out/uselib",CodeElementType.UseLibReport},
				{"/*/form/uselib",CodeElementType.UseLibForm},
				{"/*/reportsetex/uselib",CodeElementType.UseLibReport},
				
				{"/*/formset",CodeElementType.FormSet},
		
				{"/*/reportset/ask",CodeElementType.ParamAskInReportDef},
				{"/*/reportsetex/ask",CodeElementType.ParamAskInReportDef},
				{"/paramset/ask", CodeElementType.ParamAskInParamset},
				{"/colset/ask", CodeElementType.ParamAskReferenceInColset},
				{"/*/out/ask",CodeElementType.ParamAskInReportDef},
				{"/*/report/ask",CodeElementType.ParamAskInReportDef},
				
				{"/global",CodeElementType.Global},
				{"/global/generator",CodeElementType.GlobalGeneration},
				{"/*/out/generator",CodeElementType.ReportGeneration},
				
				{"/*/formsetex",CodeElementType.FormSetEx},
				
				{"/*/out/generator/include",CodeElementType.GenerationEither},
				{"/objset/generator/include",CodeElementType.ObjsetGenerationIn},
				{"/objset/generator",CodeElementType.ObjsetGeneration},
				{"/objset/generator/filter",CodeElementType.ObjsetGenerationFilter},
				{"/objset/generator/condition",CodeElementType.ObjsetGenerationCond},
				{"/*/rows/generator",CodeElementType.RowsGeneration},
				{"/objset",CodeElementType.Objset},
				
				
				
				{"/objset/object",CodeElementType.ObjectInObjset},
				{"/*/reportsetex/object",CodeElementType.ObjectInReport},
				{"/*/reportset/object",CodeElementType.ObjectInReport},
				{"/*/out/object",CodeElementType.ObjectInReport},
				
				{"/paramset/use", CodeElementType.ParamUseInParamset},
				{"/*/out/use",CodeElementType.ParamUseInReportDef},
				{"/*/report/use",CodeElementType.ParamUseInReportDef},
				{"/*/reportsetex/use",CodeElementType.ParamUseInReportDef},
				{"/*/reportset/use",CodeElementType.ParamUseInReportDef},
				
				
				{"/*/out/var",CodeElementType.ReportParamDefLocalVar},
				{"/*/report/var",CodeElementType.ReportParamDefLocalVar},
				{"/*/reportset/var",CodeElementType.ReportParamDefLocalVar},
				{"/*/reportsetex/var",CodeElementType.ReportParamDefLocalVar},
				
				{"/*/reportsetex/hide",CodeElementType.HideParamInReport},
				{"/*/reportset/hide",CodeElementType.HideParamInReport},
				
				{"/rowset",CodeElementType.Rowset},
			
				
				
				 
				
				
				
				
				
				
					
					
				
				
		
		
				
					
					
				
				
					

			};

		/// <summary>
		/// Выполнить посещение источника
		/// </summary>
		/// <param name="source"></param>
		public void Visit(Source source)
		{
			foreach (var e in source.XmlContent.Elements())
			{
				VisitElement(e);
			}
		}
		/// <summary>
		/// Выполнить посещение корневого элемента
		/// </summary>
		/// <param name="e"></param>
		private void VisitElement(XElement e)
		{
			var key = CodeIndex.GetPath(e);

			if (PathToTypeMap.ContainsKey(key))
			{
				e.SetAttributeValue("CodeType", PathToTypeMap[key]);

			}
			else if (CodeIndex.KnownRoots.Contains(key))
			{
				e.SetAttributeValue("CodeType", CodeElementType.Undefined);
			}
			else if (key.LastIndexOf("/") == 0)
			{
				e.SetAttributeValue("CodeType", CodeElementType.ThemaInherited);
			}
			else
			{
				e.SetAttributeValue("CodeType", CodeElementType.Undefined);
			}
			foreach (var e_ in e.Elements())
			{
				VisitElement(e_);
			}
		}
	}
}