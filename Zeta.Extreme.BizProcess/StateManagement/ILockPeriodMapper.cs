#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/ILockPeriodMapper.cs
#endregion
namespace Zeta.Extreme.BizProcess.StateManagement {
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