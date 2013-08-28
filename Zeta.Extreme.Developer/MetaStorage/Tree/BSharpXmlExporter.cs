﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree
{
	/// <summary>
	/// Экспорт дерева в BSharp- совместимый XML
	/// </summary>
	public class BSharpXmlExporter {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public XElement Export(IZetaRow root, string ns = "zeta.export.forms", string cls = null) {
			ns = string.IsNullOrWhiteSpace(ns) ? "zeta.export.forms" : ns;
			cls = string.IsNullOrWhiteSpace(cls) ? root.Code: cls;
			var nse = new XElement("namespace", new XAttribute("code", ns));
			var clse = new XElement("class", 
				new XAttribute("code", cls),
				new XAttribute("name",root.Name),
				new XAttribute("formcode",root.Code));
			nse.Add(clse);
			clse.Add(new XElement("import", new XAttribute("code", "tree")));
			GenerateRow(clse, root,root);
			return nse;
		}

		private void GenerateRow(XElement target, IZetaRow r,IZetaRow root) {
			var e = CreateElement(r,root);
			target.Add(e);
			foreach (var c in r.Children) {
				GenerateRow(e,c,root);
			}
		}
		
		private XElement CreateElement(IZetaRow r, IZetaRow root) {
			var marks = GetMarks(r).ToArray();
			var tags = GetTags(r);
			var type = DetermineType(r, marks, tags);
			var code = DetermineCode(r, root);
			var e = new XElement(type.ToString().ToLower());
			
			e.SetAttributeValue("code",code);
			e.SetAttributeValue("name",r.Name);
			return e;
		}

		private string DetermineCode(IZetaRow zetaRow, IZetaRow root) {
			if (zetaRow == root) return root.Code;
			var code1 = zetaRow.Code.ToLower();
			var code2 = root.Code.ToLower();
			if (code1.StartsWith(code2)) return code1.Substring(code2.Length);
			return code1;
		}

		private RowType DetermineType(IZetaRow r, string[] marks, IDictionary<string, string> tags) {
			if(marks.Contains("title"))return RowType.Title;
			if (marks.Contains("controlpoint")) return RowType.ControlPoint;
			if (r.IsFormula) return RowType.Formula;
			if (marks.Contains("sum")) return RowType.Sum;
			if(r.RefTo!=null||r.RefId!=null)return RowType.Ref;
			return RowType.Primary;
		}

		private enum RowType {
			Primary,
			Formula,
			ControlPoint,
			Ref,
			Sum,
			Title
		}

		private IDictionary<string,string> GetTags(IZetaRow root) {
			var basedict = TagHelper.Parse(root.Tag);
			var result = new Dictionary<string, string>();
			foreach (var v in basedict) {
				var map = DefaultBsMappings.DefaultRowTags.FirstOrDefault(_ => _.SourceName == v.Key);
				if (null == map) {
					result[v.Key] = v.Value;
				}
				else {
					var boolean = v.Value.ToBool();
					if (!map.Ignore) {
						if (map.Error) {
							result[v.Key] = "[error]";
						}
						
						else if (!string.IsNullOrWhiteSpace(map.Group)) {
							if (boolean) {
								var subval = v.Key.Substring(map.Group.Length + 1);
								if (!result.ContainsKey(map.Group)) {
									result[map.Group] = subval;
								}
								else {
									result[map.Group] += ", " + subval;
								}
							}
						}
						else {
							var n = v.Key;

							

							if (!string.IsNullOrWhiteSpace(map.Attribute)) {
								n = map.Attribute;
							}
							
							if (map.AttributeType == AttributeType.Bool) {
								if (boolean) {
									result[n] = "";
								}
							}else if (map.AttributeType == AttributeType.Bool10) {
								result[n] = boolean ? "1" : "0";
							}
							else {
								result[n] = v.Value;
							}
						}
					}
				}
			}
			return result;
		}

		private IEnumerable<string> GetMarks(IZetaRow root) {
			var marks = root.MarkCache.SmartSplit(false, true, '/');
			foreach (var m in marks) {
				var map = DefaultBsMappings.DefaultRowMarks.FirstOrDefault(_ => _.SourceName == m);
				if (null == map) {
					yield return m;
				}
				else {
					if (!map.Ignore) {
						if (map.Error) {
							yield return "error:" + m;
						}
						else {
							if (!string.IsNullOrWhiteSpace(map.Element)) {
								yield return map.Element;
							}else if (!string.IsNullOrWhiteSpace(map.Attribute)) {
								yield return map.Attribute;
							}
							else {
								yield return m;
							}
						}
					}
				}
			}
			
		}
	}
}
