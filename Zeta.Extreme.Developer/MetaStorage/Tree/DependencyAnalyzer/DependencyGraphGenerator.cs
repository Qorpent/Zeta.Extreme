using System;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Dot;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
    /// <summary>
    /// Преобразует внутренний граф зависимостей в DOT-совместимый граф
    /// </summary>
    public class DependencyGraphGenerator {
        private Graph graph;
        private DependencyGraph srcgraph;
        private Uri uri;
        private bool hasignore;
        /// <summary>
        /// Формирует из графа завимисомтией DOT-graph
        /// </summary>
        /// <param name="s"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public Graph Generate(DependencyGraph s,Uri u) {
            srcgraph = s;
            uri = u;
            hasignore = srcgraph.Edges.Values.Any(_ => _.To == "IGNORE");
            graph = new Graph {
                Code = s.Code,
                RankDir = RankDirType.LR,
                FontName = "Tahoma",
                DefaultNode = new Node {
                    FontSize = 9,
                    ColorScheme = "set312",
                    Style = NodeStyleType.Filled,
                    FillColor = "2",
                }
            };
            MoveNodes();
            MoveEdges();
            graph.Compactize();
            return graph;
        }

        private void MoveEdges() {
            foreach (var e in srcgraph.Edges.Values) {
                var de = new Edge {From = e.From, To = e.To, Data = e, Color = GetColor(e), Label = e.Label};
                graph.AddEdge(de);
            }
        }

        private void MoveNodes() {
            PrepareIgnoreNode();
            foreach (var n in srcgraph.Nodes.Values) {
                MoveNode(n);
            }
        }

        private void MoveNode(DependencyNode n) {
            var dn = new Node {Code = n.Code,Label = n.Label, Shape = GetShape(n)};
            graph.Nodes.Add(dn);
            CheckSpecialNodeType(n, dn);
            CheckWebAttributes(n, dn);
            CheckCluster(n, dn);
        }

        private void CheckWebAttributes(DependencyNode n, Node dn) {
            if (!string.IsNullOrWhiteSpace(n.Tooltip)) {
                dn.Tooltip = n.Tooltip;
            }
            if (null != uri) {
                var newuri = Regex.Replace(uri.ToString(), @"(root=)([^&]+)", "$1" + n.Label);
                dn.Target = "_blank";
                dn.Href = newuri;
            }
        }

        private static void CheckSpecialNodeType(DependencyNode n, Node dn) {
            if (n.IsTarget || n.IsNotFullyLeveled || n.IsTerminal) {
                if (n.IsTarget) {
                    dn.FillColor = "7";
                }
                else if (n.IsTerminal) {
                    dn.FillColor = "9";
                }
                else if (n.IsNotFullyLeveled) {
                    dn.FillColor = "12";
                }
            }
        }

        private void CheckCluster(DependencyNode n, Node dn) {
            if (srcgraph.Clusterize) {
                dn.SubgraphCode = DotLanguageUtils.GetClusterCode( n.BaseCode);
                if (null == graph.ResolveSubgraph(dn.SubgraphCode)) {
                    var sg = new SubGraph {Code =dn.SubgraphCode, Label = n.BaseCode};
                    graph.SubGraphs.Add(sg);
                }
            }
        }

        private void PrepareIgnoreNode() {
            if (hasignore) {
                var n = new Node {
                    Shape = NodeShapeType.Circle,
                    Style = NodeStyleType.Filled,
                    FillColor = ColorAttribute.Single(Color.RGB(0xFF, 0, 0)),
                    Label = ""
                };
                graph.AddNode(n);
            }
        }

        private ColorAttribute GetColor(DependencyEdge e)
        {
            if (e.Type == DependencyEdgeType.Formula) {
                return "5";
            }
            if (e.Type == DependencyEdgeType.Sum) {
                return "6";
            }
            if (e.Type == DependencyEdgeType.Ref) {
                return "10";
            }
            if (e.Type == DependencyEdgeType.ExRef) {
                return "11";
            }
            return ColorAttribute.Single( Color.RGB(0,0,0));
        }

        private NodeShapeType GetShape(DependencyNode n)
        {
            if (n.Type == DependencyNodeType.Sum) return NodeShapeType.Box3d;
            if (n.Type == DependencyNodeType.Formula) return NodeShapeType.Cds;
            if (n.Type == DependencyNodeType.Ref) return NodeShapeType.Egg;
            return NodeShapeType.Box;
        }


    }
}