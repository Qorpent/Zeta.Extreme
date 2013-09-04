using System.Collections.Generic;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {


	/// <summary>
	/// Граф для зависимостей
	/// </summary>
	public class DependencyGraph {
		/// <summary>
		/// Граф зависимостей
		/// </summary>
		public DependencyGraph() {
			Nodes = new Dictionary<string,DependencyNode>();
			Edges = new Dictionary<string,DependencyEdge>();
		}
		/// <summary>
		/// Узлы
		/// </summary>
		public IDictionary<string,DependencyNode> Nodes { get; private set; }
		/// <summary>
		/// Узлы
		/// </summary>
		public IDictionary<string,DependencyEdge> Edges { get; private set; }
		/// <summary>
		/// Признак кластеризации строк по базовой форме
		/// </summary>
		public bool Clusterize { get; set; }
		/// <summary>
		/// Код графа
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// Показать легенду
		/// </summary>
		public bool ShowLegend { get; set; }

		/// <summary>
		/// Регистрирует нод в случае отсутствия
		/// </summary>
		/// <param name="row"></param>
		public DependencyNode RegisterNode(IZetaRow row) {
			var code = DependencyNode.GetDotCode(row);
			if (Nodes.ContainsKey(code)) return Nodes[code];
			return Nodes[code] = new DependencyNode(row);
		}

		/// <summary>
		/// Регистрирует узел если отсутствует
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="type"></param>
		/// <param name="ignore"></param>
		public DependencyEdge RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type, bool ignore)
		{
			bool isnew;
			return RegisterEdge(@from, to, type,ignore ,out isnew);
		}

		/// <summary>
		/// Регистрирует узел если отсутствует
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="type"></param>
		/// <param name="ignore"></param>
		/// <param name="isnew"></param>
		public DependencyEdge RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type, bool ignore, out bool isnew) {
			isnew = false;
			var key = from.Code + "_" + to.Code + "_" + type;
			if (!Edges.ContainsKey(key)) {
				isnew = true;
				var result = Edges[key] = new DependencyEdge {From = DependencyNode.GetDotCode(from), 
					To = ignore?"IGNORE": DependencyNode.GetDotCode(to),Type = type};
				if (result.To == "IGNORE") {
					result.Label = to.Code;
				}
				return result;
			}
			return Edges[key];
		}
	}
}