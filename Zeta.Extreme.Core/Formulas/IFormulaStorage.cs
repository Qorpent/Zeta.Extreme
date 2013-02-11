using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ��������� ��������� ������
	/// </summary>
	public interface IFormulaStorage {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		///<returns>���������� ���������� ���� �������</returns>
		string Register(FormulaRequest request);

		/// <summary>
		/// ���������� ��������� ������� �� ����� 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="throwErrorOnNotFound">false ���� ���� ���������� NULL ��� ���������� ������� </param>
		/// <returns></returns>
		IFormula GetFormula(string key ,bool throwErrorOnNotFound = true);

		/// <summary>
		/// ���������� ������� ������� ���������, ����� ������������ ��� ���������� ���� ������
		/// </summary>
		/// <param name="key"></param>
		/// <param name="formula"></param>
		void Return(string key, IFormula formula);
	}
}