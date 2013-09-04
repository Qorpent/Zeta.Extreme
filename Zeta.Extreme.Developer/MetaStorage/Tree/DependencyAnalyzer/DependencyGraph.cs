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
		/// ������� ������������� ����� �� ������� �����
		/// </summary>
		public bool Clusterize { get; set; }
		/// <summary>
		/// ��� �����
		/// </summary>
		public string Code { get; set; }


		/// <summary>
		/// ������������ ��� � ������ ����������
		/// </summary>
		/// <param name="row"></param>
		public DependencyNode RegisterNode(IZetaRow row) {
			var code = DependencyNode.GetDotCode(row);
			if (Nodes.ContainsKey(code)) return Nodes[code];
			return Nodes[code] = new DependencyNode(row);
		}
		/// <summary>
		/// ������������ ���� ���� �����������
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="type"></param>
		public DependencyEdge RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type)
		{
			var key = from.Code + "_" + to.Code + "_" + type;
			if (!Edges.ContainsKey(key)) {
				return Edges[key] = new DependencyEdge {From = DependencyNode.GetDotCode(from), To = DependencyNode.GetDotCode(to)};
				
			}
			return Edges[key];
		}
	}
}