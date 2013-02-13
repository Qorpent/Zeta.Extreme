// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.IO;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Построитель фабрики тем
	/// </summary>
    public class ThemaFactoryProvider : IThemaFactoryProvider{
		/// <summary>
		/// Кэшированная фабрика
		/// </summary>
        protected internal IThemaFactory factory;

        /// <summary>
        /// стандартная фабрика фабрики
        /// </summary>
        public ThemaFactoryProvider(){
            myapp.OnReload += myapp_OnReload;
        }

        private bool checkneedload = true;
        DateTime lastloadversion = new DateTime(1900,1,1);
        void myapp_OnReload(object sender, Common.EventWithDataArgs<int> args){

            factory = null;


        }

		/// <summary>
		/// Перегрузить фабрику
		/// </summary>
		public void Reload() {
#if TC
            factory = null;
#else
            checkneedload = true;
#endif
        }

        /// <summary>
        /// Ссылка на конфигуратор тем
        /// </summary>
        public IThemaConfigurationProvider ConfigurationProvider { get; set; }


		/// <summary>
		/// Получить фабрику
		/// </summary>
		/// <returns></returns>
		public IThemaFactory Get(){
            lock (this){
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

        private void doLoad(){
            var cfg = ConfigurationProvider.Get();
            factory = cfg.Configure();
         //   GC.Collect();
            lastloadversion = DateTime.Now;
            lastcheck = DateTime.Now;
        }


        private DateTime lastcheck;

#if !TC

        bool doCheckLoad(){
            if((DateTime.Now-lastcheck).TotalSeconds < 10) {
                return false;
            }
            
            if(!myapp.files.Exists("~/tmp/compiled_themas/.compilestamp")) return true;
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
        /// Возвращает стандартным способом типовой элемент
        /// </summary>
        /// <param name="code"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDefault<T>(string code  ) where T:class {
            var factory = myapp.ioc.get<IThemaFactoryProvider>().Get();
            if(typeof(IThema).IsAssignableFrom(typeof(T)))return factory.Get(code) as T;
            if (typeof(IReportDefinition).IsAssignableFrom(typeof(T))) return factory.GetReport(code) as T;
            if (typeof(IInputTemplate).IsAssignableFrom(typeof(T))) return factory.GetForm(code) as T;
            return null;
        }
    }
}