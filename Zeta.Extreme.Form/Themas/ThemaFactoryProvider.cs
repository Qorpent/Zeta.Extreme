#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ThemaFactoryProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.IO;
using Comdiv.Inversion;
using Comdiv.Reporting;
using Zeta.Extreme.Form.InputTemplates;

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
		/// 	стандартная фабрика фабрики
		/// </summary>
		public ThemaFactoryProvider() {
			myapp.OnReload += myapp_OnReload;
		}

		private bool checkneedload = true;
		private DateTime lastloadversion = new DateTime(1900, 1, 1);

		private void myapp_OnReload(object sender, EventWithDataArgs<int> args) {
			factory = null;
		}

		/// <summary>
		/// 	Перегрузить фабрику
		/// </summary>
		public void Reload() {
#if TC
            factory = null;
#else
			checkneedload = true;
#endif
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
				if (null == factory
#if !TC
				    || (checkneedload
				        && doCheckLoad()
				       )
#endif
					) {
					doLoad();
				}
				return factory;
			}
		}

		private void doLoad() {
			var cfg = ConfigurationProvider.Get();
			factory = cfg.Configure();
			//   GC.Collect();
			lastloadversion = DateTime.Now;
			lastcheck = DateTime.Now;
		}


		private DateTime lastcheck;

#if !TC

		private bool doCheckLoad() {
			if ((DateTime.Now - lastcheck).TotalSeconds < 10) {
				return false;
			}

			if (!myapp.files.Exists("~/tmp/compiled_themas/.compilestamp")) {
				return true;
			}
			var cdate = myapp.files.LastWriteTime("~/tmp/compiled_themas/", "*.xml");
			var sdate = myapp.files.LastWriteTime("data", "*.bxl");

			if (sdate > cdate) {
				return true;
			}

			if (cdate > lastloadversion) {
				return true;
			}
			lastcheck = DateTime.Now;
			return false;
		}

#endif


		/// <summary>
		/// 	Возвращает стандартным способом типовой элемент
		/// </summary>
		/// <param name="code"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static T GetDefault<T>(string code) where T : class {
			var factory = myapp.ioc.get<IThemaFactoryProvider>().Get();
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