#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ILockPeriodMapper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Интерфейс мапера блокировки периодов
	/// </summary>
	public interface ILockPeriodMapper {
		/// <summary>
		/// 	Получить список блокированных периодов с командами блокировки
		/// </summary>
		/// <param name="operation"> </param>
		/// <param name="givenperiod"> </param>
		/// <returns> </returns>
		int[] GetLockingPeriods(LockOperation operation, int givenperiod);
	}
}