using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Обходит строку вниз и строит граф для текущей указанной строки
	/// </summary>
	internal class FallDownDependencyVisitor {
		class StubPs : IPrimarySource
		{
			public void Register(IQuery query)
			{
				query.Result = new QueryResult(1);
			}

			public void Register(string preparedQuery)
			{

			}

			public Task Collect()
			{
				return Task.Run(() => { });
			}

			public void Wait(int timeout = -1)
			{

			}

			public IList<string> QueryLog { get; private set; }
		}
		/// <summary>
		/// Обходит текущий узел
		/// </summary>
		/// <returns></returns>
		public void Process(DependencyGraphTask task)
		{
			
			var session = new Session { PrimarySource = new StubPs(), ExpandConditionalFormulas = true };
			var query = new Query(task.StartRow.Code, "Б1", 352, 2013, 1);
			query = (Query)session.Register(query);
			session.WaitPreparation();
			InternalProcess(task, query, 0);
		}

		private void InternalProcess(DependencyGraphTask task, Query query, int level) {
			var incltype = task.GetIncludeType(query.Row.Native);
			if (incltype == IncludeType.None && level!=0) return;
			var node = task.ResultGraph.RegisterNode(query.Row.Native);
			if (level == 0) {
				node.IsTarget = true;
			}
			if (IncludeType.SelfAndDescendants == incltype) {
				if (node.Type != DependencyNodeType.Primary) {
					if (task.Depth == 0 || level < task.Depth) {
						ProcessDependencies(task, query, level + 1);
					}
					else {
						if (query.SummaDependency.Any() || query.FormulaDependency.Any()) {
							node.IsNotFullyLeveled = true;
						}
					}
				}
			}
			else {
				node.IsTerminal = true;
			}

		}

		private void ProcessDependencies(DependencyGraphTask task, Query query, int level) {
			InternalProcessDependences(task, query, level, DependencyEdgeType.Formula, query.FormulaDependency);
			InternalProcessDependences(task, query, level, DependencyEdgeType.Sum, query.SummaDependency.Select(_=>_.Item2));
		}

		private void InternalProcessDependences(DependencyGraphTask task, Query query, int level, DependencyEdgeType testtype,
		                                        IEnumerable<IQuery> collection) {
			if (task.EdgeTypes.HasFlag(testtype)) {
				foreach (Query f in collection) {
					if (task.GetIncludeType(f.Row.Native) != IncludeType.None) {
						task.ResultGraph.RegisterEdge(f.Row.Native, query.Row.Native, testtype, false,true);

					}
					else {
						if (task.NodeTypes.HasFlag(DependencyNode.GetNodeType(f.Row.Native))) {
						    task.ResultGraph.RegisterEdge(f.Row.Native, query.Row.Native, testtype, true,true);
						}

					}
				}
				foreach (Query f in collection) {
					InternalProcess(task, f, level);
				}
			}
		}
	}
}