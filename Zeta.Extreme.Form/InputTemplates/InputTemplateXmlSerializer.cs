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
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.MetaCaches;


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
				result.Code = i.Attr("id");
				result.Name = i.Attr("name");
				result.ForGroup = i.GetTextElement("forGroup");
				result.AutoFillDescription = i.GetTextElement("autoFill");
				result.Help = i.GetTextElement("help");
				result.Area = i.Attr("area", "default");
				result.DetailFilterName = i.Attr("detailFilter", "");
				result.Controller = i.Attr("controller", "default");
				result.CustomView = i.Attr("customView");
				result.CustomSave = i.Attr("customSave");
				result.ShowMeasureColumn = i.Attr("showmeasurecolumn").ToBool();
				result.CustomControllerType = i.Attr("customController");
				result.SaveMethod = i.Attr("saveMethod", "save");
				result.DetailFavorite = i.Attr("detailfavorite").ToBool();
				result.SqlOptimization = i.Attr("sqloptimization", "");
				result.BindedReport = i.Attr("bindedReport", "");
				result.ForPeriods = i.Attr("forPeriods", "").SmartSplit().Select(s => s.ToInt()).ToArray();
				result.UnderwriteCode = i.Attr("underwriteCode");
				result.Script = i.GetTextElement("script");
				result.InputForDetail = i.Attr("detail", "False").ToBool();
				result.TableView = i.Attr("tableView", "");
				result.PeriodRedirect = i.Attr("periodredirect", "");
				result.ApplyValueCourse = i.Attr("applyvaluecourse").IsEmpty() || i.Attr("applyvaluecourse").ToBool();
				result.NeedFilesPeriods = i.Attr("needfilesperiods", "");
				result.NeedFiles = i.Attr("needfiles", "");
				result.DocumentRoot = i.Attr("docroot", "");
				if (result.DocumentRoot.IsEmpty()) {
					result.DocumentRoot = i.Elements("docroot").LastOrDefault().Attr("code", "");
				}

				//var fields = i.read<InputField>("./field");
				//foreach (var field in fields){
				//	result.Fields.Add(field);
				//}

				//var queries = i.XPathSelectElements("./query");
				//foreach (var q in queries){
				//	var id = q.Attr("id");
				//	var query = new InputQuery();
				//	query.Hql = q.Attr("hql");
				//	query.View = q.Attr("view");
				//	query.ViewParams = q.Attr("viewParams");
				//	result.Queries[id] = query;
				//}

				var root = i.XPathSelectElement("./root");
				if (null != root) {
					result.Form = new RowDescriptor {Code = root.Attr("code")};
				}

				var rows = i.XPathSelectElements("./row");
				BindColumns(result, rows);

				var fixedRows = i.XPathSelectElements("./fixrow");
				foreach (var x in fixedRows) {
					result.FixedRowCodes.Add(x.Attr("code"));
				}

				var docs = i.XPathSelectElements("./doc");
				foreach (var x in docs) {
					result.Documents[x.Attr("code")] = x.Attr("name");
				}


				//альтернативный вариант привязки кодов исключения
				var fixrowlist = i.Attr("fixrows", "").SmartSplit();
				foreach (var row in fixrowlist) {
					result.FixedRowCodes.Add(row);
				}


				var parameters = i.XPathSelectElements("./param");
				foreach (var x in parameters) {
					result.Parameters[x.Attr("name")] = x.Attr("value");
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
				var code = row.Attr("code");
				if (code.IsNotEmpty()) {
					var rd = new RowDescriptor(code);
					rd.Name = row.Attr("name", rd.Name);
					if (rd.Formula.IsEmpty()) {
						rd.Formula = row.Attr("formula", rd.Formula);
						if (rd.Formula.IsNotEmpty()) {
							rd.IsFormula = true;
							rd.FormulaType = row.Attr("formulatype", "boo");
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
					col.FormulaType = "boo";
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
								col.Year = v.ToInt();
								break;
							case "period":
								col.Period = v.ToInt();
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
								col.AutoCalc = v.ToBool();
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
								col.Visible = v.ToBool();
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
								col.UseObj = v.ToBool();
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
								col.UseThema = v.ToBool();
								break;
							case "valuetocssclass":
								col.ValueToCssClass = v.ToBool();
								break;
							case "controlpoint":
								col.ControlPoint = v.ToBool();
								break;
							case "evaluator":
								goto case "formulatype";
							case "formulatype":
								col.FormulaType = v;
								break;
							case "calcidx":
								col.CalcIdx = v.ToInt();
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
								col.ForPeriods = v.SmartSplit().Select(s => s.ToInt()).ToArray();
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
					col.RowCheckConditions = x.Elements("checkrule").Select(_=>_.Deserialize<ColumnRowCheckCondition>()).ToArray();


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