using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	internal class GoUpDependencyVisitor {

		/// <summary>
		/// Обходит текущий узел
		/// </summary>
		/// <returns></returns>
		public void Process(DependencyGraphTask task) {
			GetPrimaryDependencyGraph(task,task.StartRow, task.ResultGraph,0);
		}
		IEnumerable<IZetaRow> GetReferencedRows(IZetaRow row) {
			return RowCache.Bycode.Values.Where(
							_ =>
							(null != _.RefTo && _.RefTo.Code == row.Code)
							||
							(null != _.ExRefTo && _.ExRefTo.Code == row.Code)
							).ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public  void GetPrimaryDependencyGraph(DependencyGraphTask task, IZetaRow row, DependencyGraph index,int level) {
			var include = task.GetIncludeType(row);
            if (include == IncludeType.None && level != 0) return;
			var n = index.RegisterNode(row);
			if (level == 0) {
				n.IsTarget = true;
			}
			if (include != IncludeType.Self) {
				if (task.Depth == 0 || level < task.Depth) {
					ProcessRows(task, row, index, level, DependencyEdgeType.Ref, n, GetReferencedRows);
					if (!row.IsMarkSeted("0NOSUM")) {
						ProcessRows(task, row, index, level, DependencyEdgeType.Sum, n, GetUsualSums);
						ProcessRows(task, row, index, level, DependencyEdgeType.Sum, n, GetGroupSums);
					}
					ProcessRows(task, row, index, level, DependencyEdgeType.Formula, n, GetFormulas);
				}
				else {
					if (HasDependent(task, row)) {
						n.IsNotFullyLeveled = true;
					}
				}
			}
			else {
				n.IsTerminal = true;
			}


		}

		private bool HasDependent(DependencyGraphTask task, IZetaRow row) {
			if (task.EdgeTypes.HasFlag(DependencyEdgeType.Ref)) {
				if (GetReferencedRows(row).Any()) return true;
			}
			if (!row.IsMarkSeted("0NOSUM")) {
				if (task.EdgeTypes.HasFlag(DependencyEdgeType.Sum)) {
					if (GetUsualSums(row).Any()) return true;
					if (GetGroupSums(row).Any()) return true;
				}
			}
			if (task.EdgeTypes.HasFlag(DependencyEdgeType.Formula))
			{
				if (GetFormulas(row).Any()) return true;
			}
			return false;
		}

		private IEnumerable<IZetaRow> GetFormulas(IZetaRow row) {
			var formulas = RowCache.Formulas.Where(_ => _.Formula.Contains("$" + row.Code)).ToArray();
			return formulas.Where(f => Regex.IsMatch(f.Formula, @"\$" + row.Code + @"[\@\.\?]"));
		}

		private IEnumerable<IZetaRow> GetGroupSums(IZetaRow row) {
			if (!string.IsNullOrWhiteSpace(row.GroupCache)) {
				var groups = row.GroupCache.SmartSplit(false, true, ' ', ';', '/');
				var grsums =
					RowCache.Bycode.Values.Where(_ => !string.IsNullOrWhiteSpace(_.GroupCache) && _.IsMarkSeted("0SA")).ToArray();
				foreach (var gs in grsums) {
					if (gs.Code == row.Code) continue;
					var tgroups = gs.GroupCache.SmartSplit(false, true, ' ', ';', '/');
					if (tgroups.Intersect(groups).Any()) {
						yield return gs;
					}
				}
			}
		}

		private IEnumerable<IZetaRow> GetUsualSums(IZetaRow row) {
			var current = row.Parent;
			while (null != current)
			{
				if (current.IsMarkSeted("0SA"))
				{
					break;
				}
				current = current.Parent;
			}
			if (null != current) {
				yield return current;
			}
		}

		private void ProcessRows(DependencyGraphTask task, IZetaRow row, DependencyGraph index, int level, DependencyEdgeType type, DependencyNode dependencyNode, Func<IZetaRow,IEnumerable<IZetaRow>> getrows) {
			if (task.EdgeTypes.HasFlag(type)) {
				foreach (var r in getrows(row)) {
					if (task.GetIncludeType(r) != IncludeType.None) {
						if (r.Code == row.Code) continue;
						bool isnew;
						index.RegisterEdge(row, r, type, false,false, out isnew);
						if (isnew) {
							GetPrimaryDependencyGraph(task, r, index, level + 1);
						}
					}
					else {
						if (task.NodeTypes.HasFlag(DependencyNode.GetNodeType(r))) {
							index.RegisterEdge(row, r, type, true);
						}
					}
				}
			}
		}
	}
}