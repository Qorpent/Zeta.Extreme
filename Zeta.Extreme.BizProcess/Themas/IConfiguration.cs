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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IConfiguration.cs
#endregion
using Qorpent.Model;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интерфейс конфигурации типизированной темы (конкретного типа)
	/// </summary>
	/// <typeparam name="T"> </typeparam>
	public interface IConfiguration<T> : IWithCode, IWithName {
		/// <summary>
		/// 	Тип темы (имя)
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		string Role { get; set; }

		/// <summary>
		/// 	Ссылка
		/// </summary>
		string Url { get; set; }

		/// <summary>
		/// 	Шаблон
		/// </summary>
		string Template { get; set; }

		/// <summary>
		/// 	Признак наличия ошибки
		/// </summary>
		bool IsError { get; set; }

		/// <summary>
		/// 	Конфигурация темы
		/// </summary>
		IThemaConfiguration Thema { get; set; }

		/// <summary>
		/// 	Команда на формирование конфигурации
		/// </summary>
		/// <returns> </returns>
		T Configure();
	}
}