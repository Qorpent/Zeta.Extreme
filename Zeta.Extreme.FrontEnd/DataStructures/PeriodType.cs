using System;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��� �������
	/// </summary>
	[Flags]
	public enum PeriodType {
		/// <summary>
		/// ��������������
		/// </summary>
		None,
		/// <summary>
		/// �����
		/// </summary>
		Month,
		/// <summary>
		/// �������� ������� � ������ ����
		/// </summary>
		FromYearStartMain,
		/// <summary>
		/// �������������� ������� � ������ ����
		/// </summary>
		FromYearStartExt,
		/// <summary>
		/// �������� �������
		/// </summary>
		Plan,
		/// <summary>
		/// �������� ������� (��������)
		/// </summary>
		MonthPlan,
		/// <summary>
		/// ����������
		/// </summary>
		Corrective,
		/// <summary>
		/// ��������� �������
		/// </summary>
		Awaited,
		/// <summary>
		/// ������ � �������� ����
		/// </summary>
		InYear,
		/// <summary>
		/// �������������� �������
		/// </summary>
		Ext,
		
	}
}