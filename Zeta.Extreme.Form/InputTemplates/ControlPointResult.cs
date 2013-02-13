using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ��������� ������ ����������� �����
	/// </summary>
	public class ControlPointResult {
		/// <summary>
		/// ���������� ������
		/// </summary>
		public IZetaRow Row { get; set; }
		/// <summary>
		/// ����������� �������
		/// </summary>
		public ColumnDesc Col { get; set; }
		/// <summary>
		/// �������� �������� 
		/// </summary>
		public decimal Value { get; set; }
		/// <summary>
		/// �������� ���������� ����������� �����
		/// </summary>
		public bool IsValid {
			get { return Value == 0; }
		}
	}
}