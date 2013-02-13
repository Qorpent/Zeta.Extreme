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
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Фабрика тем
	/// </summary>
    public class ThemaFactory : IThemaFactory{
        /// <summary>
        /// Кэш тем
        /// </summary>
        public readonly IList<IThema> Themas = new List<IThema>();

        private readonly IDictionary<string, object> cache = new Dictionary<string, object>();


		/// <summary>
		/// Исходный XML
		/// </summary>
		public string SrcXml { get; set; }

		/// <summary>
		/// Получить тему по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public IThema Get(string code) {
            return Themas.FirstOrDefault(x => x.Code == code);
        }

		/// <summary>
		/// Кэш тем
		/// </summary>
		public IDictionary<string, object> Cache{
            get { return cache; }
        }

		/// <summary>
		/// Версия
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// Получить все темы
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IThema> GetAll(){
            return new List<IThema>(Themas);
        }

		/// <summary>
		/// Получить тему в адаптации на текущего пользователя
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IThema> GetForUser(){
            return GetForUser(myapp.usr);
        }

		/// <summary>
		/// Очистить кэш пользователя
		/// </summary>
		/// <param name="usrname"></param>
		public void CleanUser(string usrname) {
            lock (this) {
                if (cache.ContainsKey(usrname))
                {
                    cache.Remove(usrname);
                }    
            }
            
        }

		/// <summary>
		/// Получить тему в адаптации на конкретного пользователя
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
		public IEnumerable<IThema> GetForUser(IPrincipal usr){
            return cache.get(usr.Identity.Name, () => internalGetForUser(usr));
        }


		/// <summary>
		/// Получить шаблон формы
		/// </summary>
		/// <param name="code"></param>
		/// <param name="throwerror"></param>
		/// <returns></returns>
		public IInputTemplate GetForm(string code,bool throwerror = false){
            return cache.get(code + ".in", () =>{
                                               var thema =
                                                   Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
                                                       t => code.StartsWith(t.Code));
                                               if (null == thema){
                                                   return null;
                                               }
                                               var result= thema.GetForm(code);
				if(null==result && throwerror) {
					throw new Exception("cannto find form with code "+code);
				}
                                                	return result;
            }, true
                );
        }

		/// <summary>
		/// Получить дефиницию отчета
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public IReportDefinition GetReport(string code){
            return cache.get(code + ".out", () =>{
                                                var thema =
                                                    Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
                                                        t => code.StartsWith(t.Code));
                                                if (null == thema){
                                                    return null;
                                                }
                                                return thema.GetReport(code);
                                            }, true);
        }



        private IEnumerable<IThema> internalGetForUser(IPrincipal usr){
            var personalized = Themas.Select(x => x.Personalize(usr)).ToArray();
            var active = personalized.Where(x => x.IsActive(usr)).BindParents().ToArray();

            active = active.OrderBy(x => x, new themaidxcomparer()).ToArray();
            return active;
        }

        private class themaidxcomparer : IComparer<IThema>{
	        /// <summary>
	        /// Сравнивает два объекта и возвращает значение, показывающее, что один объект меньше или больше другого или равен ему.
	        /// </summary>
	        /// <returns>
	        /// Знаковое целое число, которое определяет относительные значения <paramref name="x"/> и <paramref name="y"/>, как показано в следующей таблице. Значение  Значение  Меньше нуля Значение параметра <paramref name="x"/> меньше значения параметра <paramref name="y"/>. Zero Значение параметра <paramref name="x"/> равно значению параметра <paramref name="y"/>. Больше нуля. Значение <paramref name="x"/> больше значения <paramref name="y"/>.
	        /// </returns>
	        /// <param name="x">Первый сравниваемый объект.</param><param name="y">Второй сравниваемый объект.</param>
	        public int Compare(IThema x, IThema y){
                if (x.Parent.hasContent() && y.Parent.noContent()){
                    return -1;
                }
                if (y.Parent.hasContent() && x.Parent.noContent()){
                    return 1;
                }
                if (y.Parent.hasContent() && x.Parent.hasContent()){
                    return 0;
                }
                if (x.Parent.hasContent() && y.Code == x.Parent){
                    return 1;
                }
                if (y.Parent.hasContent() && x.Code == y.Parent){
                    return -1;
                }
                return x.Idx.CompareTo(y.Idx);
            }

        }


        public void Dispose(){
            foreach (var thema in Themas){
                ((Thema)thema).Reports.Clear();
                ((Thema)thema).Forms.Clear();
                ((Thema)thema).Documents.Clear();
                ((Thema)thema).Commands.Clear();
            }
            this.Themas.Clear();
        }
    }
}