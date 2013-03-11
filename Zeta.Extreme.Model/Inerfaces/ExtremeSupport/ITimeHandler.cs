#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ITimeHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	����������� ��������� ��������� �������
	/// </summary>
	public interface ITimeHandler : IWithCacheKey {
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