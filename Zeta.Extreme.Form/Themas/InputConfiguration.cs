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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Comdiv.Extensions;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Описатель конфигурации формы ввода
	/// </summary>
    public class InputConfiguration : ItemConfigurationBase<IInputTemplate>{
		/// <summary>
		/// Признак зависимости от объектов
		/// </summary>
		public bool IsObjectDependent { get; set; }
		/// <summary>
		/// Коды источников
		/// </summary>
        public string[] Sources { get; set; }
		/// <summary>
		/// Параметр блокировки
		/// </summary>
        public string Lock { get; set; }

		/// <summary>
		/// Параметр привязки к периодам
		/// </summary>
        public string ForPeriods { get; set; }
		/// <summary>
		/// Параметр настройки автозаполнения
		/// </summary>
        public string AutoFill { get; set; }
		/// <summary>
		/// Класс расписания заполнения
		/// </summary>
        public string ScheduleClass { get; set; }
		/// <summary>
		/// Признак требования групп предприятия
		/// </summary>
        public string ForGroup { get; set; }
		/// <summary>
		/// Фиксированные строки
		/// </summary>
        public string FixRows { get; set; }
		/// <summary>
		/// Требование выполнения скрипта перед загрузкой
		/// </summary>
        public bool NeedPreloadScript { get; set; }
		/// <summary>
		/// Статус формы по умолчанию
		/// </summary>
        public string DefaultState { get; set; }
		/// <summary>
		/// Смещение расписания
		/// </summary>
        public int ScheduleDelta { get; set; }
		/// <summary>
		/// Вид таблицы
		/// </summary>
        public string TableView { get; set; }
		/// <summary>
		/// Избранные строки по деталям (видимо для m140)
		/// </summary>
        public bool DetailFavorite { get; set; }
		/// <summary>
		/// Показывать колонку с единицами
		/// </summary>
        public bool ShowMeasureColumn { get; set; }
		/// <summary>
		/// Периоды требования файлов
		/// </summary>
        public string NeedFilesPeriods { get; set; }
		/// <summary>
		/// Признак требования файловв
		/// </summary>
        public string NeedFiles { get; set; }
		/// <summary>
		/// Дополнительные документы
		/// </summary>
        public string AdvDocs { get; set; }

		/// <summary>
		/// Фиксированный объект
		/// </summary>
        public string FixedObj { get; set; }
		/// <summary>
		/// Ссылка на форму BIZTRAN
		/// </summary>
		public string Biztran { get; set; }
		/// <summary>
		/// ФОрма для деталей
		/// </summary>
        public bool InputForDetail { get; set; }
		/// <summary>
		/// XML - определения колонок
		/// </summary>
        public XElement[] ColumnDefinitions { get; set; }
		/// <summary>
		/// XML - определения строк
		/// </summary>
        public XElement[] RowDefinitions { get; set; }

		/// <summary>
		/// Признак использования только избранных строк
		/// </summary>
        public bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// Перевод периодов
		/// </summary>
        public string PeriodRedirect { get; set; }
		/// <summary>
		/// Роль на подписание
		/// </summary>
        public string UnderwriteRole { get; set; }
		/// <summary>
		/// Корневая строка
		/// </summary>
        public string RootCode { get; set; }
		/// <summary>
		/// SQL оптимизация
		/// </summary>
        public string SqlOptimization { get; set; }


		/// <summary>
		/// Команда на конфигурирование
		/// </summary>
		/// <returns></returns>
		public override IInputTemplate Configure(){
            var txs = new InputTemplateXmlSerializer();
            var template = txs.Read(TemplateXml).First();
            template.UnderwriteCode = Lock;
            template.Code = Code;
            template.ForGroup = ForGroup;
            template.Role = Role;
            template.SqlOptimization = SqlOptimization;
            template.PeriodRedirect = PeriodRedirect;
            template.Name = Name;
            template.ForPeriods = ForPeriods.split().Select(x => x.toInt()).ToArray();
            template.AutoFillDescription = AutoFill;
            template.UnderwriteRole = UnderwriteRole;
            template.ScheduleDelta = ScheduleDelta;
        	template.Biztran = this.Biztran;
            template.ScheduleClass = ScheduleClass;
            template.FixedObjectCode = FixedObj;
            template.DefaultState = DefaultState;
            template.DetailFavorite = DetailFavorite;
        	template.IgnorePeriodState = IgnorePeriodState;
        	template.IsObjectDependent = this.IsObjectDependent;
           // template.TableView = TableView;
            template.NeedFiles = NeedFiles;
            template.NeedPreloadScript = this.NeedPreloadScript;
            template.DocumentRoot = this.DocumentRoot;
            template.NeedFilesPeriods = NeedFilesPeriods;
            template.UseQuickUpdate = UseQuickUpdate;
            template.AdvDocs = AdvDocs;
            if (FixRows.hasContent()){
                foreach (var s in FixRows.split()){
                    template.FixedRowCodes.Add(s);
                }
            }
            template.FavoriteRowsOnly = FavoriteRowsOnly;
            template.InputForDetail = InputForDetail;
            template.ShowMeasureColumn = ShowMeasureColumn;

            template.Configuration = this;

            if (ColumnDefinitions.yes()){
                var serializer = new InputTemplateXmlSerializer();
                serializer.BindColumns(template, ColumnDefinitions);
            }


            if (RowDefinitions.yes()){
                var serializer = new InputTemplateXmlSerializer();
                serializer.BindRows(template, RowDefinitions);
            }

            if (template.Form == null){
                var rowcodeparam = RootCode;
                if (rowcodeparam.noContent()){
                    rowcodeparam = Thema.ResolveParameter("rootrow").GetValue() as string;
                }

                if (rowcodeparam.hasContent()){
                    template.Form = new RowDescriptor{Code = rowcodeparam};
                }
            }

            foreach (var document in Documents)
            {
                template.Documents[document.Key] = document.Value;
            }

            foreach (var p in Parameters){
                template.Parameters[p.Name] = p.Value;
            }

            return template;
        }

        private IDictionary<string, string> _documents = new Dictionary<string, string>();
        /// <summary>
        /// Документы
        /// </summary>
        public IDictionary<string, string> Documents
        {
            get { return _documents; }
            set { _documents = value; }
        }

        /// <summary>
        /// Корневая строка
        /// </summary>
        public string DocumentRoot { get; set; }

		/// <summary>
        /// Использовать быстрое обновление
        /// </summary>
        public bool UseQuickUpdate { get; set; }

    	/// <summary>
    	/// Игнорировать статус периодов
    	/// </summary>
    	public bool IgnorePeriodState { get; set; }
    }
}