using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ��� ���������� �������
	/// </summary>
	[Flags]
	public enum QueryEvaluationType {
		/// <summary>
		/// �����������
		/// </summary>
		Unknown,
		/// <summary>
		/// ���������
		/// </summary>
		Primary,
		/// <summary>
		/// ��������
		/// </summary>
		Summa,
		/// <summary>
		/// �������
		/// </summary>
		Formula,
	}
}