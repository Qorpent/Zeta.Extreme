using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Qorpent.Dot;
using Qorpent.Serialization.Graphs;
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
			return ConvertToDot(graph,false,r);
		}
		
		private static string ConvertToDot(OldDependencyGraph oldDependencyGraph, bool clustered, IZetaRow zetaRow) {
		    var gr = new Graph();
            gr.RankDir = RankDirType.LR;

			if (clustered) {
				var groups = oldDependencyGraph.Nodes.GroupBy(_ => _.Substring(2, 4));
				var i = 1;
				foreach (var g in groups) {
				    var sg = new SubGraph {Code = (i).ToString(),Label = g.Key};
				    gr.AddSubGraph(sg);
					foreach (var n in g) {
						RenderNode(n,gr,zetaRow,i);
					}

				    i++;
				}
			}
			else {
				foreach (var n in oldDependencyGraph.Nodes)
				{
					RenderNode(n, gr,zetaRow,0);
				}
			}
			foreach (var e in oldDependencyGraph.Edges) {
				if (null == e.Item1 || null == e.Item3) continue;
                if (e.Item2 == "cpt") continue; 
                var edge = new Edge {From = e.Item3, To = e.Item1, Color = Color.Black};
			    if (e.Item2 == "frm") {
                    edge.Color = Color.Blue;
				}
				if (e.Item2 == "ref" ) {
                    edge.Color = Color.Brown;
				}
                if (e.Item1 == zetaRow.Code || e.Item3 == zetaRow.Code) {
                    edge.Penwidth = 3;
                    edge.ArrowSize = 2;
                    edge.ArrowHead= ArrowType.Vee;
                    if (e.Item1 == zetaRow.Code) {
                        edge.Color = Color.Green;
                    }
                }else {
                    edge.Style = EdgeStyleType.Dashed;
                }
			    gr.AddEdge(edge);
			}
		    return GraphRender.Create(gr, new GraphOptions()).GenerateGraphScript();
		}

		private static void RenderNode(string n, Graph g, IZetaRow r, int i) {
			if (null == n) return;
		    var nod = new Node {Code = n};
            if (n == r.Code) {
                nod.Style = NodeStyleType.Filled;
                nod.FillColor = Color.Green;
                nod.FontColor = Color.White;
            }
            if (0 != i) {
                nod.SubgraphCode = i.ToString();
            }
			if (n.StartsWith("s_") || n.StartsWith("f_") || n.StartsWith("p_") || n.StartsWith("r_")) {

                nod.Label = n.Substring(2).Replace("_DOT_", ".");
                nod.Shape = NodeShapeType.Ellipse;

				if (n.StartsWith("s_")) {
					nod.Shape =NodeShapeType.Box3d;
				}
				else if (n.StartsWith("f_")) {
					nod.Shape = NodeShapeType.Egg;
				}
				else if (n.StartsWith("r_")) {
                    nod.Shape = NodeShapeType.Egg;
				}
				
			}
		    g.AddNode(nod);
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
                if (f.ExRefTo != null)
                {
                    if (f.ExRefTo.Code.Substring(0, 4) != exportroot.Code)
                    {
                        deplist.Add("ref:" + f.ExRefTo.Code);
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