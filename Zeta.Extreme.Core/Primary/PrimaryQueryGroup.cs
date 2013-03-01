namespace Zeta.Extreme.Primary {
	/// <summary>
	/// ������ ��������� ��������
	/// </summary>
	public class PrimaryQueryGroup {
		/// <summary>
		/// ������� � ������
		/// </summary>
		public Query[] Queries { get; set; }
		/// <summary>
		/// �������� ���������� �������
		/// </summary>
		public PrimaryQueryPrototype Prototype { get; set; }

		/// <summary>
		/// ��������� ��������
		/// </summary>
		public IScriptGenerator ScriptGenerator { get; set; }

		/// <summary>
		/// ������ SQL ������
		/// </summary>
		/// <returns></returns>
		public string GenerateSqlScript() {
			return ScriptGenerator.Generate(Queries, Prototype);
		}
	}
}