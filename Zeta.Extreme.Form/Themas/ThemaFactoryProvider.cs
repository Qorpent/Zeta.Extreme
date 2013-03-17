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
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaFactoryProvider.cs
#endregion
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Reports;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Построитель фабрики тем
	/// </summary>
	public class ThemaFactoryProvider : IThemaFactoryProvider {
		/// <summary>
		/// 	Кэшированная фабрика
		/// </summary>
		protected internal IThemaFactory factory;


		/// <summary>
		/// 	Перегрузить фабрику
		/// </summary>
		public void Reload() {
			factory = null;
		}

		/// <summary>
		/// 	Ссылка на конфигуратор тем
		/// </summary>
		public IThemaConfigurationProvider ConfigurationProvider { get; set; }


		/// <summary>
		/// 	Получить фабрику
		/// </summary>
		/// <returns> </returns>
		public IThemaFactory Get() {
			lock (this) {
				if (null == factory) {
					var cfg = ConfigurationProvider.Get();
					factory = cfg.Configure();
				}
				return factory;
			}
		}


		/// <summary>
		/// 	Возвращает стандартным способом типовой элемент
		/// </summary>
		/// <param name="code"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static T GetDefault<T>(string code) where T : class {
			var factory = Application.Current.Container.Get<IThemaFactoryProvider>().Get();
			if (typeof (IThema).IsAssignableFrom(typeof (T))) {
				return factory.Get(code) as T;
			}
			if (typeof (IReportDefinition).IsAssignableFrom(typeof (T))) {
				return factory.GetReport(code) as T;
			}
			if (typeof (IInputTemplate).IsAssignableFrom(typeof (T))) {
				return factory.GetForm(code) as T;
			}
			return null;
		}
	}
}