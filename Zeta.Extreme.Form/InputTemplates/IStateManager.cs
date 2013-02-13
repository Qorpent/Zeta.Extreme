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
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates{
	/// <summary>
	/// Интерфейс класса управления статусами
	/// </summary>
    public interface IStateManager{
		/// <summary>
		/// Признак возможности установить статус
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <returns></returns>
        bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state);
		/// <summary>
		/// ВЫполнить установку статуса
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <param name="parent"></param>
        void Process(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state, int parent);
        /// <summary>
        /// Найти зависимые формы
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IInputTemplate[] GetDependentTemplates(IInputTemplate source);
        /// <summary>
        /// Найти формы от которых зависит текущая
        /// </summary>
        /// <param name="target"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IInputTemplate[] GetSourceTemplates(IInputTemplate target, IZetaMainObject obj);
        /// <summary>
        /// Определить главную форму
        /// </summary>
        /// <param name="safer"></param>
        /// <returns></returns>
        IInputTemplate GetMainTemplate(IInputTemplate safer);
		/// <summary>
		/// Определеить форму-сейфер
		/// </summary>
		/// <param name="main"></param>
		/// <returns></returns>
        IInputTemplate GetSaferTemplate(IInputTemplate main);
		/// <summary>
		/// Определяет возможность установить статус
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <param name="cause"></param>
		/// <returns></returns>
        bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
                    out string cause);

        /// <summary>
        /// Выполнить установку статуса
        /// </summary>
        /// <param name="objid"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="template"></param>
        /// <param name="templatecode"></param>
        /// <param name="usr"></param>
        /// <param name="state"></param>
        /// <param name="comment"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        int DoSet(int objid, int year, int period, string template,string templatecode,string usr, string state, string comment, int parent);
        /// <summary>
        /// Выполнить установку статуса
        /// </summary>
        /// <param name="objid"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        string DoGet(int objid, int year, int period, string template);
        /// <summary>
        /// Установить статус периода
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="state"></param>
        void SetPeriodState(int year, int period, int state);
        /// <summary>
        /// Получить статус периода
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        int GetPeriodState(int year, int period);

        /// <summary>
        /// Еще один вариант проверки возможности установки статуса
        /// </summary>
        /// <param name="template"></param>
        /// <param name="obj"></param>
        /// <param name="detail"></param>
        /// <param name="state"></param>
        /// <param name="cause"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
                                    out string cause, int parent);
    }
}