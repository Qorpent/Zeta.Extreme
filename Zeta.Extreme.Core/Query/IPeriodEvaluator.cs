#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPeriodEvaluator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ���������� ���������� ��������
	/// </summary>
	public interface IPeriodEvaluator {
		/// <summary>
		/// 	���������� ��������� TimeHandler �� ����������������� ����� � ���������
		/// </summary>
		/// <param name="basePeriod"> </param>
		/// <param name="period"> </param>
		/// <param name="year"> </param>
		/// <returns> </returns>
		TimeHandler Evaluate(int basePeriod, int period, int year);
	}
}