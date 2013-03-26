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
// PROJECT ORIGIN: Zeta.Extreme.Model/IQueryDimension.cs
#endregion
using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;


namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Описывает стандартное измерение запросов Zeta
	/// </summary>
	/// <typeparam name="TItem"> </typeparam>
	public interface IQueryDimension<TItem> : IWithCacheKey, IZetaQueryDimension, IEntity, IWithFormula where TItem : class, IWithCode, IWithId, IWithTag {
		/// <summary>
		/// 	Набор кодов элемента
		/// </summary>
		string[] Codes { get; set; }

		/// <summary>
		/// 	Ссылка на исходный объект
		/// </summary>
		TItem Native { get; set; }

		/// <summary>
		/// 	Множественный набор идентификаторов
		/// </summary>
		int[] Ids { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		string FormulaType { get; set; }

		/// <summary>
		/// 	Проверяет первичность элемента запроса
		/// </summary>
		/// <returns> </returns>
		bool IsPrimary();

		/// <summary>
		/// 	Нормализует объект зоны
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		void Normalize(ISession session);
		}
}