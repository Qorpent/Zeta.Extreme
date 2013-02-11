using System;
using System.Collections.Concurrent;

namespace Zeta.Extreme {
	/// <summary>
	/// ������ �� ���������� �������
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// ���������� ����
		/// </summary>
		public string Key;
		/// <summary>
		/// ����� �������
		/// </summary>
		public string Formula;
		/// <summary>
		/// ��� �������
		/// </summary>
		public string FormulaType;
		/// <summary>
		/// ����
		/// </summary>
		public string Tags;
		/// <summary>
		/// �����
		/// </summary>
		public string Marks;
		/// <summary>
		/// ������������ ������� �����
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// ������ ����������� ���� � ��������� ���������
		/// </summary>
		public Type PreparedType;

		/// <summary>
		/// ��� ������ - ����� �������������� ����������� ��� ����������� ����
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();
	}
}