using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// ��������� ����� �������, ������������������ ������� ������� � ������
	/// ��� ������� ������ ����� ��������� ���� �� ������
	/// </summary>
	public interface ISerialSession {
		/// <summary>
		/// ����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		QueryResult Eval(ZexQuery query);

		/// <summary>
		/// ����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		Task<QueryResult> EvalAsync(ZexQuery query);
	}
}