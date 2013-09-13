using System;
using System.Linq;
using System.Xml.Linq;
using Qorpent.Graphs;
using Qorpent.Graphs.Dot;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Задача на отрисовку графика по завсимостям
	/// </summary>
	public class DependencyGraphTask:IGraphSource {
		/// <summary>
		/// 
		/// </summary>
		public DependencyGraphTask() {
			NodeTypes = DependencyNodeType.Default;
			EdgeTypes = DependencyEdgeType.Default;
			Direction = DependencyDirection.Default;

		}
		/// <summary>
		/// Код задачи/графа
		/// </summary>
		public string Code { get; set; }
        /// <summary>
        /// Базовый URL
        /// </summary>
        public Uri BaseUri { get; set; }
		/// <summary>
		/// Стартовая строка
		/// </summary>
		public IZetaRow StartRow { get; set; }
		/// <summary>
		/// Направление развертки графа
		/// </summary>
		public DependencyDirection Direction { get; set; }
		/// <summary>
		/// Типы включаемых узлов
		/// </summary>
		public DependencyNodeType NodeTypes { get; set; }
		/// <summary>
		/// Типы включаемых переходов
		/// </summary>
		public DependencyEdgeType EdgeTypes { get; set; }
		/// <summary>
		/// Разбивать зависимости по кластерам
		/// </summary>
		public bool Clusterize { get; set; }
		/// <summary>
		/// Маски терминальных узлов
		/// </summary>
		public string[] TerminalMasks { get; set; }
		/// <summary>
		/// Маски игнорируемых узлов
		/// </summary>
		public string[] IgnoreMasks { get; set; }
		/// <summary>
		/// Предельная глубина разложения
		/// </summary>
		public int Depth { get; set; }
		/// <summary>
		/// Результирующий граф
		/// </summary>
		public DependencyGraph ResultGraph { get; set; }
		/// <summary>
		/// Возвращает вариант включения в граф
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public IncludeType GetIncludeType(IZetaRow row) {
			var nodeType = DependencyNode.GetNodeType(row);
			if (!NodeTypes.HasFlag(nodeType)) return IncludeType.None;
			if (null != IgnoreMasks && 0 != IgnoreMasks.Length) {
				if (IgnoreMasks.Any(_ => row.Code.StartsWith(_))) {
					return IncludeType.None;
				}
			}
			if (null != TerminalMasks && 0 != TerminalMasks.Length)
			{
				if (TerminalMasks.Any(_ => row.Code.StartsWith(_)))
				{
					return IncludeType.Self;
				}
			}

			return IncludeType.SelfAndDescendants;
		}

		/// <summary>
		/// Отрисовывает задачу в виде DOT
		/// </summary>
		/// <returns></returns>
		public Graph Render() {
			if (null == ResultGraph) {
				Build();
			}
			return new DependencyGraphGenerator().Generate(ResultGraph);
		}

		/// <summary>
		/// Построить граф
		/// </summary>
		public void Build() {
			if (null == ResultGraph)
			{
				ResultGraph = new DependencyGraph {Clusterize = Clusterize, Code = Code, BaseUri = BaseUri};
			    if (string.IsNullOrWhiteSpace(ResultGraph.Code)) {
					ResultGraph.Code = DependencyNode.GetDotCode(StartRow) + "_" + Direction;
				}
				ResultGraph.ShowLegend = ShowLegend;
			}
       
			if (Direction.HasFlag(DependencyDirection.Down))
			{
				new GoDownDependencyVisitor().Process(this);
			}
			if (Direction.HasFlag(DependencyDirection.Up))
			{
				new GoUpDependencyVisitor().Process(this);
			}
			
		}

	    

	    /// <summary>
		/// Показать легенду
		/// </summary>
		public bool ShowLegend { get; set; }

	    /// <summary>
	    /// Вызывает процедуру построения графа
	    /// </summary>
	    /// <returns></returns>
	    public IGraphConvertible BuildGraph(GraphOptions options) {
	        if (null == ResultGraph) {
	            Build();
	        }      
	        return new DependencyGraphGenerator().Generate(ResultGraph);
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