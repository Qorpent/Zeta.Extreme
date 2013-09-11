using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
    internal class GoDownDependencyVisitor
    {

        /// <summary>
        /// Обходит текущий узел
        /// </summary>
        /// <returns></returns>
        public void Process(DependencyGraphTask task)
        {
            GetPrimaryDependencyGraph(task, task.StartRow, task.ResultGraph, 0);
        }
        IEnumerable<IZetaRow> GetReferencedRows(IZetaRow row)
        {
            if (row.RefTo != null) {
                yield return row.RefTo;
            }
            if (row.ExRefTo != null) {
                yield return row.ExRefTo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetPrimaryDependencyGraph(DependencyGraphTask task, IZetaRow row, DependencyGraph index, int level)
        {
            var include = task.GetIncludeType(row);
            if (include == IncludeType.None && level != 0) return;
            var n = index.RegisterNode(row);
            if (level == 0)
            {
                n.IsTarget = true;
            }
            if (include != IncludeType.Self)
            {
                if (task.Depth == 0 || level < task.Depth)
                {
                    ProcessRows(task, row, index, level, DependencyEdgeType.Ref, n, GetReferencedRows);
                    if (!row.IsMarkSeted("0NOSUM"))
                    {
                        ProcessRows(task, row, index, level, DependencyEdgeType.Sum, n, GetUsualSums);
                        ProcessRows(task, row, index, level, DependencyEdgeType.Sum, n, GetGroupSums);
                    }
                    ProcessRows(task, row, index, level, DependencyEdgeType.Formula, n, GetFormulas);
                }
                else
                {
                    if (HasDependent(task, row))
                    {
                        n.IsNotFullyLeveled = true;
                    }
                }
            }
            else
            {
                n.IsTerminal = true;
            }


        }

        private bool HasDependent(DependencyGraphTask task, IZetaRow row)
        {
            if (task.EdgeTypes.HasFlag(DependencyEdgeType.Ref))
            {
                if (GetReferencedRows(row).Any()) return true;
            }
            if (!row.IsMarkSeted("0NOSUM"))
            {
                if (task.EdgeTypes.HasFlag(DependencyEdgeType.Sum))
                {
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

        private IEnumerable<IZetaRow> GetFormulas(IZetaRow row)
        {
            if (row.IsFormula) {
                var codematches = Regex.Matches(row.Formula, @"\$([^\@\.\?]+)");
                IList<IZetaRow> rows = new List<IZetaRow>();
                foreach (Match m in codematches) {
                    var code = m.Groups[1].Value;
                    var r = RowCache.get(code);
                    if (null != r) {
                        rows.Add(r);
                    }
                }
                foreach (var r in rows.Distinct()) {
                    yield return r;
                }
            }
        }

        private IEnumerable<IZetaRow> GetGroupSums(IZetaRow row)
        {
            if (row.IsMarkSeted("0SA") && !string.IsNullOrWhiteSpace(row.GroupCache)) {
                var groups = row.GroupCache.SmartSplit(false, true, ' ', ';', '/');
                var grsums =
                    RowCache.Bycode.Values.Where(_ => !string.IsNullOrWhiteSpace(_.GroupCache) ).ToArray();
                foreach (var gs in grsums)
                {
                    if (gs.Code == row.Code) continue;
                    var tgroups = gs.GroupCache.SmartSplit(false, true, ' ', ';', '/');
                    if (tgroups.Intersect(groups).Any())
                    {
                        yield return gs;
                    }
                }
            }
        }

        private IEnumerable<IZetaRow> GetUsualSums(IZetaRow row)
        {
            if (row.IsMarkSeted("0SA") && string.IsNullOrWhiteSpace(row.GroupCache)) {
                foreach (var r in row.Children.Where(_ => !_.IsMarkSeted("0CAPTION") && !_.IsMarkSeted("0NOSUM")).ToArray()) {
                    yield return r;
                }
                
            }
        }

        private void ProcessRows(DependencyGraphTask task, IZetaRow row, DependencyGraph index, int level, DependencyEdgeType type, DependencyNode dependencyNode, Func<IZetaRow, IEnumerable<IZetaRow>> getrows)
        {
            if (task.EdgeTypes.HasFlag(type))
            {
                foreach (var r in getrows(row))
                {
                    if (task.GetIncludeType(r) != IncludeType.None)
                    {
                        if (r.Code == row.Code) continue;
                        bool isnew;
                        index.RegisterEdge(r, row, type, false, false, out isnew);
                        if (isnew)
                        {
                            GetPrimaryDependencyGraph(task, r, index, level + 1);
                        }
                    }
                    else
                    {
                        if (task.NodeTypes.HasFlag(DependencyNode.GetNodeType(r)))
                        {
                            index.RegisterEdge( r,row, type, true,true);
                        }
                    }
                }
            }
        }
    }
}