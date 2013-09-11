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
        private const string IGNORENODE = "IGNORE";

        /// <summary>
        /// Формирует из графа завимисомтией DOT-graph
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Graph Generate(DependencyGraph s) {
            srcgraph = s;
            uri = s.BaseUri;
            hasignore = srcgraph.Edges.Values.Any(_ => _.To == IGNORENODE || _.From==IGNORENODE);
            graph = new Graph {
                Code = s.Code,
                RankDir = RankDirType.LR,
                FontName = "Tahoma",
                DefaultNode = new Node {
                    FontSize = 9,
                    ColorScheme = "set312",
                    Style = NodeStyleType.Filled,
                    FillColor = "2",
                },
                DefaultEdge = new Node
                {
                    FontSize = 8,
                }
            };
            if (s.ShowLegend) {
                BuildLegend();
            }
            MoveNodes();
            MoveEdges();
            graph.Compactize();

            graph.AutoTune();
            return graph;
        }
        private void BuildLegend() {
	        var sg = new SubGraph {Code = "cluster_legend"};
            graph.SubGraphs.Add(sg);
            graph.AddNode(new Node {
	            Code = "l_prim", 
	            SubgraphCode = sg.Code,
	            Shape = NodeShapeType.Box,
	            Label = "Первичная"
	        });
            graph.AddNode(new Node
            {
                Code = "l_form",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Cds,
                Label = "Формула"
            });
            graph.AddNode(new Node
            {
                Code = "l_sum",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Folder,
                Label = "Сумма"
            });
            graph.AddNode(new Node
            {
                Code = "l_ref",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Larrow,
                Label = "Ссылка"
            });
            graph.AddNode(new Node
            {
                Code = "l_exref",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.lpromoter,
                Label = "Добавочная ссылка"
            });
            graph.AddNode(new Node
            {
                Code = "l_target",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Folder,
                Label = "Запрошенная\r\nстрока",
                FillColor = "7",
                Tooltip = "Узел, от которого начинается развертка"
            });
            graph.AddNode(new Node
            {
                Code = "l_nonlevel",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Folder,
                Label = "Ограничение\r\nуровеня",
                FillColor = "12",
                Tooltip = "Узел, который содержит дальнейшую развертку, но при этом ограничен по уровню"
            });
            graph.AddNode(new Node
            {
                Code = "l_terminal",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Folder,
                FillColor = "9",
                Label = "Терминал",
                Tooltip = "Узел, указан как терминальный - сам выводится, но дальнейше развертки нет"
            });
            graph.AddNode(new Node
            {
                Code = "l_ignore",
                SubgraphCode = sg.Code,
                Shape = NodeShapeType.Terminator,
                FillColor = Color.RGB(0xFF,0,0),
                Label = "Игнор",
                Tooltip = "Собирает все игнорируемые узлы"
            });
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
                    Code=IGNORENODE,
                    Shape = NodeShapeType.Terminator,
                    Style = NodeStyleType.Filled,
                    FillColor = ColorAttribute.Single(Color.RGB(0xFF, 0, 0)),
                    Label = "",
                    Tooltip = "Игнорируемые узлы "
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
            if (n.Type == DependencyNodeType.Sum) return NodeShapeType.Folder;
            if (n.Type == DependencyNodeType.Formula) return NodeShapeType.Cds;
            if (n.Type == DependencyNodeType.Ref) return NodeShapeType.Larrow;
            if (n.Type == DependencyNodeType.ExRef) return NodeShapeType.lpromoter;
            return NodeShapeType.Box;
        }


    }
}