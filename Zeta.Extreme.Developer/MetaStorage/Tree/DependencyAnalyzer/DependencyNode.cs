using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Узел графа зависимостей
	/// </summary>
	public class DependencyNode {
		/// <summary>
		/// 
		/// </summary>
		public  DependencyNode()
		{
			
		}
		/// <summary>
		/// Создает нод из строки
		/// </summary>
		/// <param name="row"></param>
		public DependencyNode(IZetaRow row) {
			Row = row;
			Code = GetDotCode(row);
			Tooltip = row.Name;
			BaseCode = row.Code.Substring(0, 4);
			Type = GetNodeType(row);
			Label = row.Code;
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tooltip { get; set; }

		/// <summary>
		/// Признак специально запрошенного узла
		/// </summary>
		public bool IsTarget { get; set; }

		/// <summary>
		/// Признак терминального узла
		/// </summary>
		public bool IsTerminal { get; set; }

		/// <summary>
		/// Признак того, что узел не был дораскрыт из-за ограничений уровня
		/// </summary>
		public bool IsNotFullyLeveled { get; set; }
		/// <summary>
		/// Код узла
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// Код по форме
		/// </summary>
		public string BaseCode { get; set; }
		/// <summary>
		/// Тип узла
		/// </summary>
		public DependencyNodeType Type { get; set; }
		/// <summary>
		/// Ссылка на строку
		/// </summary>
		public IZetaRow Row { get; set; }
		/// <summary>
		/// Заголовок
		/// </summary>
		public string Label { get; set; }
		/// <summary>
		/// Проверка идентичности
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			if (base.Equals(obj)) return true;
			if (!(obj is DependencyNode)) return false;
			return ((obj as DependencyNode).Code == Code);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() {
			return Code.GetHashCode();
		}

		/// <summary>
		/// Получить тип узла
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static DependencyNodeType GetNodeType(IZetaRow row)
		{
			if (row.IsMarkSeted("CONTROLPOINT")) return DependencyNodeType.ControlPoint;
			if (row.IsFormula) return DependencyNodeType.Formula;
			if (null != row.RefTo) return DependencyNodeType.Ref;
			if (null != row.ExRefTo) return DependencyNodeType.ExRef;
			if (row.IsMarkSeted("0SA")) return DependencyNodeType.Sum;
			return DependencyNodeType.Primary;
		}

		/// <summary>
		/// Получить тип узла
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static string GetDotCode(IZetaRow row) {
		    return row.Code;
		}
		
		
	}
}