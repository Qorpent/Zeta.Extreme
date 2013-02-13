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
using Comdiv.Application;
using Comdiv.Dbfs;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Useful;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using NHibernate;
using Comdiv.Security;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateRequest{
        private readonly IDictionary<string, string> parameters = new Dictionary<string, string>();
        private readonly StorageWrapper<IZetaCell> storage;
        private IZetaDetailObject detail;
        private object detailId;
        private IZetaMainObject @object;
        private object objectId;
        private IInputTemplate template;
        private string templateCode;

        public InputTemplateRequest(){
            storage = myapp.storage.Get<IZetaCell>();
        }

        public bool IsReadOnly{
            get{
                
                if (myapp.roles.IsInRole("NOBLOCK",false)){
                    return false;
                }
                return Template.GetState(Object, Detail) != "0ISOPEN" || !Template.IsPeriodOpen();
            }
        }

        public string TemplateCode{
            get { return templateCode; }
            set{
                if (templateCode != value){
                    templateCode = value;
                    Template = null;
                }
            }
        }
        public void CheckFormLive(bool full = false)
        {
            if (this.Task.HaveToTerminate ||  ( full && (null != this.Task.Context && !this.Task.Context.Response.IsClientConnected)))
            {
                this.Task.Terminate();
                throw new Exception("Выполнение формы было отменено (noerrc)");
            }
        }

        public object DetailId{
            get { return detailId; }
            set{
                if (detailId != value){
                    detailId = value;
                    Detail = null;
                }
            }
        }

        public object ObjectId{
            get { return objectId; }
            set{
                if (objectId != value){
                    objectId = value;
                    Object = null;
                }
            }
        }

        public int Year { get; set; }
        public int Period { get; set; }
        public DateTime Date { get; set; }

        public IZetaDetailObject Detail{
            get{
                if (null == detail && null != detailId){
                    detail = storage.Load<IZetaDetailObject>(DetailId);
                }
                return detail;
            }
            set { detail = value; }
        }

        public IZetaMainObject Object{
            get{
                if (null == @object && null != ObjectId){

                    @object =Template.FixedObject ?? storage.Load<IZetaMainObject>(ObjectId);
                    @objectId = @object.Id;
                }
                return @object;
            }
            set { @object = value; }
        }

        public IInputTemplate Template{
            get{
                if (null == template && TemplateCode.hasContent()){
                    ReloadTemplate();
                }
                return template;
            }
            set { template = value; }
        }

        public IDictionary<string, string> Parameters{
            get { return parameters; }
        }

        public InputTemplateRequest Copy(){
            var result = (InputTemplateRequest) MemberwiseClone();
            result.cachedPkg = null;
            return result;
        }

        public IList<IFile> GetAttachedFiles(){
           // var pkg = GetDefaultPkg();

            return myapp.ioc.get<IDbfsRepository>().SearchBySpecialProc("usm.get_form_attachments",new Dictionary<string, object>
                                                               {
                                                                   {"form", Template.Code},
                                                                   {"year",this.Year.ToString()},
                                                                   {"period",this.Period.ToString()},
                                                                   {"obj",this.ObjectId.ToString()},
                                                               });

            return myapp.ioc.get<IDbfsRepository>().Search(new Dictionary<string, string>
                                                               {
                                                                   {"form", Template.Code},
                                                                   {"year",this.Year.ToString()},
                                                                   {"period",this.Period.ToString()},
                                                                   {"obj",this.ObjectId.ToString()},
                                                               });
        }

        private IPkg cachedPkg = null;
        public LongTask Task { get; set; }

        public IPkg GetDefaultPkg(){
            if (null == cachedPkg) {
                using (var s = new TemporaryTransactionSession()) {
                    var type = storage.Load<IPkgType>("DEFAULTFORMPKG");
                    if (null == type) {
                        type = storage.New<IPkgType>();
                        type.Code = "DEFAULTFORMPKG";
                        type.Name = "Пакет для заполняемой формы в целом (вне сеанса)";
                        storage.Save(type);
                        s.CanBeCommited = true;
                    }
                    var code = ToString().toSystemName().Replace("_", "");
                    var result = storage.Load<IPkg>(code);
                    if (null == result) {
                        result = storage.New<IPkg>();
                        result.Code = code;
                        result.Name = "Пакет формы по умолчанию для " + this;
                        result.Usr = myapp.usrName;
                        result.Type = type;
                        result.CreateTime = DateTime.Now;
                        result.State = PkgState.None;
                        result.Date = DateTime.Now;
                        if (Period != 0) {
                            result.Date = Periods.Get(Period).EndDate.accomodateToYear(Year);
                        }
                        if (Date.Year > 1901) {
                            result.Date = Date;
                        }
                        result.Object = Object;
                        result.DetailObject = Detail;
                        storage.Save(result);
                        s.CanBeCommited = true;
                    }
                    cachedPkg = result;
                }
                
            }
            return cachedPkg;
        }

        public override string ToString(){
            return string.Format("FORM REQUEST T:{0},O:{1},D:{2}, Y:{3},P:{4},D:{5}", TemplateCode, ObjectId, DetailId,
                                 Year, Period, Date.ToString("ddMMyyyyHHmmss"));
        }

        public void Save(string xml){
            ReloadTemplate();
            Task.PhaseFinished("prepare template");
            if (IsReadOnly){
                if (!this.Template.IsPeriodOpen())
                {
                    throw new Exception("Период был заблокирован, ввод новых данных невозможен");    
                }
                throw new Exception("Шаблон был заблокирован, ввод новых данных невозможен");
            }

            var reader = new CellSerializer();
            var saver = new CellSaver{TemplateRequest = this};
            if (Template.CustomSave.hasContent()) {
                ICustomFormSaver formsaver = myapp.ioc.get(Template.CustomSave) as ICustomFormSaver;
                formsaver.Save(this, xml);

            }
            else{
                using (DataSaveLock.Get()) {
                    using (var handler = new TemporaryTransactionSession()) {
                        handler.Session.FlushMode = FlushMode.Commit;

                        var cells = reader.ReadXml(xml);
                        Task.PhaseFinished("cells readed");
                        saver.Save(cells);
                        Task.PhaseFinished("save executed");
                        handler.Commit();
                    }
                    Task.PhaseFinished("transaction commited");
                }

                using (var c = myapp.ioc.getConnection()){
                        c.ExecuteNonQuery(
                            @"exec usm.after_save_trigger 
                            @form=@form,
                            @obj=@obj,
                            @year=@year,
                            @period=@period,
                            @usr=@usr",
                            new Dictionary<string, object>{
                                                              {"@form", TemplateCode},
                                                              {"@obj", ObjectId},
                                                              {"@year", Year},
                                                              {"@period", Period},
                                                              {"@usr", myapp.usrName.ToLower()}
                                                          },900);
                    }
              Task.PhaseFinished("after save trigger called");
                
            }
        }

        public void ReloadTemplate(){
            ReloadTemplate(true);
        }

        // static object sync = new object();
        public void ReloadTemplate(bool keepParameters){
            lock (typeof (InputTemplateRequest)){
                var oldParameters = template != null ? template.Parameters : null;
                if(@object == null && 0!= @objectId.toInt()) {
                    @object = myapp.storage.Get<IZetaMainObject>().Load(@objectId);
                }
                template = new InputTemplateRepository().GetTemplate(TemplateCode).PrepareForPeriod(Year, Period, Date,
                                                                                                    @object);
                    //myapp.storage.Get<IInputTemplate>().Load(TemplateCode).PrepareForPeriod(Year, Period, Date);
                foreach (var row in template.Rows){
                    row.Target = RowCache.get(row.Code);
                }

                foreach (var parameter in Parameters){
                    template.Parameters[parameter.Key] = parameter.Value;
                }

                if (keepParameters && null != oldParameters){
                    foreach (var parameter in oldParameters){
                        template.Parameters[parameter.Key] = parameter.Value;
                    }
                }
            }
        }


        public void SetObject(IZetaMainObject @object){
            ObjectId = @object.Code;
            Object = @object;
        }

        public void SetTemplate(IInputTemplate template){
            TemplateCode = template.Code;
            Template = template;
        }
    }
}