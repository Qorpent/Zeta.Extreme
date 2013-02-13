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
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Caching;
using Comdiv.IO;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateCache : Cache<IInputTemplate>, IInputTemplateCache{
        private IFilePathResolver _pathResolver;
        private string path;

        public IList<IInputTemplate> Templates{
            get { return Values.ToList(); }
        }

        #region IInputTemplateCache Members

        public string Path{
            get { return path ?? "forms/"; }
            set { path = value; }
        }

        public
            IFilePathResolver PathResolver{
            get { return _pathResolver ?? (myapp.files); }
            set { _pathResolver = value; }
        }

        public override void Reload(){
            base.Reload();
            var forms = PathResolver.ResolveAll(Path, "*.xml");
            foreach (var file in forms){
                var doc = XElement.Parse(PathResolver.Read(file));
                foreach (var template in new InputTemplateXmlSerializer().Read(doc)){
                    this[template.Code] = (template);
                }
            }
        }

        #endregion
    }
}