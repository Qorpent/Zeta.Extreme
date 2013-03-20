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
// PROJECT ORIGIN: Zeta.Extreme.Model/QuerySetupInfo.cs
#endregion
namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Настройки запроса при создании
	/// </summary>
	public class QuerySetupInfo {
		/// <summary>
		/// 
		/// </summary>
		public QuerySetupInfo() {
			Row = ExtremeFactory.CreateRowHandler();
			Col = ExtremeFactory.CreateColumnHandler();
			Obj = ExtremeFactory.CreateObjHandler();
			Time = ExtremeFactory.CreateTimeHandler();
		}
		/// <summary>
		/// Описатель строки
		/// </summary>
		public IRowHandler Row { get;  set; }
		/// <summary>
		/// Описатель колонки
		/// </summary>
		public IColumnHandler Col { get; set; }
		/// <summary>
		/// Описатель объекта
		/// </summary>
		public IObjHandler Obj { get; set; }
		/// <summary>
		/// Описатель времени
		/// </summary>
		public ITimeHandler Time { get;  set; }
		/// <summary>
		/// Описатель валюты
		/// </summary>
		public string Valuta { get; set; }
	}
}