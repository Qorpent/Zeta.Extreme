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
// PROJECT ORIGIN: Zeta.Extreme.Model/ExtremeFactory.cs
#endregion
using Qorpent.Applications;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Фабрика для создания сессий и запросов
	/// </summary>
	public static class ExtremeFactory {
		private static IExtremeFactory _factory;
		private static readonly object Sync =new object();

		/// <summary>
		/// Создать сессию
		/// </summary>
		/// <returns></returns>
		public static ISession CreateSession(SessionSetupInfo info = null) {
			return RealFactory.CreateSession(info);
		}

		/// <summary>
		/// Создать описатель строки
		/// </summary>
		/// <returns></returns>
		public static IRowHandler CreateRowHandler()
		{
			return RealFactory.CreateRowHandler();
		}
		/// <summary>
		/// Доступ к хранилищу формул
		/// </summary>
		/// <returns></returns>
		public static IFormulaStorage GetFormulaStorage() {
			return RealFactory.GetFormulaStorage();
		}

		/// <summary>
		/// Создать описатель колонки
		/// </summary>
		/// <returns></returns>
		public static IColumnHandler CreateColumnHandler()
		{
			return RealFactory.CreateColumnHandler();
		}

		/// <summary>
		/// Создать описатель объекта
		/// </summary>
		/// <returns></returns>
		public static IObjHandler CreateObjHandler()
		{
			return RealFactory.CreateObjHandler();
		}


		/// <summary>
		/// Создать описатель времени
		/// </summary>
		/// <returns></returns>
		public static ITimeHandler CreateTimeHandler()
		{
			return RealFactory.CreateTimeHandler();
		}



		/// <summary>
		/// Создать запрос
		/// </summary>
		/// <returns></returns>
		public static IQuery CreateQuery(QuerySetupInfo info=null) {
			return RealFactory.CreateQuery(info);
		}
		/// <summary>
		/// Реальная целевая фабрика
		/// </summary>
		public static IExtremeFactory RealFactory {
			get {
				if(null!=_factory) return _factory;
				lock (Sync) {
					_factory = Application.Current.Container.Get<IExtremeFactory>() ?? new StubExtremeFactory();
					return _factory;
				}
			}set {
				lock (Sync) {
					_factory = value;
				}
			}
		}
		/// <summary>
		/// Создает описатель ссылки
		/// </summary>
		/// <returns></returns>
		public static IReferenceHandler CreateReference() {
			return RealFactory.CreateReference();
		}
	}
}