using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ��� ������ � �������� ��� ������� ������� ���� "������" ��� ���������� ������,
	/// ��� ������ � �������� �������� - �� ����� ����������� ������
	/// </summary>
	[Flags]
	public enum DetailMode {
		/// <summary>
		/// �������������� - ������� ���� �������� ������
		/// </summary>
		None, 
		/// <summary>
		/// ������������ ������ �������, ���������� - ������ ��������, ��� DETAIL IS NULL ��� �������� �� ������
		/// </summary>
		SafeObject,
		/// <summary>
		/// ������ - ��� �����, ������������ �������� SUM(VALUE), �� ����� �������������� ��� ��������� ������ ��� �����
		/// </summary>
		SumObject,
		/// <summary>
		/// �� �� ����� ��� � SumObject, ������ ������������ ������� ��������, ��� DETAIL IS NOT NULL
		/// </summary>
		SafeSumObject,
		/// <summary>
		/// �������� ��� ������������� ������������ ������ �������� DETAILTYPE (���������� �� ������ �������� �������)
		/// </summary>
		TypedSumObject,
	}
}