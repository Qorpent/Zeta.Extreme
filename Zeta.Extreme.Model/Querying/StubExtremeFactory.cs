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
// PROJECT ORIGIN: Zeta.Extreme.Model/StubExtremeFactory.cs
#endregion
 namespace Zeta.Extreme.Model.Querying {
	class StubExtremeFactory : IExtremeFactory {

		/// <summary>
		/// Создает сессии
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает запросы
		/// </summary>
		/// <returns></returns>
		public IQuery CreateQuery(QuerySetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает TimeHandler
		/// </summary>
		/// <returns></returns>
		public ITimeHandler CreateTimeHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		public IFormulaStorage GetFormulaStorage() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}
	}
}