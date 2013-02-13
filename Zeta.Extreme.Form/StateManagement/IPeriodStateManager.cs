#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IPeriodStateManager.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	��������� ��������� ������� ��������
	/// </summary>
	public interface IPeriodStateManager {
		/// <summary>
		/// 	�������
		/// </summary>
		string System { get; set; }

		/// <summary>
		/// 	��
		/// </summary>
		string Database { get; set; }

		/// <summary>
		/// 	�������� ��� ������
		/// </summary>
		/// <param name="year"> </param>
		/// <returns> </returns>
		PeriodStateRecord[] All(int year);

		/// <summary>
		/// 	�������� ������ �� ���� � �������
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		PeriodStateRecord Get(int year, int period);

		/// <summary>
		/// 	�������� ������
		/// </summary>
		/// <param name="record"> </param>
		void UpdateState(PeriodStateRecord record);

		/// <summary>
		/// 	�������� �������
		/// </summary>
		/// <param name="record"> </param>
		void UpdateDeadline(PeriodStateRecord record);

		/// <summary>
		/// 	�������� ������� �� ����������
		/// </summary>
		/// <param name="record"> </param>
		void UpdateUDeadline(PeriodStateRecord record);
	}
}