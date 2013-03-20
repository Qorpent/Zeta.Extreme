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
// PROJECT ORIGIN: Zeta.Extreme.Model/IPrimarySource.cs
#endregion
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Абстракция акцессора первичных данных
	/// </summary>
	public interface IPrimarySource {
		/// <summary>
		/// 	Регистрирует целевой запрос
		/// </summary>
		/// <param name="query"> </param>
		void Register(IQuery query);

		/// <summary>
		/// 	Регистрирует заранее подготовленный SQL-запрос
		/// </summary>
		/// <param name="preparedQuery"> </param>
		void Register(string preparedQuery);

		/// <summary>
		/// 	Получает асинхронную задачу сбора текущих данных,
		/// 	завершение задачи означает окончание всех текущих запросов
		/// </summary>
		/// <returns> </returns>
		Task Collect();

		/// <summary>
		/// 	Выполняет все требуемые запросы в режиме ожидания
		/// </summary>
		void Wait(int timeout = -1);

		/// <summary>
		/// 	Журнал выполненных SQL
		/// </summary>
		IList<string> QueryLog { get; }
	}
}