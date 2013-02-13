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
	/// <summary>
	/// Генератор списка HTML для стартера
	/// </summary>
    public class HtmlListDefinition{
        /// <summary>
        /// Элементы для генерации
        /// </summary>
        public readonly IList<HtmlListDefinition> Items = new List<HtmlListDefinition>();
        private string _text;
        /// <summary>
        /// Ид атрибут
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Класс (?)
        /// </summary>
        public string Class { get; set; }
		/// <summary>
		/// Текст элекмента
		/// </summary>
        public string Text{
            get { return _text.hasContent() ? _text : Value; }
            set { _text = value; }
        }
		/// <summary>
		/// Признак группы
		/// </summary>
        public bool IsGroup{
            get { return Items.Count > 0; }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override string ToString(){
            return Render();
        }

        /// <summary>
        /// Добавляет элемент к списку
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public HtmlListDefinition Add(string value, string text){
            var result = new HtmlListDefinition{Value = value, Text = text};
            Items.Add(result);
            return result;
        }

        /// <summary>
        /// Отрисовывает целевые объекты на HTML
        /// </summary>
        /// <returns></returns>
        public string Render(){
            if (Items.Count == 0){
                return "<span class='empty'>Целевые объекты отсутствуют</span>";
            }
            var result = new StringBuilder();
            Render(result, true);
            return result.ToString();
        }
		/// <summary>
		/// Отрисовка элементов
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="root"></param>
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