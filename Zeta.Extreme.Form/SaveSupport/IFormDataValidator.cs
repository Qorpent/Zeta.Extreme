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
using System.Collections.Generic;
using Comdiv.MVC;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web{
	/// <summary>
	/// Интерфейс валидизации входных сохраняемых данных
	/// </summary>
    public interface IFormDataValidator{
        /// <summary>
        /// Выполняет проверку целевых ячеек, возвращает результат проверки
        /// </summary>
        /// <param name="targetResult"></param>
        /// <param name="request"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
                                  IEnumerable<IZetaCell> cells);
    }
}