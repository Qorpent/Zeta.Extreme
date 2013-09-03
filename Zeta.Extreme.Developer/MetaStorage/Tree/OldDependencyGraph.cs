using System;
using System.Collections.Generic;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Промежуточный индекс узлов
	/// </summary>
	public class OldDependencyGraph {
		/// <summary>
		/// 
		/// </summary>
		public OldDependencyGraph() {
			Nodes = new List<string>();
			Edges = new List<Tuple<string, string, string>>();
		}
		/// <summary>
		/// Узлы
		/// </summary>
		public IList<string> Nodes { get; private set; }
		/// <summary>
		/// Ребра
		/// </summary>
		public IList<Tuple<string, string,string>> Edges { get; private set; }
	}
}