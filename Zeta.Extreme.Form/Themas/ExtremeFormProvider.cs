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
// PROJECT ORIGIN: Zeta.Extreme.Form/ExtremeFormProvider.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Упрощенный провайдер форм для Extreme
	/// </summary>
	[ContainerComponent(Lifestyle.Singleton, Name = "extreme.form", ServiceType = typeof (IExtremeFormProvider))]
	public class ExtremeFormProvider : ServiceBase, IExtremeFormProvider {
		/// <summary>
		/// 	Стандартный провайдер форм
		/// </summary>
		public ExtremeFormProvider() {}

		/// <summary>
		/// 	Провайдер с указанной папкой для загрузки, загрузка начинается асинхронно
		/// </summary>
		/// <param name="rootdir"> </param>
		public ExtremeFormProvider(string rootdir) {
			_rootdir = rootdir;
			_loadTask = Task.Run(() => DoLoad());
		}

		/// <summary>
		/// 	Провайдер с прямым набором тем, или с директорией, опционально источник расширений, загрузка должна вызываться синхронно (по сути тестовый режим)
		/// </summary>
		/// <param name="rootdir"></param>
		/// <param name="directdefs"></param>
		/// <param name="propertysoruces"></param>
		public ExtremeFormProvider(string rootdir=null,IEnumerable<XElement> directdefs=null, IEnumerable<IBizCasePropertySource> propertysoruces =null )
		{
			if(null!=directdefs)_directdefs = directdefs.ToArray();
			_rootdir = rootdir;
			if(null!=propertysoruces)_propertysoruces = propertysoruces.ToArray();
		}

		/// <summary>
		/// 	Флаг нахождения в режиме загрузки
		/// </summary>
		public bool IsInLoadProcess {
			get { return !_loadTask.IsCompleted; }
		}

		/// <summary>
		/// 	Прямой акцессор к фабрике
		/// </summary>
		public IThemaFactory Factory {
			get {
				if (null != _loadTask) {
					_loadTask.Wait();
				}
				lock (_loadsync) {
					return _factory;
				}
			}
		}

		/// <summary>
		/// 	Принудительная перезагрузка фабрики
		/// </summary>
		public void Reload(bool async = false) {
			if (null != _loadTask) {
				_loadTask.Wait();
			}
			lock (_loadsync) {
				if (async) {
					_loadTask = Task.Run(() => DoLoad());
					return;
				}
				DoLoad();
			}
		}

		/// <summary>
		/// 	Получить шаблон по коду
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		public IInputTemplate Get(string code) {
			if (null != _loadTask) {
				_loadTask.Wait(); //синхронизируемся с загрузчиком
			}
			lock (_loadsync) {
				if (null == Factory) {
					throw new Exception("something wrong with factory");
				}
				return Factory.GetForm(code);
			}
		}

		/// <summary>
		/// Direct call to load
		/// </summary>
		public void DoLoad() {
			lock (_loadsync) {
				var options = ThemaLoaderOptionsHelper.GetExtremeFormOptions(_rootdir);
				options.DirectThemaConfigurations = _directdefs;
				options.PropertySources = _propertysoruces;
				var configurator = new ThemaConfigurationProvider(options);
				var themaFactoryProvider = new ThemaFactoryProvider {ConfigurationProvider = configurator};
				_factory = themaFactoryProvider.Get();
			}
		}

		private readonly object _loadsync = new object();
		private readonly string _rootdir;

		private IThemaFactory _factory;
		private Task _loadTask;
		private XElement[] _directdefs;
		private IBizCasePropertySource[] _propertysoruces;
	}
}