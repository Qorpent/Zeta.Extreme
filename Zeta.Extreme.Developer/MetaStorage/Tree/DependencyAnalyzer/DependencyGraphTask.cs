using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Задача на отрисовку графика по завсимостям
	/// </summary>
	public class DependencyGraphTask {
		/// <summary>
		/// Стартовая строка
		/// </summary>
		public IZetaRow StartRow { get; set; }
		/// <summary>
		/// Типы включаемых узлов
		/// </summary>
		public DependencyNodeType NodeTypes { get; set; }
		/// <summary>
		/// Типы включаемых переходов
		/// </summary>
		public DependencyEdgeType EdgeTypes { get; set; }
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
		public OldDependencyGraph ResultGraph { get; set; }
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
		
		

	}
}