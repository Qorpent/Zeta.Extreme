using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// ���� ����� ������������
	/// </summary>
	public class DependencyNode {
		/// <summary>
		/// 
		/// </summary>
		public  DependencyNode()
		{
			
		}
		/// <summary>
		/// ������� ��� �� ������
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
		/// ������� ���������� ������������ ����
		/// </summary>
		public bool IsTarget { get; set; }

		/// <summary>
		/// ������� ������������� ����
		/// </summary>
		public bool IsTerminal { get; set; }

		/// <summary>
		/// ������� ����, ��� ���� �� ��� ��������� ��-�� ����������� ������
		/// </summary>
		public bool IsNotFullyLeveled { get; set; }
		/// <summary>
		/// ��� ����
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// ��� �� �����
		/// </summary>
		public string BaseCode { get; set; }
		/// <summary>
		/// ��� ����
		/// </summary>
		public DependencyNodeType Type { get; set; }
		/// <summary>
		/// ������ �� ������
		/// </summary>
		public IZetaRow Row { get; set; }
		/// <summary>
		/// ���������
		/// </summary>
		public string Label { get; set; }
		/// <summary>
		/// �������� ������������
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
		/// �������� ��� ����
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
		/// �������� ��� ����
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static string GetDotCode(IZetaRow row) {
		    return row.Code;
		}
		
		
	}
}