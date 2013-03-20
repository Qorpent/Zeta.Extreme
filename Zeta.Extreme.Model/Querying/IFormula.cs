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
// PROJECT ORIGIN: Zeta.Extreme.Model/IFormula.cs
#endregion
namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Базовый интерфейс формулы
	/// </summary>
	public interface IFormula {
		/// <summary>
		/// 	Настраивает формулу на конкретный переданный запрос
		/// </summary>
		/// <param name="query"> </param>
		void Init(IQuery query);

		/// <summary>
		/// 	Устанавливает контекст использования формулы
		/// </summary>
		/// <param name="request"> </param>
		void SetContext(FormulaRequest request);

		/// <summary>
		/// 	Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
		/// </summary>
		/// <param name="query"> </param>
		void Playback(IQuery query);

		/// <summary>
		/// 	Команда вычисления результата
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		QueryResult Eval();

		/// <summary>
		/// 	Выполняет очистку ресурсов формулы после использования
		/// </summary>
		void CleanUp();
	}
}