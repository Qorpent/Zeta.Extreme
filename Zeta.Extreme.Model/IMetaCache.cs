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
// PROJECT ORIGIN: Zeta.Extreme.Model/IMetaCache.cs
#endregion
using Qorpent.Model;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 	Интерфейс локального хранилища метаданных
	/// </summary>
	public interface IMetaCache {
		/// <summary>
		/// 	Родительский кэш
		/// </summary>
		IMetaCache Parent { get; set; }

		/// <summary>
		/// 	Получить объект из хранилища
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		T Get<T>(object id) where T : class, IEntity;

		/// <summary>
		/// 	Сохранить объект в хранилище
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		IMetaCache Set<T>(T item) where T : class, IEntity;


	}
}