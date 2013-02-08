using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ������������ ����� ���
	/// </summary>
	[Flags]
	public enum ZoneType {
		/// <summary>
		/// �� ����������
		/// </summary>
		None = 0,
		/// <summary>
		/// ������� ������
		/// </summary>
		Obj = 1,
		/// <summary>
		/// ������� ������
		/// </summary>
		Detail = 2,
		/// <summary>
		/// ������� Detail
		/// </summary>
		Det = Detail,
		/// <summary>
		/// ������� Detail
		/// </summary>
		Sp = Detail,
		/// <summary>
		/// ������ ��������
		/// </summary>
		Grp = 4,
		/// <summary>
		/// ������ Grp
		/// </summary>
		Og = Grp,
		/// <summary>
		/// ��������
		/// </summary>
		Div = 8,
		/// <summary>
		/// ������� DIV
		/// </summary>
		H = Div,
		/// <summary>
		/// ����������� ���
		/// </summary>
		Unknown = 128
	}
}