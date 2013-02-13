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

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Базовая конфигурация элемента темы
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public abstract class ItemConfigurationBase<T> : IConfiguration<T>{

		/// <summary>
		/// Парамтеры элемента
		/// </summary>
        public readonly IList<TypedParameter> Parameters = new List<TypedParameter>();
		/// <summary>
		/// Предупреждения по элементу
		/// </summary>
        public readonly IList<string> Warrnings = new List<string>();
		/// <summary>
		/// Исходный XML шаблока
		/// </summary>
        public XElement TemplateXml;
		/// <summary>
		/// Признак наличия ошибок
		/// </summary>
        private bool _isError;
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
        public ItemConfigurationBase(){
            Active = true;
        }
		/// <summary>
		/// Признак активности элемента
		/// </summary>
        public bool Active { get; set; }
		/// <summary>
		/// Значение элемента
		/// </summary>
        public string Value { get; set; }

		/// <summary>
		/// Ссылка на конфигурацию - контейнер
		/// </summary>
        public IThemaConfiguration Thema { get; set; }
		/// <summary>
		/// Команда на конфигурирование
		/// </summary>
		/// <returns></returns>
        public abstract T Configure();
		/// <summary>
		/// Роль доступа
		/// </summary>
        public string Role { get; set; }
		/// <summary>
		/// Тип элемента
		/// </summary>
        public string Type { get; set; }
		/// <summary>
		/// Код элемента
		/// </summary>
        public string Code { get; set; }
		/// <summary>
		/// URL  элемента
		/// </summary>
        public string Url { get; set; }
		/// <summary>
		/// Название элемента
		/// </summary>
        public string Name { get; set; }
		/// <summary>
		/// Имя шаблона ??
		/// </summary>
        public string Template { get; set; }
		/// <summary>
		/// Признак наличия ошибки
		/// </summary>
        public bool IsError{
            get { return _isError || getErrorInternal(); }
            set { _isError = value; }
        }

		/// <summary>
		/// Перегружаемый метод для поиска ошибок
		/// </summary>
		/// <returns></returns>
        protected virtual bool getErrorInternal(){
            return false;
        }
    }
}