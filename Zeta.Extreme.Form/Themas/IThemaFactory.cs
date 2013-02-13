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
using System.Security.Principal;
using System.Xml.Linq;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Фабрика тем
	/// </summary>
    public interface IThemaFactory:IDisposable {
		/// <summary>
		/// Получить тему по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        IThema Get(string code);
		/// <summary>
		/// Кэш тем
		/// </summary>
        IDictionary<string, object> Cache { get; }
		/// <summary>
		/// Исходный XML
		/// </summary>
        string SrcXml { get; set; }
		/// <summary>
		/// Версия
		/// </summary>
    	DateTime Version { get; set; }
		/// <summary>
		/// Получить все темы
		/// </summary>
		/// <returns></returns>
    	IEnumerable<IThema> GetAll();
		/// <summary>
		/// Получить дефиницию отчета
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        IReportDefinition GetReport(string code);
		/// <summary>
		/// Получить тему в адаптации на текущего пользователя
		/// </summary>
		/// <returns></returns>
        IEnumerable<IThema> GetForUser();
		/// <summary>
		/// Получить тему в адаптации на конкретного пользователя
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        IEnumerable<IThema> GetForUser(IPrincipal usr);
		/// <summary>
		/// Получить шаблон формы
		/// </summary>
		/// <param name="code"></param>
		/// <param name="throwerror"></param>
		/// <returns></returns>
        IInputTemplate GetForm(string code,bool throwerror = false);
		/// <summary>
		/// Очистить кэш пользователя
		/// </summary>
		/// <param name="usrname"></param>
        void CleanUser(string usrname);
    }
}