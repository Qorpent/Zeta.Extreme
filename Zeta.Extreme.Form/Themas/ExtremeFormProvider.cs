#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ExtremeFormProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Qorpent;
using Qorpent.IoC;
using Zeta.Extreme.Form.InputTemplates;

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
		/// 	Провайдер с указанной папкой для загрузки
		/// </summary>
		/// <param name="rootdir"> </param>
		public ExtremeFormProvider(string rootdir) {
			_rootdir = rootdir;
			_loadTask = Task.Run(() => DoLoad());
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

		private void DoLoad() {
			lock (_loadsync) {
				var options = ThemaLoaderOptions.GetExtremeFormOptions(_rootdir);
				var configurator = new ThemaConfigurationProvider(options);
				var themaFactoryProvider = new ThemaFactoryProvider {ConfigurationProvider = configurator};
				_factory = themaFactoryProvider.Get();
			}
		}

		private readonly object _loadsync = new object();
		private readonly string _rootdir;

		private IThemaFactory _factory;
		private Task _loadTask;
	}
}