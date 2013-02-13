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
using Comdiv.Collections;
using Comdiv.MVC;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web{
	/// <summary>
	/// Проверяет пересоздаение ячейки
	/// </summary>
    public class RecreateCellValidator : IRecreateCellValidator{
        #region IRecreateCellValidator Members

		/// <summary>
		/// Выполняет проверку целевых ячеек, возвращает результат проверки
		/// </summary>
		/// <param name="targetResult"></param>
		/// <param name="request"></param>
		/// <param name="cells"></param>
		/// <returns></returns>
		public ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
                                         IEnumerable<IZetaCell> cells){
            foreach (var cell in cells){
                if (cell.Id == 0){
                    var checkExisted = new InputRowHelper().Get(cell.Object, cell.DetailObject, cell.Row,
                                                                new ColumnDesc(cell.Column.Code, cell.Year, cell.Period),
                                                                // { DirectDate = cell.DirectDate },
                                                                false);
                    if (checkExisted != null){
                        if (cell.Value != checkExisted.Value){
                            //redirect
                            cell.Tag = checkExisted;
                        }
                        else{
                            cell.Tag = "ignore";
                        }
                        //targetResult.Fail(string.Format("try to recreate cell {0}", cell));
                    }
                }
            }
            return targetResult;
        }

        #endregion
    }
}