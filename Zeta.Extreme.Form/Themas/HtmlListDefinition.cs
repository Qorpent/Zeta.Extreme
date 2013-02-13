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
using System.Text;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web{
    public class HtmlListDefinition{
        public readonly IList<HtmlListDefinition> Items = new List<HtmlListDefinition>();
        private string _text;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Class { get; set; }

        public string Text{
            get { return _text.hasContent() ? _text : Value; }
            set { _text = value; }
        }

        public bool IsGroup{
            get { return Items.Count > 0; }
        }

        public override string ToString(){
            return Render();
        }

        public HtmlListDefinition Add(string value, string text){
            var result = new HtmlListDefinition{Value = value, Text = text};
            Items.Add(result);
            return result;
        }

        public string Render(){
            if (Items.Count == 0){
                return "<span class='empty'>Целевые объекты отсутствуют</span>";
            }
            var result = new StringBuilder();
            Render(result, true);
            return result.ToString();
        }

        protected void Render(StringBuilder builder, bool root){
            if (root){
                builder.AppendFormat("<select id='{0}' name='{1}' class='{2}'>", Id, Name, Class);
            }
            if (!root && IsGroup){
                builder.AppendFormat("<optgroup label='{0}'>", Text);
            }

            if (IsGroup){
                foreach (var item in Items){
                    item.Render(builder, false);
                }
            }
            else{
                builder.AppendFormat("<option value='{0}'>{1}</option>", Value, Text);
            }
            if (root){
                builder.Append("</select>");
            }
            if (!root && IsGroup){
                builder.Append("</optgroup>");
            }
        }
    }
}