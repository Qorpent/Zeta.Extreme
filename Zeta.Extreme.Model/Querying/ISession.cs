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
// PROJECT ORIGIN: Zeta.Extreme.Model/ISession.cs
#endregion
using System;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Асинхронная, Zeta.Extrem cecсия, базовый интерфейс
	/// </summary>
	public interface ISession {
		

		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Get(string key, int timeout = -1);

		/// <summary>
		/// 	Синхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		IQuery Register(IQuery query, string uid = null);

		/// <summary>
		/// 	Асинхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> задачу, по результатам которой возвращается запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
		/// </remarks>
		Task<IQuery> RegisterAsync(IQuery query, string uid = null);

		/// <summary>
		/// 	Выполняет синхронизацию и расчет значений в сессии
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);

		/// <summary>
		/// Источник дополнительных параметров
		/// </summary>
		ISessionPropertySource PropertySource { get; set; }

		/// <summary>
		/// Флаг использования синхронизированной подготовки запросов
		/// </summary>
		bool UseSyncPreparation { get; set; }
	}
}