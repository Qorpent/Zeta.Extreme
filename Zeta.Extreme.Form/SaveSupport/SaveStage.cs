using System;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������ ����������
	/// </summary>
	[Flags]
	public enum SaveStage {
		/// <summary>
		/// ����������� ���������
		/// </summary>
		None,
		/// <summary>
		/// �������� ������ ����������
		/// </summary>
		Load,
		/// <summary>
		/// �������� ����������� ���������� �� �������� ������������
		/// </summary>
		Authorize,
		/// <summary>
		/// ���������� ������� ������ - ����������� ������������
		/// </summary>
		Prepare,
		/// <summary>
		/// �������� ����������� ������������ ����������
		/// </summary>
		Validate,
		/// <summary>
		/// �������� ����������� ����������, �������� �������� � ����
		/// </summary>
		Test,
		/// <summary>
		/// ���������� ���������� �����
		/// </summary>
		Save,
		/// <summary>
		/// ���������� ����������� �������� ����� ���������� ����������, ������-�������
		/// </summary>
		AfterSave,
		/// <summary>
		/// �������� ����������
		/// </summary>
		Finished,
	}
}