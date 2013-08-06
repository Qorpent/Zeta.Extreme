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
// PROJECT ORIGIN: IPeriodStateManager.cs

#endregion

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Интерфейс менеджера статуса периодов
	/// </summary>
	public interface IPeriodStateManager {
		/// <summary>
		///     Система
		/// </summary>
		string System { get; set; }

		/// <summary>
		///     БД
		/// </summary>
		string Database { get; set; }

		/// <summary>
		///     Получить запись по году и периоду
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="grp">Группа формы</param>
		/// <returns>
		/// </returns>
		PeriodStateRecord Get(int year, int period , string grp);

		/// <summary>
		///     Обновить дедлайн
		/// </summary>
		/// <param name="record"></param>
		void UpdateDeadline(PeriodStateRecord record);

		/// <summary>
		///     Обновить дедлайн по подписанию
		/// </summary>
		/// <param name="record"></param>
		void UpdateUDeadline(PeriodStateRecord record);
	}
}