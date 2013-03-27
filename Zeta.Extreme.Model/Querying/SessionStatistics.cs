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
// PROJECT ORIGIN: Zeta.Extreme.Model/SessionStatistics.cs
#endregion
using System;
using Qorpent.Serialization;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Инкапсулирует статистические данные по сессии
	/// </summary>
	[Serialize]
	public class SessionStatistics {
		
		/// <summary>
		/// 	Статистика батчей
		/// </summary>
		[SerializeNotNullOnly]
		public int BatchCount;

		/// <summary>
		/// 	Статистика времени батчей
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan BatchTime;

		/// <summary>
		/// 	Статистика использованных значений
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryAffected;

		/// <summary>
		/// 	Статистика возвращеных ячеек
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryCatched;

		/// <summary>
		/// 	Счетчик формул
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeFormula;

		/// <summary>
		/// 	Счетчик первичных запросов
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypePrimary;

		/// <summary>
		/// 	Счетчик сумм
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeSum;

		/// <summary>
		/// 	Счетчик игнорируемых запросов
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryIgnored;

		/// <summary>
		/// 	Статистика действительно уникальных регистраций
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryNew;

		/// <summary>
		/// 	Статистика вызовов препроцессора
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryPreprocessed;

		/// <summary>
		/// 	Статистика резольвинга по внутреннему ключу
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByKey;

		/// <summary>
		/// 	Статистика количества дублированных запросов без препроцессинга
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByMapKey;

		/// <summary>
		/// 	Статистика резольвинга по наличию в кэше
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByUid;

		/// <summary>
		/// 	Статистика количества вызовов регистрации
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStarted;

		/// <summary>
		/// 	Статистика пользовтельских регистраций
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStartedUser;

		/// <summary>
		/// 	Счетчик результативных клиентских запросов
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryUser;

		/// <summary>
		/// 	Счетчик переводов строки
		/// </summary>
		[SerializeNotNullOnly]
		public int RowRedirections;

	

		/// <summary>
		/// 	Статистика общего времени выполнения
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan TimeTotal;
	}
}