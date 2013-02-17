#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : InputTemplateXmlSerializer.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Extensibility;
using Comdiv.Extensions;
using Comdiv.Zeta.Data.Minimal;

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
				result.DetailSplit = false;
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
					var col = new ColumnDesc();
					col.Period = -10000; //mark not setted
					col.Editable = true; //default
					col.Visible = true;
					col.MatrixFormulaType = "sum";
					col.FormulaEvaluator = "boo";
					foreach (var attribute in x.Attributes().ToArray()) {
						var n = attribute.Name.LocalName;
						var v = attribute.Value;
						switch (n) {
							case "code":
								col.Code = v;
								break;
							case "name":
								col.Title = v;
								break;
							case "year":
								col.Year = v.toInt();
								break;
							case "period":
								col.Period = v.toInt();
								break;
							case "_file":
								col.File = v;
								break;
							case "_line":
								col.Line = v;
								break;
							case "condition":
								col.Condition = v;
								break;
							case "lockyear":
								col.LockYear = true;
								break;
							case "lockperiod":
								col.LockPeriod = true;
								break;
							case "autocalc":
								col.AutoCalc = v.toBool();
								break;
							case "fixed":
								col.Editable = false;
								break;
							case "auto":
								col.IsAuto = true;
								break;
							case "valuta":
								col.Valuta = v;
								break;
							case "group":
								col.Group = v;
								break;
							case "visible":
								col.Visible = v.toBool();
								break;
							case "cssclass":
								goto case "css-class";
							case "css-class":
								col.CssClass = v;
								break;
							case "style":
								goto case "css-style";
							case "css-style":
								col.CssStyle = v;
								break;
							case "tag":
								col.Tag = v;
								break;
							case "format":
								col.NumberFormat = v;
								break;
							case "validation":
								col.Validation = v;
								break;
							case "wavg":
								col.WAvg = v;
								break;
							case "useobj":
								col.UseObj = v.toBool();
								break;
							case "matrixforrows":
								col.MatrixForRows = v;
								break;
							case "colgroup":
								col.ColGroup = v;
								break;
							case "translaterows":
								col.TranslateRows = v;
								break;
							case "lookupFilter":
								col.LookupFilter = v;
								break;
							case "editforrole":
								col.EditForRole = v;
								break;
							case "forrole":
								col.ForRole = v;
								break;
							case "forgroup":
								col.ForGroup = v;
								break;
							case "customview":
								col.CustomView = v;
								break;
							case "valuereplacer":
								col.ValueReplacer = v;
								break;
							case "matrixid":
								col.MatrixId = v;
								break;
							case "matrixformula":
								col.MatrixFormula = v;
								break;
							case "matrixtotalformula":
								col.MatrixTotalFormula = v;
								break;
							case "usethema":
								col.UseThema = v.toBool();
								break;
							case "valuetocssclass":
								col.ValueToCssClass = v.toBool();
								break;
							case "controlpoint":
								col.ControlPoint = v.toBool();
								break;
							case "evaluator":
								goto case "formulatype";
							case "formulatype":
								col.FormulaEvaluator = v;
								break;
							case "calcidx":
								col.CalcIdx = v.toInt();
								break;
							case "matrixformulatype":
								col.MatrixFormulaType = v;
								break;
							case "formula":
								col.Formula = v;
								if (!string.IsNullOrWhiteSpace(v)) {
									col.IsFormula = true;
								}
								break;
							case "forperiods":
								col.ForPeriods = v.split().Select(s => s.toInt()).ToArray();
								break;
							case "customCode":
								col.CustomCode = v;
								break;
						}
					}

					if (col.Period == -10000) {
						col.NeedPeriodPreparation = true;
						col.Period = 0;
					}
					if (col.Period < 0) {
						col.NeedPeriodPreparation = true;
					}

					if (null == col.ForPeriods) {
						col.ForPeriods = new int[] {};
					}

					col.ConditionMatcher = (IConditionMatcher) result;
					col.RowCheckConditions = x.Elements("checkrule").serialize<ColumnRowCheckCondition>().ToArray();


					//if (null != storage){
					col.Target = ColumnCache.get(col.Code);
					//}
					if (col.Year < 1000) {
						col.NeedYearPreparation = true;
					}

					if (!string.IsNullOrWhiteSpace(col.CustomCode)) {
						col.InitialCode = col.Code;
						col.Code = col.CustomCode;
					}


					var uid = "[" + col.Code + "_" + col.Year + "_" + col.Period + "]";
					col.Uid = uid;

					result.Values.Add(col);
				}
			}
		}
	}
}