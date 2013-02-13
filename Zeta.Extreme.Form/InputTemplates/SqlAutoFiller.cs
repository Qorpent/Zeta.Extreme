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
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Web.Themas;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates{
	/// <summary>
	/// Автозаполнение на SQL
	/// </summary>
    public class SqlAutoFiller : IAutoFillExecutor{
        private IInversionContainer _container;

        /// <summary>
        /// Ссылка на контейнер
        /// </summary>
        public IInversionContainer Container{
            get{
                if (_container.invalid()){
                    lock (this){
                        if (_container.invalid()){
                            Container = myapp.ioc;
                        }
                    }
                }
                return _container;
            }
            set { _container = value; }
        }


		/// <summary>
		/// Выполнить автозаполнение
		/// </summary>
		/// <param name="autoFill"></param>
		/// <param name="obj"></param>
		public void Execute(AutoFill autoFill, IZetaMainObject obj){
            var sql = autoFill.CallData;
	        var code = (autoFill.Template.Thema as IThema).GetParameter("fillcode","");
			if(code.noContent()) {
				code = autoFill.Template.Form.Code;
				if (autoFill.Template.Rows.Count > 1) {
					code = (autoFill.Template.Thema as IThema).Code;
				}
			}

	        var dict = new Dictionary<string, object>{
                                                         {"@obj", obj.Id},
                                                         {"@form", code},
                                                         {"@year", autoFill.Template.Year},
                                                         {"@period", autoFill.Template.Period},
                                                         {"@dobj", 0},
                                                         {"@date", autoFill.Template.DirectDate}
                                                     };

            if (sql == "DEFAULT"){
                sql =
                    @"
                exec usm.AutoFill 
                        @obj=@obj,@form=@form,
                        @year=@year,@period=@period,
                        @dobj=@dobj,@date=@date
                ";
            }
            Container.getConnection().ExecuteNonQuery(sql, dict);
        }

    }
}