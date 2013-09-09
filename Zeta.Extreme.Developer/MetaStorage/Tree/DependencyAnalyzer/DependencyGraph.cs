using System;
using System.Collections.Generic;
using Qorpent.Serialization;
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
        /// Базовый URI
        /// </summary>
        public Uri BaseUri { get; set; }
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
	    /// <param name="isfall"></param>
	    public DependencyEdge RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type, bool ignore, bool isfall=false)
		{
			bool isnew;
			return RegisterEdge(@from, to, type,ignore ,isfall,out isnew);
		}

	    /// <summary>
	    /// Регистрирует узел если отсутствует
	    /// </summary>
	    /// <param name="from"></param>
	    /// <param name="to"></param>
	    /// <param name="type"></param>
	    /// <param name="ignore"></param>
	    /// <param name="isfall"></param>
	    /// <param name="isnew"></param>
	    public DependencyEdge RegisterEdge(IZetaRow from, IZetaRow to, DependencyEdgeType type, bool ignore,bool isfall, out bool isnew) {
			isnew = false;
			var key = from.Code + "_" + to.Code + "_" + type;
			if (!Edges.ContainsKey(key)) {
			    var fromc = from.Code;
			    var toc = to.Code;
                if (ignore) {
                    if (isfall) {
                        fromc = "IGNORE";
                    }
                    else {
                        toc = "IGNORE";
                    }
                }
				isnew = true;
				var result = Edges[key] = new DependencyEdge {From =fromc, 
					To=toc,Type = type};
				if (ignore) {
                    if (isfall) {
                        result.Label = from.Code;
                    }
                    else {
                        result.Label = to.Code;
                    }
					
				}
				return result;
			}
			return Edges[key];
		}
	}
}