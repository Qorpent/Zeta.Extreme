using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������������� ������ ���������
	/// </summary>
	public class FormStructureRow {
		/// <summary>
		/// ������ ����������� ������
		/// </summary>
		public IZetaRow Native;

		/// <summary>
		/// �����
		/// </summary>
		public int Idx;

		/// <summary>
		/// �������
		/// </summary>
		public int Level;

		/// <summary>
		/// �������������� � �������� ��������� ������ (��� �������� � ����)
		/// </summary>
		public IZetaObject AttachedObject;

		/// <summary>
		/// ������� ������ ������������� ������������� ������������ �� �������
		/// </summary>
		public bool SumObj;
		/// <summary>
		/// �������������� ������ �� ID ������������
		/// </summary>
		public string AltObjFilter;
	}
}