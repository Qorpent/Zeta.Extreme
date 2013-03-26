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
// PROJECT ORIGIN: Zeta.Extreme.Core/DefaultExtremeFactory.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// Фабрика классов Extreme
	/// </summary>
	public class DefaultExtremeFactory : IExtremeFactory {
		
		/// <summary>
		/// Создает сессии
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			setupInfo = setupInfo ?? new SessionSetupInfo();
			return new Session(setupInfo.CollectStatistics);
		}

		/// <summary>
		/// Создает запросы
		/// </summary>
		/// <returns></returns>
		public IQuery CreateQuery(QuerySetupInfo setupInfo = null) {
			var result = new Query();
			if(null!=setupInfo) {
				result.Row = setupInfo.Row;
				result.Col = setupInfo.Col;
				result.Obj = setupInfo.Obj;
				result.Time = setupInfo.Time;
				result.Currency = setupInfo.Valuta;
			}
			return result;
		}

		/// <summary>
		/// Создает RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			return new RowHandler();
		}

		/// <summary>
		/// Создает ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			return new ColumnHandler();
		}

		/// <summary>
		/// Создает ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			return new ObjHandler();
		}

		/// <summary>
		/// Создает TimeHandler
		/// </summary>
		/// <returns></returns>
		public ITimeHandler CreateTimeHandler() {
			return new TimeHandler();
		}

		/// <summary>
		/// Акцессор к хранилищу формул
		/// </summary>
		/// <returns></returns>
		public IFormulaStorage GetFormulaStorage() {
			return FormulaStorage.Default;
		}
	}
}