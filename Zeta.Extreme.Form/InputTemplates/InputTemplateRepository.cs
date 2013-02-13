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
using System.Linq;
using Comdiv.Application;
using Comdiv.Inversion;
using Comdiv.Security.Acl;
using Comdiv.Zeta.Web.Themas;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateRepository{
        private IInversionContainer _container;

        public IInversionContainer Container{
            get{
                if (_container.invalid()){
                    lock (this){
                        if (_container.invalid()){
                            Container = ioc.Container;
                        }
                    }
                }
                return _container;
            }
            set { _container = value; }
        }

        public IList<string> GetAllTemplateCodes(){
            return myapp.storage.Get<IInputTemplate>().All().Select(i => i.Code).ToList();
        }

        public IInputTemplate GetTemplate(string code){
            lock (this){
                var thcode = code;
                if (code.EndsWith(".in")){
                    code = code.Substring(0, code.Length - 3);
                }
                var th = Container.get<IThemaFactoryProvider>().Get().GetForm(thcode);
                if (null != th){
                    return th.Clone();
                }
                return myapp.storage.Get<IInputTemplate>().Load(code);
            }
        }

        public IList<IInputTemplate> GetMyTemplates(){
            lock (this){
                var templates = myapp.storage.Get<IInputTemplate>().All().ToArray();
                return templates.Where(acl.get).ToList();
            }

            //var allowedTemplates = ioc.get<IApplicationFileSystem>()["//zeta_data_security/templates"];
            //if (allowedTemplates.HasContent() && allowedTemplates != "*"){
            //    var at = allowedTemplates.Split(';', ' ', ',').Select(t => t.Trim()).Where(t => t.HasContent());
            //    templates = templates.Where(t => at.Contains(t.Code));
            //}

            // var authorizer = ioc.get<IAuthorizeService>();
            //if (authorizer == null) return templates.ToList();

            /*return templates.Where(
                t => authorizer.Authorize("input-security", false, new MvcContext()
                                                                       .AdminAlways()
                                                                       .Apply(t)
                                                                       .Attach(t)
                                                                       .SetUser(ioc.get<IPrincipalSourceService>().Current))).ToList();}
            */
        }
    }
}