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
// PROJECT ORIGIN: Zeta.Extreme.Model/IRowHandler.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	—тандартный описатель измерени€ Row
	/// </summary>
	public interface IRowHandler : IQueryDimension<IZetaRow> {
		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsSum { get; }

		/// <summary>
		/// 	ѕроста€ копи€ услови€ на строку
		/// </summary>
		/// <returns> </returns>
		IRowHandler Copy();

		/// <summary>
		/// 	Ќормализует ссылки и параметры
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		void Normalize(ISession session, IZetaColumn column);
	}
}