using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Экспортер дерева в BSharp
	/// </summary>
	public class BSharpTreeExporter :ITreeExporter {
		private StringBuilder sb;

		private const string BIG_COMMENT_START =
			@"####################################################################################################
####                                                                                            ####";
		private const string BIG_COMMENT_END =
			@"####                                                                                            ####
####################################################################################################";

		private const string CLS_TAB = "\t";
		private const string CLS_CNT_TAB = "\t\t";

		private const string BIG_COMMENT_LINE = "####      {0,-80}      ####";
		private const string BIG_COMMENT_LINE_DOUBLE = "####      {0,-43}: {1,-35}      ####";

		/// <summary>
		/// Выполняет экспорт дерева в строку
		/// </summary>
		/// <param name="exportroot"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public string ProcessExport(IZetaRow exportroot, TreeExporterOptions options = null) {
			var xml = new BSharpXmlExporter().Export(exportroot, options.Namespace, options.ClassName);
			return ConvertXmlToBSharp(exportroot,xml, options);
		}

		private void BeginBigComment() {
			sb.AppendLine(BIG_COMMENT_START);
		}
		private void EndBigComment() {
			sb.AppendLine(BIG_COMMENT_END);
		}
		private void AddBigComment(string first, string second = null) {
			if (string.IsNullOrWhiteSpace(second)) {
				sb.AppendLine(string.Format(BIG_COMMENT_LINE, first));
			}
			else {
				sb.AppendLine(string.Format(BIG_COMMENT_LINE_DOUBLE, first,second));
			}
			
		}

		/// <summary>
		/// Конвертирует XML в BSharp
		/// </summary>
		/// <param name="exportroot"></param>
		/// <param name="xml"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public string ConvertXmlToBSharp(IZetaRow exportroot,XElement xml, TreeExporterOptions options) {
			sb = new StringBuilder();
			RenderComment(exportroot, options);
			RenderClassStart(xml);
			RenderClassContent(xml.Element("class").Elements().ElementAt(1),0);
			return sb.ToString();
		}

		private void RenderClassContent(XElement e,int level) {
			RenderSelfClass(e, level);

			foreach (var c in e.Elements()) {
				RenderClassContent(c, level+1);
			}
		}

		private void RenderSelfClass(XElement e, int level) {
			sb.AppendLine();
			DoTab(level);
			sb.Append(e.Name.LocalName);
			sb.Append(" ");
			sb.Append(e.Attr("code"));
			sb.Append(" '" + e.Attr("name").Replace("'", "\\'") + "' ");
			foreach (var a in e.Attributes()) {
				if (a.Name.LocalName == "formula") continue;
				if (a.Name.LocalName == "code") continue;
				if (a.Name.LocalName == "name") continue;
				if (a.Value == "") {
					sb.Append(a.Name.LocalName);
					sb.Append(" ");
				}
				else {
					if (a.Value.All(_ => char.IsLetterOrDigit(_) || _ == '.')) {
						sb.Append(a.Name.LocalName);
						sb.Append("=");
						sb.Append(a.Value);
						sb.Append(" ");
					}
					else {
						sb.Append(a.Name.LocalName);
						sb.Append("='");
						sb.Append(a.Value.Replace("'","\\'"));
						sb.Append("' ");
					}
				}
			}

			var f = e.Attribute("formula");
			if (null != f) {
				if (e.Elements().Any()) {
					sb.Append(" formula=(");
				}
				else {
					sb.Append(" : (");
				}
				sb.AppendLine();
				DoTab(level + 1);
				sb.Append(f.Value);
				sb.AppendLine();
				DoTab(level);
				sb.Append(")");
			}
		}

		private void DoTab(int level) {
			sb.Append(CLS_CNT_TAB);
			for (var i = 0; i < level; i++) {
				sb.Append("\t");
			}
		}

		private void RenderClassStart(XElement xml) {
			var cls = xml.Element("class");
			sb.AppendFormat("namespace {0}", xml.Attr("code"));
			sb.AppendLine();
			sb.AppendFormat("{0}class {1} '{2}' formcode={3}", CLS_TAB, cls.Attr("code"), cls.Attr("name"), cls.Attr("formcode"));
			sb.AppendLine();
			sb.AppendFormat("{0}import {1}", CLS_CNT_TAB, cls.Element("import").Attr("code"));
		}

		private void RenderComment(IZetaRow exportroot, TreeExporterOptions options) {
			BeginBigComment();
			AddBigComment("Экспорт формы из БД Zeta");
			AddBigComment("");
			AddBigComment("Исходный код корневой строки формы", exportroot.Code);
			AddBigComment("Заданное пространство имен", options.Namespace);
			AddBigComment("Заданное имя класса", options.ClassName);
			AddBigComment("Исполнитель", Application.Current.Principal.CurrentUser.Identity.Name);
			AddBigComment("Время генерации", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			AddBigComment("ID строки в БД", exportroot.Id.ToString());
			AddBigComment("Максимальная версия строк в форме", new[] { exportroot }.Union(exportroot.AllChildren).Max(_ => _.Version).ToString("yyyy-MM-dd HH:mm:ss"));

			WriteOutReferencesComment(exportroot);

			EndBigComment();
			sb.AppendLine();
		}

		private void WriteOutReferencesComment(IZetaRow exportroot) {
			var deplist = new List<string>();
			foreach (var f in new[] {exportroot}.Union(exportroot.AllChildren)) {
				if (f.RefTo != null) {
					if (f.RefTo.Code != exportroot.Code) {
						deplist.Add("ref:" + f.RefTo.Code);
						
					}
					continue;
				}
				if (f.IsFormula) {
					var type = "frm:";
					if (f.MarkCache.Contains("CONTROLPOINT")) {
						type = "cpt:";
					}
					var codes = Regex.Matches(f.Formula, @"\$([\w_\d]+)")
					                 .OfType<Match>().Select(_ => _.Groups[1]).ToArray();
					foreach (var c in codes) {
						if (c.Value != exportroot.Code) {
							deplist.Add(type + c.Value);
						}
					}
				}
			}
			var formlist = deplist.Select(_ => _.Substring(0, 8)).Distinct().Where(_=>_!=exportroot.Code).OrderBy(_ => _).ToArray();
			if (0 != formlist.Length) {
				AddBigComment("");
				AddBigComment("Обнаружены зависимости от других форм");
				foreach (var f in formlist) {
					AddBigComment("        "+f);
				}
			}
		}
	}
}