using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Построитель графов на языке DOT
	/// </summary>
	public class DependencyGraphDotRender {
		private StringBuilder buffer;
		private DependencyGraph graph;
		private bool hasignore;
		private Uri uri;
		private const string CLOSE = "\r\n}\r\n";
		private const string OPEN = "{\r\n";
		private const string GRAPHSTART = "digraph ";
		private const string SUBGRAPHSTART = "subgraph ";
		private const string CLUSTERSTART = "cluster_";
		private const string WS = " ";
		private const string EDGE = " -> ";
		private const string LABEL = "label";
		private const string SHAPE = "shape";
		private const string COLOR = "color";
		private const string RANKDIR = "rankdir";
		private const string ATTR = "{0}={1}; ";
		private const string STRATTR = "{0}=\"{1}\"; ";
		private const string AOPEN = "[";
		private const string ACLOSE = "]";
		private const string PRIMARYSHAPE = "ellipse";
		private const string SUMSHAPE = "box3d";
		private const string FORMULASHAPE = "cds";
		private const string REFSHAPE = "egg";


		/// <summary>
		/// Построитель графов на языке Dot
		/// </summary>
		/// <returns></returns>
		public string Render(DependencyGraph g, Uri u = null) {
			graph = g;
			uri = u;
			hasignore = graph.Edges.Values.Any(_ => _.To == "IGNORE");
			buffer = new StringBuilder();
			WriteGraphStart();
			WriteAttr(RANKDIR,"LR");
			WriteAttr("fontname","Tahoma");
			WriteAttr("fontsize","9");
			buffer.AppendLine("node[fontsize=9]");
			buffer.AppendLine();
			WriteLegend();
			WriteNodes();
			WriteEdges();
			WriteGraphEnd();
			return buffer.ToString();
		}

		private void WriteLegend() {
			buffer.Append(SUBGRAPHSTART);
			buffer.Append(CLUSTERSTART);
			buffer.Append("legend");
			buffer.Append(OPEN);
			WriteStrAttr(LABEL, "Легенда");
			WriteAttr("style","filled");
			WriteAttr("fillcolor", "cornsilk");
			buffer.Append(SUBGRAPHSTART);
			buffer.Append(CLUSTERSTART);
			buffer.Append("legend_colors");
			buffer.Append(OPEN);
			WriteStrAttr(LABEL, "Цвета");
			WriteNode(new DependencyNode {Code = "lc_usual", Label = "Обычный", Type = DependencyNodeType.Sum});
			WriteNode( new DependencyNode { Code = "lc_target", Label = "Целевая строка", Type = DependencyNodeType.Sum ,IsTarget = true});
			WriteNode(new DependencyNode { Code = "lc_level", Label = "Ограничено уровнем", Type = DependencyNodeType.Sum, IsNotFullyLeveled = true });
			WriteNode(new DependencyNode { Code = "lc_terminal", Label = "Ограничено как терминал", Type = DependencyNodeType.Sum, IsTerminal = true });
			buffer.AppendLine("lc_IGNORE [shape=circle;style=filled;fillcolor=red;label=\"Игнор.\"];");
			buffer.Append(CLOSE);

			buffer.Append(SUBGRAPHSTART);
			buffer.Append(CLUSTERSTART);
			buffer.Append("legend_forms");
			buffer.Append(OPEN);
			WriteStrAttr(LABEL, "Формы");
			WriteNode(new DependencyNode { Code = "lf_formula", Label = "Формула", Type = DependencyNodeType.Formula });
			WriteNode(new DependencyNode { Code = "lf_ref", Label = "Ссылка", Type = DependencyNodeType.Ref });
			WriteNode(new DependencyNode { Code = "lc_sum", Label = "Сумма", Type = DependencyNodeType.Sum });
			WriteNode(new DependencyNode { Code = "lc_prim", Label = "Первичная", Type = DependencyNodeType.Primary });
			buffer.Append(CLOSE);

			buffer.AppendLine("lc_usual->lf_formula[color=none];");

			buffer.Append(CLOSE);
		}

		private void WriteEdges() {
			foreach (var e in graph.Edges.Values) {
				WriteEdge(e);
			}
		}

		private void WriteEdge(DependencyEdge e) {
			buffer.AppendLine();
			buffer.Append(e.From);
			buffer.Append(EDGE);
			buffer.Append(e.To);
			buffer.Append(AOPEN);
			WriteEdgeAttributes(e);
			buffer.Append(ACLOSE);
		}

		private void WriteEdgeAttributes(DependencyEdge e) {
			WriteAttr(COLOR,GetColor(e));
			if (!string.IsNullOrWhiteSpace(e.Label)) {
				WriteStrAttr(LABEL,e.Label);
			}
		}

		private string GetColor(DependencyEdge e) {
			if (e.Type == DependencyEdgeType.Formula) {
				return "blue";
			}
			if (e.Type == DependencyEdgeType.Sum) {
				return "black";
			}
			if (e.Type == DependencyEdgeType.Ref)
			{
				return "brown";
			}
			if (e.Type == DependencyEdgeType.ExRef)
			{
				return "pink";
			}
			return "black";
		}


		private void WriteNodes() {
			if (hasignore) {
				buffer.AppendLine("IGNORE [shape=circle;style=filled;fillcolor=red;label=\"\"];");
			}
			IEnumerable<DependencyNode> nodes = graph.Nodes.Values;
			if (graph.Clusterize) {
				var gnodes = nodes.GroupBy(_ => _.BaseCode);
				foreach (var g in gnodes) {
					WriteCluster(g);
				}
			}
			else {
				foreach (var n in nodes) {
					WriteNode(n);
				}
			}
		}

		private void WriteNode(DependencyNode n) {
			buffer.AppendLine();
			buffer.Append(n.Code);
			buffer.Append(WS);
			buffer.Append(AOPEN);
			WriteNodeAttributes(n);
			
			buffer.Append(ACLOSE);

		}

		private void WriteNodeAttributes(DependencyNode n) {
			WriteStrAttr(LABEL,n.Label);
			WriteAttr(SHAPE,GetShape(n));
			if (n.IsTarget || n.IsNotFullyLeveled || n.IsTerminal) {
				WriteAttr("style","filled");
				if (n.IsTarget) {
					WriteAttr("fillcolor", "green1");
				}else if (n.IsTerminal) {
					WriteAttr("fillcolor","pink");

				}else if (n.IsNotFullyLeveled) {
					WriteAttr("fillcolor","yellow");
				}
			}
			if (!string.IsNullOrWhiteSpace(n.Tooltip)) {
				WriteStrAttr("tooltip",n.Tooltip);
			}
			if (null != uri) {
				var newuri = Regex.Replace(uri.ToString(), @"(root=)([^&]+)", "$1" + n.Label);
				WriteStrAttr("target","_blank");
				WriteStrAttr("href", Uri.EscapeUriString(newuri).Replace("+","%2B"));
			}
		}

		private string GetShape(DependencyNode n) {
			if (n.Type == DependencyNodeType.Sum) return SUMSHAPE;
			if (n.Type == DependencyNodeType.Formula) return FORMULASHAPE;
			if (n.Type == DependencyNodeType.Ref) return REFSHAPE;
			return PRIMARYSHAPE;
		}

		private void WriteAttr(string name, string value) {
			buffer.AppendFormat(ATTR, name, value);
		}
		private void WriteStrAttr(string name, string value)
		{

			buffer.AppendFormat(STRATTR, name, value.Trim().Replace("\"","&quot;"));
		}

		private void WriteCluster(IGrouping<string, DependencyNode> nodes) {
			buffer.Append(SUBGRAPHSTART);
			buffer.Append(CLUSTERSTART);
			buffer.Append(nodes.Key);
			buffer.Append(OPEN);
			WriteStrAttr(LABEL,nodes.Key);
			var clnode = MetaCache.Default.Get<IZetaRow>(nodes.Key);
			if (null != clnode) {
				WriteStrAttr("tooltip",clnode.Name);
			}
			foreach (var n in nodes) {
				WriteNode(n);
			}
			buffer.Append(CLOSE);
		}

		private void WriteGraphEnd() {
			buffer.Append(CLOSE);
		}

		private void WriteGraphStart() {
			buffer.Append(GRAPHSTART);
			buffer.Append(graph.Code);
			buffer.Append(OPEN);

		}
	}
}