using System;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// ��� ����������
	/// </summary>
	[Flags]
	public enum ProbeResultType {
		/// <summary>
		/// ���������������
		/// </summary>
		Undefined = 1,
		/// <summary>
		/// ��������
		/// </summary>
		Success = 2,
		/// <summary>
		/// ��������������
		/// </summary>
		Ignored = 4,
		/// <summary>
		/// ������, ���������� � �������� ������
		/// </summary>
		Error = 8,
	}
}