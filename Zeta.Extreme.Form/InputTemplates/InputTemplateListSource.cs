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
using System.Security.Principal;
using Comdiv.Model;
using Comdiv.MVC.Controllers;
using Comdiv.Security.Acl;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateListSource : IAclSource{
        #region IAclSource Members

        public AclList Get(IPrincipal principal){
            var result = new AclList();
            result.Name = "Формы ввода";
            foreach (var template in new InputTemplateRepository().GetMyTemplates()){
                var e = new Entity();
                e.Code = template.Code;
                e.Name = (template).Name;
                e.Comment = acl.token(template);
                e.Action = acl.get(template, "access", "", principal).ToString();
                result.Items.Add(e);
            }
            return result;
        }

        #endregion
    }
}