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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/DataChunk.cs
#endregion
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Описывает возвращаемый набор данных из сессии
	/// 	оптимизирован по трафику и для сериализации в JSON
	/// </summary>
	[Serialize]
	public class DataChunk {
		/// <summary>
		/// 	Собственно возвращаемые данные
		/// </summary>
		[SerializeNotNullOnly] public OutCell[] data;

		/// <summary>
		/// 	Сообщение об ошибке
		/// </summary>
		[SerializeNotNullOnly] public string e;

		/// <summary>
		/// 	Последний переданный индекс
		/// </summary>
		[SerializeNotNullOnly] public int ei;

		/// <summary>
		/// 	Первый индекс
		/// </summary>
		[SerializeNotNullOnly] public int si;

		///<summary>
		///	Статус передачи (e,w,f)
		///</summary>
		[Serialize] public string state;
	}
}