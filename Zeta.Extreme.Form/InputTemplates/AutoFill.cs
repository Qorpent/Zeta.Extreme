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
using Comdiv.Extensions;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates{
	/// <summary>
	/// Класс обслуживания автозаполнения
	/// </summary>
    public class AutoFill{
		/// <summary>
		/// Создает автозаполнитель
		/// </summary>
        public AutoFill(){
            Periods = new List<int>();
        }

        /// <summary>
        /// ВЫполняет автозаполнение
        /// </summary>
        /// <param name="description"></param>
        public AutoFill(string description) : this(){
            if (description.hasContent()){
                var def = description.split(false, true, '|');
                if (1 < def.Count){
                    Periods.addRange(def[0].split().Select(s => s.toInt()));
                }
                CallData = def[def.Count - 1];
                if (CallData.hasContent()){
                    AutoFillType = AutoFillType.Custom;
                    if (CallData.StartsWith("sql:")){
                        AutoFillType = AutoFillType.Sql;
                        CallData = CallData.Substring(4);
                    }
                }
            }
        }

        /// <summary>
        /// Автозаполнение по шаблону
        /// </summary>
        /// <param name="template"></param>
        public AutoFill(IInputTemplate template) : this(template.AutoFillDescription){
            Template = template;
        }

        /// <summary>
        /// Целевая форма
        /// </summary>
        public IInputTemplate Template { get; set; }
        /// <summary>
        /// Периоды
        /// </summary>
        public IList<int> Periods { get; set; }
        /// <summary>
        /// Тип автозаполнения
        /// </summary>
        public AutoFillType AutoFillType { get; set; }
        /// <summary>
        /// Данные для ячеек ???
        /// </summary>
        public string CallData { get; set; }

        /// <summary>
        /// Проверка выполнитмости
        /// </summary>
        public bool IsExecutable{
            get{
                if (AutoFillType.None == AutoFillType){
                    return false;
                }
                if (CallData.noContent()){
                    return false;
                }
                if (0 != Periods.Count){
                    if (!Periods.Contains(Template.Period)){
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Запуск автозаполнения
        /// </summary>
        /// <param name="obj"></param>
        public void Perform(IZetaMainObject obj){
            if (IsExecutable){
                var executor = getExecutor();
                executor.Execute(this, obj);
            }
        }

        private IAutoFillExecutor getExecutor(){
            if (AutoFillType.Sql == AutoFillType){
                return new SqlAutoFiller();
            }
            else{
                return CallData.toType().create<IAutoFillExecutor>();
            }
        }
    }
}