using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Класс, выполняющий поиск зависимостей по формам
	/// </summary>
	public class FormDependencyHelper {
		/// <summary>
		/// Формирует скрипт для DOT с зависимостями
		/// </summary>
		/// <param name="r"></param>
		public static string GetDependencyDot(IZetaRow r) {
			var graph = GetDependencyGraph(r);
			return ConvertToDot(graph,false);
		}
		
		private static string ConvertToDot(OldDependencyGraph oldDependencyGraph, bool clustered) {
			var result = new StringBuilder();
			result.AppendLine("digraph G {");
			result.AppendLine("\trankdir=LR");
			if (clustered) {
				var groups = oldDependencyGraph.Nodes.GroupBy(_ => _.Substring(2, 4));
				var i = 0;
				foreach (var g in groups) {
					result.AppendLine("subgraph cluster_" + (i++) + "{");
					result.AppendLine("label=" + g.Key);
					result.AppendLine("color=blue");
					foreach (var n in g) {
						RenderNode(n,result);
					}

					result.AppendLine("}");
				}
			}
			else {
				foreach (var n in oldDependencyGraph.Nodes)
				{
					RenderNode(n, result);
				}
			}
			foreach (var e in oldDependencyGraph.Edges) {
				if (null == e.Item1 || null == e.Item3) continue;
				if (e.Item2 == "cpt") continue;
				var color = "black";
				if (e.Item2 == "cpt") {
					color = "red";
				}
				if (e.Item2 == "frm") {
					color = "blue";
				}
				if (e.Item2 == "ref" ) {
					color = "brown";
				}
				
				result.AppendLine("\t" + e.Item3 + "->" + e.Item1 + " [color=" + color + "]");
			}
			result.AppendLine("}");
			return result.ToString();
		}

		private static void RenderNode(string n, StringBuilder result) {
			if (null == n) return;
			if (n.StartsWith("s_") || n.StartsWith("f_") || n.StartsWith("p_") || n.StartsWith("r_")) {
				var label = n.Substring(2).Replace("_DOT_", ".");
				var shape = "ellipse";
				if (n.StartsWith("s_")) {
					shape = "box3d";
				}
				else if (n.StartsWith("f_")) {
					shape = "cds";
				}
				else if (n.StartsWith("r_")) {
					shape = "egg";
				}
				result.AppendLine("\t" + n + " [shape=" + shape + ";label=\"" + label + "\"]");
			}
			else {
				result.AppendLine("\t" + n);
			}
		}

	
		private static string GetRowCode(Query q) {
			var code = q.Row.Code;
			var prefix = "p";
			if (!q.Row.IsPrimary()) {
				if (q.Row.IsFormula) {
					prefix = "f";
				}
				else {
					prefix = "s";
				}
			}
			code = prefix + "_"+code;
			return code.Replace(".","_DOT_");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="root"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static OldDependencyGraph GetDependencyGraph(IZetaRow root,OldDependencyGraph index = null) {
			index = index ?? new OldDependencyGraph();
			if (index.Nodes.Contains(root.Code)) {
				return index;
			}
			index.Nodes.Add(root.Code);

			var deps = GetFormList(root);
			IList<string> childreq =new List<string>();
			foreach (var d in deps) {
				var typecode = d.Split(':');
				var t = new Tuple<string, string, string>(root.Code,typecode[0],typecode[1]);
				if (!index.Nodes.Contains(typecode[1])) {
					if (!childreq.Contains(typecode[1])) {
						childreq.Add(typecode[1]);
					}
				}
				if (!index.Edges.Contains(t)) {
					index.Edges.Add(t);
				}
			}
			foreach (var c in childreq) {
				var row = MetaCache.Default.Get<IZetaRow>(c);
				if (null != row) {
					GetDependencyGraph(row, index);
				}
			}

			return index;
		}

		

		/// <summary>
		/// Возвращает зависимости в строчной нотации
		/// </summary>
		/// <param name="exportroot"></param>
		/// <returns></returns>
		public static string[] GetFormList(IZetaRow exportroot) {
			var deplist = new List<string>();
			foreach (var f in new[] {exportroot}.Union(exportroot.AllChildren)) {
				if (f.RefTo != null) {
					if (f.RefTo.Code.Substring(0, 4) != exportroot.Code) {
						deplist.Add("ref:" + f.RefTo.Code);
					}
					continue;
				}
				if (f.IsFormula) {
					var type = "frm:";
					if (f.MarkCache.Contains("CONTROLPOINT")) {
						type = "cpt:";
						continue;
					}
					var codes = Regex.Matches(f.Formula, @"\$([\w_\d]+)")
					                 .OfType<Match>().Select(_ => _.Groups[1]).ToArray();
					foreach (var c in codes) {
						if (c.Value.Substring(0, 4) != exportroot.Code) {
							deplist.Add(type + c.Value);
						}
					}
				}
			}
			var formlist =
				deplist.Select(_ => _.Substring(0, 8)).Distinct().Where(_ => _ != exportroot.Code).OrderBy(_ => _).ToArray();
			return formlist;
		}
	}
}