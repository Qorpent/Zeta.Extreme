namespace Zeta.Extreme {
	/// <summary>
	/// ��������� ��������� ���������� �������
	/// </summary>
	public struct PrimaryQueryPrototype {
		/// <summary>
		/// ������� ������������� ���������
		/// </summary>
		public bool UseSum { get; set; }
		/// <summary>
		/// ������ �� ������������� �������
		/// </summary>
		public bool PreserveDetails { get; set; }
		/// <summary>
		/// ��������c�� � ������������� �������
		/// </summary>
		public bool RequireDetails { get; set; }
		/// <summary>
		/// ������������� ������������ ������ ������� � ��������� ���������
		/// </summary>
		public bool RequreZetaEval { get; set; }
	}
}