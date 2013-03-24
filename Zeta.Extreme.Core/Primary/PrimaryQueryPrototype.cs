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
// PROJECT ORIGIN: Zeta.Extreme.Core/PrimaryQueryPrototype.cs
#endregion
namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Описатель прототипа первичного запроса
	/// </summary>
	public struct PrimaryQueryPrototype {
		
		/// <summary>
		/// Marks that query requires zeta.data instead of zeta.cell
		/// </summary>
		public bool UseViewInsteadOfTable { get; set; }

		/// <summary>
		/// 	Признак использования агрегации
		/// </summary>
		public bool UseSum { get; set; }

		/// <summary>
		/// 	Запрет на использование деталей
		/// </summary>
		public bool PreserveDetails { get; set; }

		/// <summary>
		/// 	Потребноcть в использовании деталей
		/// </summary>
		public bool RequireDetails { get; set; }

		/// <summary>
		/// 	Использование специального метода доступа к первичным значениям
		/// </summary>
		public bool UseZetaEval { get; set; }

		/// <summary>
		/// 	Целевая валюта
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		/// Marks that period is aggregated period
		/// </summary>
		public bool AggregatePeriod { get; set; }
	}
}