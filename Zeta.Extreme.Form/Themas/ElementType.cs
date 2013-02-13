using System;

namespace Comdiv.Zeta.Web.Themas {
	/// <summary>
	/// ��� �������� ����
	/// </summary>
	[Flags]
	public enum ElementType {
		/// <summary>
		/// ��������������
		/// </summary>
		None= 0,
		/// <summary>
		/// �����
		/// </summary>
		Form = 1,
		/// <summary>
		/// ������
		/// </summary>
		Report = 2,
		/// <summary>
		/// ���������
		/// </summary>
		Document = 4,
		/// <summary>
		/// �������
		/// </summary>
		Command = 8,
		/// <summary>
		/// ���������������� ������������� ��������
		/// </summary>
		Custom = 16,
		/// <summary>
		/// ������ ��� �������� ���� ���������
		/// </summary>
		All = Form | Report|Document|Command|Custom,
	}
}