using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Qorpent.Graphs;
using Qorpent.Graphs.Dot;
using Qorpent.Graphs.Dot.Types;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd.Helpers
{
    /// <summary>
    /// Строит граф вычислений 
    /// </summary>
    public class QueryGraphBuilder : IGraphSource
    {
        private IQuery _query;
       

        /// <summary>
        /// Формирует анализатор запроса
        /// </summary>
        /// <param name="query"></param>
        public QueryGraphBuilder(IQuery query) {
            _query = query;
        }

        /// <summary>
        /// Флаг исключения нулевых первичных запросов
        /// </summary>
        public bool ExcludePrimaryZeroes { get; set; }
        /// <summary>
        /// Флаг исключения нулевых не первичных запросов
        /// </summary>
        public bool ExcludeNonPrimaryZeroes { get; set; }
        /// <summary>
        /// Флаг исключения нулевых не первичных запросов
        /// </summary>
        public bool PreserveCompact { get; set; }

        /// <summary>
        /// Вызывает процедуру построения графа
        /// </summary>
        /// <returns></returns>
        public IGraphConvertible BuildGraph(GraphOptions options) {
            var realquery = _query;
            if (null == realquery.Result) {
                var session = new Session();
                realquery = session.Register(_query);
                session.WaitEvaluation();
            }
            var graph = new Graph();
            graph.DefaultNode = new Node {FontSize = 10};
            graph.RankDir = RankDirType.RL;
            graph.FontSize = 8;
            var visited = new List<IQuery>();
            graph.AddElements(GetQueryElementsTree(realquery,null,visited));
            return graph;
        }

        private string GetCode(IQuery q) {
            return "q" + q.Uid;
        }

        private IEnumerable<GraphElementBase> GetQueryElementsTree(IQuery query, IQuery master, List<IQuery> visited) {
            if(visited.Contains(query))yield break;
            visited.Add(query);
            var q = (Query) query;
            if(CheckExclude(q))yield break;

            var result = new Node {
                Shape = NodeShapeType.Mrecord, 
                Label = BuildQueryTable(query,master).ToString(SaveOptions.DisableFormatting)
                .Replace("{","\\{")
                .Replace("}", "\\}")
                ,
                Code=GetCode(query),
            };
            yield return result;
            if (!q.IsPrimary) {
                foreach (var e in GenerateEdges(q,master)) yield return e;
                foreach (var e in GenerateChildNodes(visited, q,master)) yield return e;
            }
        }

        private IEnumerable<GraphElementBase> GenerateChildNodes(List<IQuery> visited, Query q, IQuery master) {
            foreach (Query f in q.FormulaDependency) {
                if (!CheckExclude(f)) {
                    foreach (var e in GetQueryElementsTree(f,q, visited)) {
                        yield return e;
                    }
                }
            }
            foreach (var f in q.SummaDependency) {
                if (!CheckExclude((Query) f.Item2)) {
                    foreach (var e in GetQueryElementsTree(f.Item2,q, visited)) {
                        yield return e;
                    }
                }
            }
        }

        private IEnumerable<GraphElementBase> GenerateEdges(Query q, IQuery master) {
            foreach (Query f in q.FormulaDependency) {
                if (!CheckExclude(f)) {
                    yield return BuildFormulaEdge(q, master, f);
                }
            }
            foreach (var f in q.SummaDependency) {
                if (!CheckExclude((Query) f.Item2)) {
                    yield return BuildSumEdge(q,master, f);
                }
            }
        }

        private GraphElementBase BuildSumEdge(Query query, IQuery master, Tuple<decimal, IQuery> tuple) {
            var edge=  new Edge { From = GetCode(tuple.Item2), To = GetCode(query), Type = "sum" };
            if (tuple.Item1 != 1) {
                edge.Label = tuple.Item1.ToString("0.####", CultureInfo.InvariantCulture);
            }
            return edge;
        }

        private Edge BuildFormulaEdge(Query query, IQuery master, IQuery q) {
            return new Edge {From = GetCode(q), To = GetCode(query), Type = "formula"};
        }

        private bool CheckExclude(Query q) {
            bool exclude =false;
            if (null == q.Result.Error && 0 == q.Result.NumericResult) {
                if (q.IsPrimary) {
                    if (ExcludePrimaryZeroes) {
                        exclude = true;
                    }
                }
                else {
                    if (ExcludeNonPrimaryZeroes) {
                        exclude = true;
                    }
                }
            }
            return exclude;
        }

        private XElement BuildQueryTable( IQuery query,IQuery master) {
            var result = new XElement("TABLE");
            result.SetAttributeValue("BORDER",0);
            result.SetAttributeValue("CELLBORDER",1);
            result.SetAttributeValue("CELLSPACING",0);
            result.SetAttributeValue("CELLPADDING",2);
            BuildMainStateRow(result, query);
            if (PreserveCompact || (null == master || master.Row.GetCacheKey() != query.Row.GetCacheKey())) {
                BuildRowInfoRow(result, query);
            }
            if (PreserveCompact || (null == master || master.Obj.GetCacheKey() != query.Obj.GetCacheKey())) {
                BuildObjInfoRow(result, query);
            }
            if (PreserveCompact || (null == master || master.Col.GetCacheKey() != query.Col.GetCacheKey())) {
                BuildColInfoRow(result, query);
            }
            if (PreserveCompact || (null == master || master.Time.GetCacheKey() != query.Time.GetCacheKey())) {
                BuildTimeInfoRow(result, query);
            }
            if (PreserveCompact || (null == master || (master.Reference.GetCacheKey() != query.Reference.GetCacheKey()))) {
                BuildExtInfoRow(result, query);
            }
            return result;
        }

        private void BuildObjInfoRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            BuildObjTypeCell(query, row);
            BuildObjCodeCell(query, row);
            row.Add(new XElement("TD", " "));
            result.Add(row);
        }

        private void BuildObjCodeCell(IQuery query, XElement row) {
            var code = query.Obj.Id.ToString();
            if (0 == query.Obj.Id)
            {
                code = "CUSTOM FORMULA";
            }
            var codecell = new XElement("TD", code);
            codecell.SetAttributeValue("HREF", "#");
            var tooltip = query.Obj.Name;
            if (query.Obj.IsFormula)
            {
                tooltip += " (" + query.Obj.Formula + ")";
            }
            codecell.SetAttributeValue("TITLE", tooltip);
            row.Add(codecell);
        }

        private void BuildObjTypeCell(IQuery query, XElement row) {
            var statecell = new XElement("TD", " ");
            statecell.SetAttributeValue("HREF", "#");
            statecell.SetAttributeValue("TITLE", "Первичная");
            var color = Color.Green;
            if (query.Obj.IsFormula)
            {
                color = Color.Blue;
                statecell.SetAttributeValue("TITLE", "Формула");
            }
            statecell.SetAttributeValue("BGCOLOR", color.ToString());
            row.Add(statecell);
        }

        private void BuildExtInfoRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            row.Add(new XElement("TD", " "));
            var val = new XElement("TD", " ");

            val.Value = query.Currency;
            if (null != query.Reference) {
                val.Value += "(" + query.Reference.GetCacheKey() + ")";
            }
            row.Add(val);
            val.SetAttributeValue("HREF", "#");
            val.SetAttributeValue("TITLE","Валюта и дополнительные опции запроса");
            row.Add(new XElement("TD", " "));
            result.Add(row);
        }

        private void BuildTimeInfoRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            row.Add(new XElement("TD", " "));
            var val = new XElement("TD", " ");
            var p = Periods.Get(query.Time.Period);
            val.Value = query.Time.Year + ", " + p.Name + " (" + query.Time.Period + ")";
            row.Add(val);
            row.Add(new XElement("TD", " "));
            val.SetAttributeValue("HREF", "#");
            val.SetAttributeValue("TITLE", "Год и период");
            result.Add(row);
        }

        private void BuildColInfoRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            BuildColTypeCell(query, row);
            BuildColCodeCell(query, row);
            row.Add(new XElement("TD", " "));
            result.Add(row);
        }

        private void BuildColCodeCell(IQuery query , XElement row) {
            var code = query.Col.Code;
            if (0 == query.Col.Id) {
                code = "CUSTOM FORMULA";
            }
            var codecell = new XElement("TD", code);
            codecell.SetAttributeValue("HREF", "#");
            var tooltip = query.Col.Name;
            if (query.Col.IsFormula)
            {
                tooltip += " (" + query.Col.Formula + ")";
            }
            codecell.SetAttributeValue("TITLE", tooltip);
            row.Add(codecell);
        }

        private void BuildColTypeCell(IQuery query, XElement row) {
            var statecell = new XElement("TD", " ");
            statecell.SetAttributeValue("HREF", "#");
            statecell.SetAttributeValue("TITLE", "Первичная");
            var color = Color.Green;


            if (query.Col.IsFormula)
            {
                color = Color.Blue;
                statecell.SetAttributeValue("TITLE", "Формула");
            }
            statecell.SetAttributeValue("BGCOLOR", color.ToString());
            row.Add(statecell);
        }

        private void BuildRowInfoRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            BuildRowTypeCell(query, row);          
            BuildRowCodeCell(query, row);
            row.Add(new XElement("TD", " "));
            result.Add(row);
        }

        private void BuildRowCodeCell(IQuery query, XElement row) {
            var code = query.Row.Code;
            if (0 == query.Row.Id)
            {
                code = "CUSTOM FORMULA";
            }
            var codecell = new XElement("TD", code);
            codecell.SetAttributeValue("HREF", "#");
            var tooltip = query.Row.Name;
            if (query.Row.IsFormula) {
                tooltip += " (" + query.Row.Formula + ")";
            }
            codecell.SetAttributeValue("TITLE",tooltip);
            //codecell.SetAttributeValue("COLSPAN",2);
            row.Add(codecell);
        }

        private void BuildRowTypeCell(IQuery query, XElement row) {
            var statecell = new XElement("TD", " ");
            statecell.SetAttributeValue("HREF", "#");
            statecell.SetAttributeValue("TITLE", "Первичная");
            var color = Color.Green;

            if (query.Row.IsSum)
            {
                color = Color.Yellow;
                statecell.SetAttributeValue("TITLE", "Сумма");
            }
            else if (query.Row.IsFormula)
            {
                color = Color.Blue;
                statecell.SetAttributeValue("TITLE", "Формула");
            }
            statecell.SetAttributeValue("BGCOLOR", color.ToString());
            row.Add(statecell);
        }

        private void BuildMainStateRow(XElement result, IQuery query) {
            var row = new XElement("TR");
            BuildMainQueryTypeCell(query, row);
           
            BuildValueCell(query, row);
            BuildStateCell(query, row);
            result.Add(row);
        }

        private static void BuildStateCell(IQuery query, XElement row) {
            var statecell = new XElement("TD"," ");
            var color = Color.Green;
            statecell.SetAttributeValue("HREF","#");
            statecell.SetAttributeValue("TITLE","OK");
            if (null != query.Result.Error) {
                color = Color.Red;
                statecell.SetAttributeValue("TITLE", "ERROR: "+query.Result.Error.ToString());
            }
            statecell.SetAttributeValue("BGCOLOR", color.ToString());
            
            row.Add(statecell);
        }

        private static void BuildValueCell(IQuery query, XElement row) {
            var valcell = new XElement("TD");
            row.Add(valcell);
            var text = new XElement("B");
            text.Value = query.Result.NumericResult.ToString("#,0.####", CultureInfo.GetCultureInfo("Ru-ru"));
            valcell.Add(text);
        }

        private static void BuildMainQueryTypeCell(IQuery query, XElement row) {
            var maintype = new XElement("TD", " ");
            maintype.SetAttributeValue("HREF", "#");
            maintype.SetAttributeValue("TITLE", "Первичный");
            row.Add(maintype);
            var color = Color.Green;
            var q = (Query) query;
            if (q.EvaluationType == QueryEvaluationType.Formula) {
                color = Color.Blue;
                maintype.SetAttributeValue("TITLE", "Формула");
            }
            else if (q.EvaluationType == QueryEvaluationType.Summa) {
                color = Color.Yellow;
                maintype.SetAttributeValue("TITLE", "Сумма");
            }
            maintype.SetAttributeValue("BGCOLOR", color.ToString());
        }

        /// <summary>
        /// Выполнение пост-обработки SVG с графом
        /// </summary>
        /// <param name="currentSvg"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public XElement PostprocessGraphSvg(XElement currentSvg, GraphOptions options) {
            return currentSvg;
        }
    }
}
