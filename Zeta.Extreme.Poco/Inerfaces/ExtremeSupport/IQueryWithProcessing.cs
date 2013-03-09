using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ��������� ������� � ���������� ���������
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		/// <summary>
		/// 	������������ ������� ���������� �������
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);
	}
}