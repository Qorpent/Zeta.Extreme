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
using Comdiv.Extensions;
using Comdiv.Model.Interfaces;
using Comdiv.Model;

namespace Comdiv.Zeta.Web.Themas{
    public abstract class GenericObjectGenerator<T> : XmlGeneratorBase where T : IEntityDataPattern{
        protected T[] objects { get; set; }
        public string Prefix { get; set; }

        protected override IList<string> getTargetCodes(){
            IList<string> list;
            list = objects.Select(x => x.Code).ToList();
            return list;
        }

        protected override string getSelfCondition(string code){
            return Prefix + code.Replace("(", "_").Replace(")", "_").ToUpper();
        }

        protected override void processIncludes(){
            if (Include != null){
                objects =
                    Enumerable.ToArray<T>(
                        Include.Value.split().Select(x => myapp.storage.Get<T>().Load(x)).Where(x => x != null));
            }
            else{
                objects = Enumerable.ToArray<T>(myapp.storage.Get<T>().All().OrderBy(x => (100000 + x.Idx()) + x.Name()));
            }
        }

        protected string getBaseCondition(T otr){
            var result = "(( " + getCondition(otr.Code.Replace("(", "_").Replace(")", "_")) + ") or ALL_" + Prefix + ")";
            if (FilterCondition.hasContent()){
                result += " and " + FilterCondition;
            }
            return result;
        }

        protected object[] generateObjects(){
            var result = new List<XElement>();
            foreach (var otr in objects){
                var code = Prefix.ToLower() + "_" + otr.Id + id++;
                var o = new XElement("object");
                o.SetAttributeValue("id", otr.Id);
                o.SetAttributeValue("type", Prefix.ToLower());
                o.SetAttributeValue("name", otr.Name);
                var of = new XElement(o);
                o.SetAttributeValue("code", code);
                of.SetAttributeValue("code", code + "_orgs");
                of.SetAttributeValue("formula", "orgs");
                var cond = getBaseCondition(otr);
                o.SetAttributeValue("condition", cond + " and " + Prefix);
                of.SetAttributeValue("condition", cond + " and " + Prefix + "_ORG");
                result.Add(o);
                result.Add(of);
            }
            return result.ToArray();
        }

        protected object[] getListDefinition(){
            var result = "";
            foreach (var otr in objects){
                var cond = Conditionmap.get(otr.Code);
                if (cond.hasContent()){
                    cond += ",";
                }
                cond += getSelfCondition(otr.Code);
                result += cond;
                result += ":";
                result += otr.Name;
                result += "|\r\n";
            }
            return new object[]{new XText(result)};
        }
    }
}