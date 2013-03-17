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
// PROJECT ORIGIN: Zeta.Extreme.Model/IQuery.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Интерфейс описателя запроса
	/// </summary>
	public interface IQuery : IWithCacheKey {
		/// <summary>
		/// 	Условие на время
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	Условие на строку
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	Условие на колонку
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	Условие на объект
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		string Valuta { get; set; }

		/// <summary>
		/// Текущий кэшированный резултат
		/// </summary>
		QueryResult Result { get; set; }

		/// <summary>
		/// 	Обеспечивает возврат результата запроса с ожиданием
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);

		/// <summary>
		/// 	Автоматический код запроса, присваиваемый системой
		/// </summary>
		long Uid { get; set; }

		/// <summary>
		/// 	Обратная ссылка на сессию
		/// </summary>
		ISession Session { get; set; }

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <param name="deep"> Если да, то делает копии вложенных измерений </param>
		/// <returns> </returns>
		IQuery Copy(bool deep = false);

		/// <summary>
		/// 	Стандартная процедура нормализации
		/// </summary>
		void Normalize(ISession session = null);

	
	}
}