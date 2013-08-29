namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// ����� ��������� �����
	/// </summary>
	public enum TreeExporterCodeMode {
		/// <summary>
		/// ��������������
		/// </summary>
		Undefined = 1,
		/// <summary>
		/// ������������ ������ ����� ���
		/// </summary>
		Full = 1<<1,

		/// <summary>
		/// ���� ������ �� ����������� - ������ ���� ������������ �������� ��� �������� (��� � �����������)
		/// </summary>
		NoCode = 1<<2,
		/// <summary>
		/// ���� ��� ��������� ���� �������� ��� ����� � �������� ��������, �� ���������
		/// </summary>
		RootPrefix = 1<<3,
		/// <summary>
		/// ���� ��� ��������� ���� �������� ��� ������������� � �������� ��������, �� ���������
		/// </summary>
		ParentPrefix = 1<<4,
		/// <summary>
		/// �� ��������� ���� �� ��������������
		/// </summary>
		Default = Full,

	}
}