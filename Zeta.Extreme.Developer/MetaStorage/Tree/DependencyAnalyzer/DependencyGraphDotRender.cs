using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Построитель графов на языке DOT
	/// </summary>
	public class DependencyGraphDotRender {
		private StringBuilder buffer;
		private DependencyGraph graph;
		private const string CLOSE = "\r\n}";
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
		private const string ATTR = "{0}={1};";
		private const string STRATTR = "{0}=\"{1}\"";
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
		public string Render(DependencyGraph g) {
			graph = g;
			buffer = new StringBuilder();
			WriteGraphStart();
			WriteAttr(RANKDIR,"LR");
			buffer.AppendLine(";");
			WriteNodes();
			WriteEdges();
			WriteGraphEnd();
			return buffer.ToString();
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
			buffer.AppendFormat(STRATTR, name, value);
		}

		private void WriteCluster(IGrouping<string, DependencyNode> nodes) {
			buffer.Append(SUBGRAPHSTART);
			buffer.Append(CLUSTERSTART);
			buffer.Append(nodes.Key);
			buffer.Append(OPEN);
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