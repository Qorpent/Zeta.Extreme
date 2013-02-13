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
using System.Xml.Linq;
using Comdiv.Extensions;
using Comdiv.Xml;

namespace Comdiv.Zeta.Web.Themas{
    /// <summary>
    /// Абстрактный генератор XML	
    /// </summary>
    public abstract class XmlGeneratorBase : IXmlGenerator{
        /// <summary>
        /// Внутренний генератор ИД
        /// </summary>
        public static int id;

		/// <summary>
		/// Словарь условий
		/// </summary>
        protected IDictionary<string, string> Conditionmap { get; set; }

		/// <summary>
		/// Элемент для включения
		/// </summary>
        protected XElement Include { get; set; }

        /// <summary>
        /// Фильтрующее условие
        /// </summary>
        public string FilterCondition { get; set; }

        #region IXmlGenerator Members

        public object[] Generate(XElement call){
            prepare(call);

            return internalGenerate();
        }

        #endregion

        protected virtual string getCondition(string code){
            var self = getSelfCondition(code);

            if (!Conditionmap.ContainsKey(code)){
                return self;
            }
            var mask = Conditionmap[code];
            if (mask.Contains("$SELF")){
                return mask.Replace("$SELF", self);
            }
            return mask + "," + self;
        }

        protected virtual string getSelfCondition(string code){
            return "";
        }

        protected virtual void prepare(XElement call){
            var x = call.Element("filter");
            if (null != x){
                FilterCondition = x.attr("id", "");
            }
            Include = call.Element("include");
            Conditionmap = new Dictionary<string, string>();
            processIncludes();
            processConditions(call);
        }

        protected abstract object[] internalGenerate();

        private void processConditions(XElement call){
            foreach (var element in call.Elements("condition")){
                var cond = element.attr("id");

                var list = element.Value.split();
                if (element.Value.noContent()){
                    list = getTargetCodes();
                }

                foreach (var code in list){
                    var existed = Conditionmap.get(code, "");
                    if (existed.hasContent()){
                        existed += ",";
                    }
                    existed += cond;
                    Conditionmap[code] = existed;
                }
            }
        }


        protected virtual IList<string> getTargetCodes(){
            return new string[]{};
        }

        protected virtual void processIncludes(){
        }
    }
}