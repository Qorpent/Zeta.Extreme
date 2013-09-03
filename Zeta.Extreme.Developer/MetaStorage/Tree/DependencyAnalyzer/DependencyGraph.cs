using System.Collections.Generic;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// ���� ��� ������������
	/// </summary>
	public class DependencyGraph {
		/// <summary>
		/// ���� ������������
		/// </summary>
		public DependencyGraph() {
			Nodes = new Dictionary<string,DependencyNode>();
			Edges = new Dictionary<string,DependencyEdge>();
		}
		/// <summary>
		/// ����
		/// </summary>
		public IDictionary<string,DependencyNode> Nodes { get; private set; }
		/// <summary>
		/// ����
		/// </summary>
		public IDictionary<string,DependencyEdge> Edges { get; private set; }

		/// <summary>
		/// ������������ ��� � ������ ����������
		/// </summary>
		/// <param name="row"></param>
		public bool RegisterNode(IZetaRow row) {
			var code = DependencyNode.GetDotCode(row);
			if (Nodes.ContainsKey(code)) return false;
			Nodes[code] = new DependencyNode(row);
			return true;
		}
		/// <summary>
		/// ������������ ���� ���� �����������
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="type"></param>
		public bool RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type) {
			var key = from.Code + "_" + to.Code + "_" + type;
			if (!Edges.ContainsKey(key)) {
				Edges[key] = new DependencyEdge {From = DependencyNode.GetDotCode(from), To = DependencyNode.GetDotCode(to)};
				return true;
			}
			return false;
		}
	}
}