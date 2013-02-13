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
using Comdiv.Extensions;
using Comdiv.MVC;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web{
	/// <summary>
	/// Проверяет, что используется правильный целевой объект
	/// </summary>
    public class CellsAreOfTargetOrg : ICheckRequestObject{
        #region ICheckRequestObject Members

		/// <summary>
		/// Выполняет проверку целевых ячеек, возвращает результат проверки
		/// </summary>
		/// <param name="targetResult"></param>
		/// <param name="request"></param>
		/// <param name="cells"></param>
		/// <returns></returns>
		public ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
                                         IEnumerable<IZetaCell> cells){
            try{

                if (request.Template.Parameters.get("anyobject", "false").toBool()){
                    return targetResult;
                }
                var orgToCheck = request.Template.FixedObject ?? request.Object;
                //если запрос прошел проверки и при этом не связан с объектом, ячейки могут быть из разных объектов
                if (null == orgToCheck){
                    return targetResult;
                }
                foreach (var cell in cells){
                    if (!cell.Object.Path.Contains("/"+orgToCheck.Id+"/")){
                        targetResult.IsValid = false;
                        targetResult.Messages.Add(
                            string.Format(
                                "cell with Id {0} has not allowed object {1} (requested {2})",
                                cell.Id,
                                cell.Object.Name, orgToCheck.Name));
                    }
                    if (cell.DetailObject != null){
                        if (!cell.DetailObject.Object.Path.Contains("/" + orgToCheck.Id + "/"))
						{
                            targetResult.IsValid = false;
                            string.Format(
                                "cell with Id {0} has not allowed detail object {1} of {2} (requested {3})",
                                cell.Id,
                                cell.DetailObject.Name, cell.Object.Name,
                                orgToCheck.Name);
                        }
                    }
                }
                return targetResult;
            }
            catch (Exception ex){
                targetResult.Error = ex;
                return targetResult;
            }
        }

        #endregion
    }
}