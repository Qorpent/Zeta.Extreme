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
using System.Linq;
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Zeta.Extreme.Form.Themas;

namespace Comdiv.Zeta.Web.Themas{
    public class ListDefinitionHqlGenerator : XmlGeneratorBase{
        public string Query { get; set; }

        protected override void Prepare(XElement call){
            base.Prepare(call);
            Query = call.attr("query");
        }

        protected override object[] InternalGenerate(){
            var items =
                Enumerable.ToArray<IEntityDataPattern>(
                    myapp.storage.GetDefault().Query(Query).Cast<IEntityDataPattern>().OrderBy(
                        x => (100000 + x.Idx()) + x.Name));
            var str = " : -- |" + StringExtensions.concat(items.Select(x => x.Code + ":" + x.Name), "|");
            return new[]{new XText(str)};
        }
    }
}