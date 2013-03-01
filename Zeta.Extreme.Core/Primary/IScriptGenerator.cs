namespace Zeta.Extreme.Primary {
	/// <summary>
	/// ��������� ���������� ��������
	/// </summary>
	public interface IScriptGenerator {
		/// <summary>
		/// ������ SQL ������ � ������ ���������, � �� ���� "������" �������
		/// </summary>
		/// <param name="queries"></param>
		/// <param name="prototype"></param>
		/// <returns></returns>
		string Generate(Query[] queries, PrimaryQueryPrototype prototype);
	}
}