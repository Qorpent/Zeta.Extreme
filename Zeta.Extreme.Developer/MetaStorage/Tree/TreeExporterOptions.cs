namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// ����� ���������� �����
	/// </summary>
	public class TreeExporterOptions {
		/// <summary>
		/// 
		/// </summary>
		public TreeExporterOptions() {
			CodeMode = TreeExporterCodeMode.Default;
		}
		/// <summary>
		/// ��� ��������� ������ �������� ������������ �� ��������� ����
		/// </summary>
		public bool DetachRoot { get; set; }
		/// <summary>
		/// ����� ��������� �����
		/// </summary>
		public TreeExporterCodeMode CodeMode { get; set; }
		/// <summary>
		/// ������������ ���� ��� ������ B#
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// ��� ������ ��� ������ B#
		/// </summary>
		public string ClassName { get; set; }
	}
}