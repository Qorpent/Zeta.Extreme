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
// PROJECT ORIGIN: Zeta.Extreme.Core/IQueryWithProcessing.cs
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Описатель запроса с поддержкой обработки
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		

		/// <summary>
		/// 	Sign that primary was not set
		/// </summary>
		bool HavePrimary { get; set; }

		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		Task PrepareTask { get; set; }

		/// <summary>
		/// Проверяет, является ли запрос циклическим
		/// </summary>
		bool GetIsRecycle(IDictionary<long,bool> registry = null );

		/// <summary>
		/// 	Client processed mark
		/// </summary>
		bool Processed { get; set; }

		/// <summary>
		/// 	Статус по подготовке
		/// </summary>
		PrepareState PrepareState { get; set; }

		/// <summary>
		/// 	Дочерние запросы
		/// </summary>
		IList<IQuery> FormulaDependency { get; }

		/// <summary>
		/// 	Проверяет "первичность запроса"
		/// </summary>
		bool IsPrimary { get; }

		/// <summary>
		/// 	Зависимости для суммовых запросов
		/// </summary>
		IList<Tuple<decimal, IQuery>> SummaDependency { get; }

		/// <summary>
		/// 	Формула, которая присоединяется к запросу на фазе подготовки
		/// </summary>
		IFormula AssignedFormula { get; set; }

		/// <summary>
		/// 	Тип вычисления запроса
		/// </summary>
		QueryEvaluationType EvaluationType { get; set; }
		/// <summary>
		/// Связанный ключ формулы
		/// </summary>
		string AssignedFormulaKey { get; set; }

		/// <summary>
		/// 	Позволяет синхронизировать запросы в подсессиях
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPrepare(int timeout = -1);
	}
}