namespace Zeta.Extreme {
	/// <summary>
	/// ������� ��������� �������
	/// </summary>
	public interface IFormula {

		/// <summary>
		/// ����������� ������� �� ���������� ���������� ������
		/// </summary>
		/// <param name="query"></param>
		void Init(ZexQuery query);
		/// <summary>
		/// ������� ���������� ����������
		/// </summary>
		/// <returns></returns>
		/// <remarks>� �������� ����� ���������� ���������� ������� �� ������ ������ �����</remarks>
		QueryResult Eval();
		/// <summary>
		/// ��������� ������� �������� ������� ����� �������������
		/// </summary>
		void CleanUp();
	}
}