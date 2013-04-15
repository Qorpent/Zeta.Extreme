using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������� ��������� "����������"
	/// </summary>
	public interface IReferenceHandler:IQueryDimension {
		/// <summary>
		/// ������ �� ������������
		/// </summary>
		string Contragents { get; set; }
		/// <summary>
		/// ������ �� ������
		/// </summary>
		string Accounts { get; set; }

		/// <summary>
		/// ����������� ��� ����������� �������
		/// </summary>
		/// <returns></returns>
		IReferenceHandler Copy();
	}
}