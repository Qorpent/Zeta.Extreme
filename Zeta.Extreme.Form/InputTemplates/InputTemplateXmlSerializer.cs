#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : InputTemplateXmlSerializer.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensibility;
using Comdiv.Extensions;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Сериализует шаблон формы из XML
	/// </summary>
	public class InputTemplateXmlSerializer {
		/// <summary>
		/// 	Основной метод - считывает шаблон из XML
		/// </summary>
		/// <param name="xpath"> </param>
		/// <returns> </returns>
		public IEnumerable<IInputTemplate> Read(XElement xpath) {
			IEnumerable<XElement> i_ = null;
			if (xpath.Name.LocalName == "inputs") {
				i_ = xpath.XPathSelectElements("input");
			}
			else {
				i_ = new[] {xpath};
			}
			foreach (var i in i_) {
				var result = new InputTemplate();
				result.DetailSplit = i.attr("split").toBool();
				result.SourceXmlConfiguration = xpath.CreateNavigator();
				result.Code = i.attr("id");
				result.Name = i.attr("name");
				result.ForGroup = i.getText("forGroup");
				result.AutoFillDescription = i.getText("autoFill");
				result.Help = i.getText("help");
				result.Area = i.attr("area", "default");
				result.DetailFilterName = i.attr("detailFilter", "");
				result.Controller = i.attr("controller", "default");
				result.CustomView = i.attr("customView");
				result.CustomSave = i.attr("customSave");
				result.ShowMeasureColumn = i.attr("showmeasurecolumn", false);
				result.CustomControllerType = i.attr("customController");
				result.SaveMethod = i.attr("saveMethod", "save");
				result.DetailFavorite = i.attr("detailfavorite", false);
				result.SqlOptimization = i.attr("sqloptimization", "");
				result.BindedReport = i.attr("bindedReport", "");
				result.ForPeriods = i.attr("forPeriods", "").split().Select(s => s.toInt()).ToArray();
				result.UnderwriteCode = i.attr("underwriteCode");
				result.Script = i.getText("script");
				result.InputForDetail = i.attr("detail", "False").toBool();
				result.TableView = i.attr("tableView", "");
				result.PeriodRedirect = i.attr("periodredirect", "");
				result.ApplyValueCourse = i.attr("applyvaluecourse", true);
				result.NeedFilesPeriods = i.attr("needfilesperiods", "");
				result.NeedFiles = i.attr("needfiles", "");
				result.DocumentRoot = i.attr("docroot", "");
				if (result.DocumentRoot.noContent()) {
					result.DocumentRoot = i.Elements("docroot").LastOrDefault().attr("code", "");
				}

				//var fields = i.read<InputField>("./field");
				//foreach (var field in fields){
				//	result.Fields.Add(field);
				//}

				//var queries = i.XPathSelectElements("./query");
				//foreach (var q in queries){
				//	var id = q.attr("id");
				//	var query = new InputQuery();
				//	query.Hql = q.attr("hql");
				//	query.View = q.attr("view");
				//	query.ViewParams = q.attr("viewParams");
				//	result.Queries[id] = query;
				//}

				var root = i.XPathSelectElement("./root");
				if (null != root) {
					result.Form = new RowDescriptor {Code = root.attr("code")};
				}

				var rows = i.XPathSelectElements("./row");
				BindColumns(result, rows);

				var fixedRows = i.XPathSelectElements("./fixrow");
				foreach (var x in fixedRows) {
					result.FixedRowCodes.Add(x.attr("code"));
				}

				var docs = i.XPathSelectElements("./doc");
				foreach (var x in docs) {
					result.Documents[x.attr("code")] = x.attr("name");
				}


				//альтернативный вариант привязки кодов исключения
				var fixrowlist = i.attr("fixrows", "").split();
				foreach (var row in fixrowlist) {
					result.FixedRowCodes.Add(row);
				}


				var parameters = i.XPathSelectElements("./param");
				foreach (var x in parameters) {
					result.Parameters[x.attr("name")] = x.attr("value");
				}

				var cols = i.XPathSelectElements("./col");
				BindColumns(result, cols);

				yield return result;
			}
		}

		/// <summary>
		/// 	Привязать строки из XML
		/// </summary>
		/// <param name="result"> </param>
		/// <param name="rows"> </param>
		public void BindRows(IInputTemplate result, IEnumerable<XElement> rows) {
			foreach (var row in rows) {
				var code = row.attr("code");
				if (code.hasContent()) {
					var rd = new RowDescriptor(code);
					rd.Name = row.attr("name", rd.Name);
					if (rd.Formula.noContent()) {
						rd.Formula = row.attr("formula", rd.Formula);
						if (rd.Formula.hasContent()) {
							rd.IsFormula = true;
							rd.FormulaEvaluator = row.attr("formulatype", "boo");
						}
					}
					result.Rows.Add(rd);
				}
			}
		}

		/// <summary>
		/// 	Привязать колонки из XML
		/// </summary>
		/// <param name="result"> </param>
		/// <param name="cols"> </param>
		public void BindColumns(IInputTemplate result, IEnumerable<XElement> cols) {
			if (null != cols) {
				foreach (var x in cols) {
					var col = new ColumnDesc {Code = x.attr("code"), Title = x.attr("name")};

					col.Year = x.value("year", 0);
					col.Period = x.value("period", -10000);
					if (col.Period == -10000) {
						col.NeedPeriodPreparation = true;
						col.Period = 0;
					}
					if (col.Period < 0) {
						col.NeedPeriodPreparation = true;
					}
					col.File = x.attr("_file");
					col.Line = x.attr("_line");
					col.ConditionMatcher = (IConditionMatcher) result;
					col.RowCheckConditions = x.Elements("checkrule").serialize<ColumnRowCheckCondition>().ToArray();
					col.LockYear = x.hasAttribute("lockyear");
					col.AutoCalc = x.attr("autocalc", false);
					col.LockPeriod = x.hasAttribute("lockperiod");
					col.Editable = !x.hasAttribute("fixed");
					col.IsAuto = x.hasAttribute("auto");
					col.Valuta = x.attr("valuta");
					col.Group = x.attr("group");
					col.Visible = x.attr("visible", true);
					col.CssClass = x.chooseAttr("cssclass", "css-class");
					col.CssStyle = x.chooseAttr("style", "css-style");
					col.Tag = x.attr("tag", "");
					col.NumberFormat = x.attr("format");
					col.Validation = x.attr("validation");
					col.WAvg = x.attr("wavg");
					col.UseObj = x.attr("useobj", false);
					col.Lookup = x.attr("lookup");
					col.MatrixForRows = x.attr("matrixforrows");
					col.ColGroup = x.attr("colgroup");
					col.TranslateRows = x.attr("translaterows");
					col.LookupFilter = x.attr("lookupFilter");
					col.EditForRole = x.attr("editforrole");
					col.ForRole = x.attr("forrole");
					col.ForGroup = x.attr("forgroup");
					col.CustomView = x.attr("customview");
					col.ValueReplacer = x.attr("valuereplacer");
					col.ValueToCssClass = x.attr("valuetocssclass", false);
					col.FormulaEvaluator = x.chooseAttr("evaluator", "formulatype");
					col.ControlPoint = x.attr("controlpoint", false);
					col.UseThema = x.attr("usethema", false);
					col.MatrixId = x.attr("matrixid");
					col.CalcIdx = x.attr("calcidx", 0);
					col.MatrixFormula = x.attr("matrixformula");
					col.MatrixTotalFormula = x.attr("matrixtotalformula");
					col.MatrixFormulaType = x.attr("matrixformulatype", "sum");
					col.Condition = x.attr("condition", "");

					if (col.FormulaEvaluator.noContent()) {
						col.FormulaEvaluator = "boo";
					}
					col.ForPeriods = x.attr("forperiods", "").split().Select(s => s.toInt()).ToArray();
					//if (null != storage){
					col.Target = ColumnCache.get(col.Code);
					//}
					if (col.Year < 1000) {
						col.NeedYearPreparation = true;
					}
					var formula = x.attr("formula");
					if (formula.hasContent()) {
						col.IsFormula = true;
						col.Formula = formula;
					}
					if (x.attr("customCode").hasContent()) {
						col.Code = x.attr("customCode");
					}
					/*
					var uid = result.Code + "_" + idx++;
					col.Uid = uid;
					 //olduid def
					 */

					var uid = "[" + col.Code + "_" + col.Year + "_" + col.Period + "]";
					col.Uid = uid;

					result.Values.Add(col);
				}
			}
		}

	}
}