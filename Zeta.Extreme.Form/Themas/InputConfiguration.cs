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
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
    public class InputConfiguration : ItemConfigurationBase<IInputTemplate>{
		public bool IsObjectDependent { get; set; }
        public string[] Sources { get; set; }
        public string Lock { get; set; }
        public string ForPeriods { get; set; }
        public string AutoFill { get; set; }
        public string ScheduleClass { get; set; }
        public string ForGroup { get; set; }
        public string FixRows { get; set; }
        public bool NeedPreloadScript { get; set; }
        public string DefaultState { get; set; }
        public int ScheduleDelta { get; set; }
        public string TableView { get; set; }
        public bool DetailFavorite { get; set; }
        public bool ShowMeasureColumn { get; set; }
        public string NeedFilesPeriods { get; set; }
        public string NeedFiles { get; set; }
        public string AdvDocs { get; set; }
        public string MatrixExRows { get; set; }
        public string FixedObj { get; set; }
		public string Biztran { get; set; }
        public bool InputForDetail { get; set; }
        public XElement[] ColumnDefinitions { get; set; }
        public XElement[] RowDefinitions { get; set; }
        public bool UseFormMatrix { get; set; }
        public bool FavoriteRowsOnly { get; set; }

        public string PeriodRedirect { get; set; }

        public string UnderwriteRole { get; set; }

        public string RootCode { get; set; }

        public string SqlOptimization { get; set; }

        protected override bool getErrorInternal(){
            //if (Template == "empty.in"){
            //    if (ColumnDefinitions.no()){
            //        return true;
            //    }
            //}
            return base.getErrorInternal();
        }

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
            template.UseFormMatrix = this.UseFormMatrix;
            template.MatrixExRows = this.MatrixExRows;
            template.MatrixExSqlHint = this.MatrixExSqlHint;
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
            template.TableView = TableView;
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
        public IDictionary<string, string> Documents
        {
            get { return _documents; }
            set { _documents = value; }
        }

        public string DocumentRoot { get; set; }

        public string MatrixExSqlHint { get; set; }

        public bool UseQuickUpdate { get; set; }

    	public bool IgnorePeriodState { get; set; }
    }
}