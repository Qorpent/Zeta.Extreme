using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
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
		public XElement Export(IZetaRow root, string ns = "zeta.export.forms", string cls = null, bool dictmode = false, string valuerediect = null) {
			UseDict = dictmode;
			ValueRedirect = valuerediect;
			ns = string.IsNullOrWhiteSpace(ns) ? "zeta.export.forms" : ns;
			cls = string.IsNullOrWhiteSpace(cls) ? root.Code: cls;
			var nse = new XElement("namespace", new XAttribute("code", ns));
			var clse = new XElement("class", 
				new XAttribute("code", cls),
				new XAttribute("name",root.Name.Trim().Replace("\r","\\r").Replace("\n","\\n")),
				new XAttribute("formcode",root.Code));
			nse.Add(clse);
			clse.Add(new XElement("import", new XAttribute("code",UseDict? "dict":"tree")));
            if (UseDict) {
                clse.Add(new XElement("export",new XAttribute("code",cls.Replace("dir_","").Replace("dict_",""))));
            }
		    if (!UseDict) {
		        CollectDependency(clse, root);
		    }
		    GenerateRow(clse, root,root);
			return nse;
		}

	    private void CollectDependency(XElement clse, IZetaRow root) {
	        IList<string> codes = new List<string>();
            Action<string> regcode = c =>
            {
                if (string.IsNullOrWhiteSpace(c)) return;
                var b = c.Substring(0, 4);
                if (b == root.Code) return;
                if (!codes.Contains(b))
                {
                    codes.Add(b);
                }
            };
	        Action<IZetaRow> regrow = r => {
	            if (null == r) return;
	            var c = r.Code;
	            regcode(c);
	        };
            
            foreach (var r in root.AllChildren) {
                if(r.IsMarkSeted("CONTROLPOINT"))continue;
                regrow(r.RefTo);
                regrow(r.ExRefTo);
                if (r.IsFormula) {
                    foreach (Match m in Regex.Matches(r.Formula, @"\$([^\.@\?]+)")) {
                        var code = m.Groups[1].Value;
                        regcode(code);
                    }
                }
            }
            foreach (var d in codes) {
                clse.Add(new XElement("dependon",new XAttribute("code",d)));
            }
	    }

	    /// <summary>
		/// Перевод значения для словаря
		/// </summary>
		protected string ValueRedirect { get; set; }
		/// <summary>
		/// Признак режима словаря
		/// </summary>
		protected bool UseDict { get; set; }

		private void GenerateRow(XElement target, IZetaRow r,IZetaRow root) {
			var e = CreateElement(r,root);
			target.Add(e);
			foreach (var c in r.Children.OrderBy(_=>_.GetSortKey())) {
				GenerateRow(e,c,root);
			}
		}
		
		private XElement CreateElement(IZetaRow r, IZetaRow root) {
			var marks = GetMarks(r).ToArray();
			var tags = GetTags(r);
			var type = DetermineType(r, marks, tags);
			if (UseDict && type != RowType.Title) {
				type = RowType.Item;
			}
			var code = DetermineCode(r, root);
            
			var e = new XElement(type.ToString().ToLower());
            if (r != root && r.Code.ToLower() == code)
            {
                e.SetAttributeValue("notlocalizedcode",true);
            }
			e.SetAttributeValue("code",code);
			e.SetAttributeValue("name",r.Name.Trim().Replace("\r"," ").Replace("\n",""));

			if (!string.IsNullOrWhiteSpace(r.OuterCode) && code!=r.OuterCode) {
				e.SetAttributeValue("outer",r.OuterCode);
			}

			if (type == RowType.Formula || type==  RowType.ControlPoint) {
				if (!string.IsNullOrWhiteSpace(r.Formula)) {
					e.SetAttributeValue("formula", r.Formula);
					if (r.FormulaType != "boo" && r.FormulaType != "cs") {
						e.SetAttributeValue("formulatype", r.FormulaType);
					}
				}
			}

			if (r.RefTo != null || r.RefId != null) {
				e.SetAttributeValue("ref",r.RefTo==null?(object)r.RefId:r.RefTo.Code);
			}

			if (!string.IsNullOrWhiteSpace(r.GroupCache)) {
				e.SetAttributeValue("groups",string.Join(" ",r.GroupCache.SmartSplit()));
			}

			if (!string.IsNullOrWhiteSpace(r.Measure)) {
				e.SetAttributeValue("measure",r.Measure);
			}

			foreach (var m in marks) {
				if(m=="sum")continue;
				if (m == "controlpoint") continue;
				if (m == "title") continue;
				e.SetAttributeValue(m,"");
			}

			foreach (var t in tags) {
				e.SetAttributeValue(t.Key, t.Value);
			}

			if (type==RowType.Item && !string.IsNullOrWhiteSpace(ValueRedirect)) {
				var attr = e.Attribute(ValueRedirect);
				if (null != attr) {
					var val = attr.Value;	
					attr.Remove();
					if (!string.IsNullOrWhiteSpace(val)) {
						if (r.HasChildren()) {
							e.SetAttributeValue("value", val);
						}
						else {
							e.Value = val;
						}
					}
				}
				
				
			}

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
			if(r.HasChildren())return RowType.Title;
			return RowType.Primary;
		}

		private enum RowType {
			Primary,
			Formula,
			ControlPoint,
			Ref,
			Sum,
			Title,
			Item
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
							var n = v.Key.Trim();

							

							if (!string.IsNullOrWhiteSpace(map.Attribute)) {
								n = map.Attribute.Trim();
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
					yield return m.Trim();
				}
				else {
					if (!map.Ignore) {
						if (map.Error) {
							yield return "ERROR_" + m.Trim();
						}
						else {
							if (!string.IsNullOrWhiteSpace(map.Element)) {
								yield return map.Element.Trim();
							}else if (!string.IsNullOrWhiteSpace(map.Attribute)) {
								yield return map.Attribute.Trim();
							}
							else {
								yield return m.Trim();
							}
						}
					}
				}
			}
			
		}
	}
}
