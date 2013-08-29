using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Класс, выполняющий поиск зависимостей по формам
	/// </summary>
	public class FormDependencyHelper {
		/// <summary>
		/// Промежуточный индекс узлов
		/// </summary>
		public class GraphIndex {
			/// <summary>
			/// 
			/// </summary>
			public GraphIndex() {
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


		/// <summary>
		/// Формирует скрипт для DOT с зависимостями
		/// </summary>
		/// <param name="r"></param>
		public static string GetDependencyDot(IZetaRow r) {
			var graph = GetDependencyGraph(r);
			return ConvertToDot(graph);
		}
		/// <summary>
		/// Формирует скрипт для DOT с зависимостями
		/// </summary>
		/// <param name="r"></param>
		public static string GetFormulaDependencyDot(IZetaRow r)
		{
			var graph = GetFormulaDependencyGraph(r);
			return ConvertToDot(graph);
		}
		private static string ConvertToDot(GraphIndex graph) {
			var result = new StringBuilder();
			result.AppendLine("digraph G {");
			result.AppendLine("\trankdir=LR");
			foreach (var n in graph.Nodes) {
				if (n.StartsWith("s_") || n.StartsWith("f_") || n.StartsWith("p_")) {
					var label = n.Substring(2).Replace("_DOT_",".");
					var shape = "ellipse";
					if (n.StartsWith("s_")) {
						shape = "box3d";
					}
					else if (n.StartsWith("f_")) {
						shape = "cds";
					}
					result.AppendLine("\t" + n + " [shape=" + shape + ";label=\"" + label + "\"]");
				}
				else {
					result.AppendLine("\t" + n);
				}
			}
			foreach (var e in graph.Edges) {
				if (e.Item2 == "cpt") continue;
				var color = "black";
				if (e.Item2 == "cpt") {
					color = "red";
				}
				if (e.Item2 == "frm") {
					color = "blue";
				}
				if (e.Item2 == "ref" || e.Item2=="sum") {
					color = "brown";
				}
				result.AppendLine("\t" + e.Item1 + "->" + e.Item3 + " [color=" + color + "]");
			}
			result.AppendLine("}");
			return result.ToString();
		}

		class StubPs : IPrimarySource {
			public void Register(IQuery query) {
				query.Result = new QueryResult(1);
			}

			public void Register(string preparedQuery) {
				
			}

			public Task Collect() {
				return Task.Run(() => { });
			}

			public void Wait(int timeout = -1) {
	
			}

			public IList<string> QueryLog { get; private set; }
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static GraphIndex GetFormulaDependencyGraph(IZetaRow root, int objid=352,string colcode= "Б1", int year=2012,int period=1, bool expandifs= true) {
			var session = new Session {PrimarySource = new StubPs(),ExpandConditionalFormulas=expandifs};
			var query = new Query(root.Code, colcode, objid, year, period);
			query = (Query)session.Register(query);
			session.WaitPreparation();
			return GetQueryRowDependencyGraph(query,new GraphIndex());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="q"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static GraphIndex GetQueryRowDependencyGraph(Query q, GraphIndex index=null) {
			index = index ?? new GraphIndex();
			var code = GetRowCode(q);
			if (!index.Nodes.Contains(code)) {
				index.Nodes.Add(code);
			}
			if (q.IsPrimary) return index;
			if (null != q.FormulaDependency) {
				foreach (Query cq in q.FormulaDependency) {
					var tuple = new Tuple<string, string, string>(code, "frm",GetRowCode(cq));
					if (!index.Edges.Contains(tuple)) {
						index.Edges.Add(tuple);
					}
				}
			}
			if (null != q.SummaDependency) {
				foreach (Query cq in q.SummaDependency.Select(_ => _.Item2))
				{
					var tuple = new Tuple<string, string, string>(code, "sum", GetRowCode(cq));
					if (!index.Edges.Contains(tuple))
					{
						index.Edges.Add(tuple);
					}
				}
			}

			if (null != q.FormulaDependency)
			{
				foreach (Query cq in q.FormulaDependency) {
					GetQueryRowDependencyGraph(cq, index);
				}
			}
			if (null != q.SummaDependency)
			{
				foreach (Query cq in q.SummaDependency.Select(_=>_.Item2))
				{
					GetQueryRowDependencyGraph(cq, index);
				}
			}

			return index;
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
		public static GraphIndex GetDependencyGraph(IZetaRow root,GraphIndex index = null) {
			index = index ?? new GraphIndex();
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