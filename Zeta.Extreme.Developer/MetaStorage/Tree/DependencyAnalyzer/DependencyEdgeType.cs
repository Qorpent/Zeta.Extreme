using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// ��� �����������
	/// </summary>
	[Flags]
	public enum DependencyEdgeType {
		/// <summary>
		/// ����� �����
		/// </summary>
		Sum = 1,
		/// <summary>
		/// ������
		/// </summary>
		Ref = 1<<1,
		/// <summary>
		/// ���������� ������
		/// </summary>
		ExRef = 1<<2,
		/// <summary>
		/// �������
		/// </summary>
		Formula = 1<<3,
	}
}