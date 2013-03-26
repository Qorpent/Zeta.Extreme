namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������� ��������� "����������"
	/// </summary>
	public interface IReferenceHandler:IWithCacheKey {
		/// <summary>
		/// ������ �� ������������
		/// </summary>
		string Contragents { get; set; }
		/// <summary>
		/// ������ �� ������
		/// </summary>
		string Accounts { get; set; }
		/// <summary>
		/// ������������ ��������� ��� �������
		/// </summary>
		/// <param name="session"></param>
		void Normalize(ISession session);
		/// <summary>
		/// ����������� ��� ����������� �������
		/// </summary>
		/// <returns></returns>
		IReferenceHandler Copy();
	}
}