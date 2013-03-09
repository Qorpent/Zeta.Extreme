namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ����������� ��������� ��������� �������
	/// </summary>
	public interface ITimeHandler : IWithCacheKey
	{
		/// <summary>
		/// 	���
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		int Period { get; set; }

		/// <summary>
		/// 	����� �� ���������� �����
		/// </summary>
		int[] Years { get; set; }

		/// <summary>
		/// 	����� ��������
		/// </summary>
		int[] Periods { get; set; }

		/// <summary>
		/// 	������� ��� (��� �������� ��������)
		/// </summary>
		int BaseYear { get; set; }

		/// <summary>
		/// 	������� ������ (��� �������� ��������)
		/// </summary>
		int BasePeriod { get; set; }

		/// <summary>
		/// 	True ���� ������ �������� � ���������
		/// </summary>
		/// <returns> </returns>
		bool IsPeriodDefined();

		/// <summary>
		/// 	True ���� ��� ��� �������� � ���������
		/// </summary>
		/// <returns> </returns>
		bool IsYearDefinied();

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		ITimeHandler Copy();

		/// <summary>
		/// 	����������� ���������� ���� � �������
		/// </summary>
		/// <param name="session"> </param>
		void Normalize(ISession session = null);
	}
}