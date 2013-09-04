using System;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Задача на отрисовку графика по завсимостям
	/// </summary>
	public class DependencyGraphTask {
		/// <summary>
		/// Код задачи/графа
		/// </summary>
		public string Code { get; set; }
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
		public string Render() {
			if (null == ResultGraph) {
				Build();
			}
			return new DependencyGraphDotRender().Render(ResultGraph);
		}

		/// <summary>
		/// Построить граф
		/// </summary>
		public void Build() {
			if (null == ResultGraph)
			{
				ResultGraph = new DependencyGraph();
				ResultGraph.Clusterize = this.Clusterize;
				ResultGraph.Code = Code;
				if (string.IsNullOrWhiteSpace(ResultGraph.Code)) {
					ResultGraph.Code = DependencyNode.GetDotCode(StartRow) + "_" + Direction;
				}
			}
			if (Direction.HasFlag(DependencyDirection.Down))
			{
				new FallDownDependencyVisitor().Process(this);
			}
			if (Direction.HasFlag(DependencyDirection.Up))
			{
				new GoUpDependencyVisitor().Process(this);
			}
			
		}
		
	}
}