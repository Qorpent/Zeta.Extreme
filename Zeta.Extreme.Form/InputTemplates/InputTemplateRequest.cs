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
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Useful;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NHibernate;
using Comdiv.Security;

namespace Comdiv.Zeta.Web.InputTemplates{
	/// <summary>
	/// ������ �� �����
	/// </summary>
    public class InputTemplateRequest{
        private readonly IDictionary<string, string> parameters = new Dictionary<string, string>();
        private readonly StorageWrapper<IZetaCell> storage;
        private IZetaDetailObject detail;
        private object detailId;
        private IZetaMainObject @object;
        private object objectId;
        private IInputTemplate template;
        private string templateCode;

        /// <summary>
        /// 
        /// </summary>
        public InputTemplateRequest(){
            storage = myapp.storage.Get<IZetaCell>();
        }

        /// <summary>
        /// ���������, �� �������� �� ����� read-only
        /// </summary>
        public bool IsReadOnly{
            get{
                
                if (myapp.roles.IsInRole("NOBLOCK",false)){
                    return false;
                }
                return Template.GetState(Object, Detail) != "0ISOPEN" || !Template.IsPeriodOpen();
            }
        }

        /// <summary>
        /// ��� �����
        /// </summary>
        public string TemplateCode{
            get { return templateCode; }
            set{
                if (templateCode != value){
                    templateCode = value;
                    Template = null;
                }
            }
        }
        /// <summary>
        /// �������� ���������� �����
        /// </summary>
        /// <param name="full"></param>
        /// <exception cref="Exception"></exception>
        public void CheckFormLive(bool full = false)
        {
            if (this.Task.HaveToTerminate ||  ( full && (null != Task.Context && !this.Task.Context.Response.IsClientConnected)))
            {
                this.Task.Terminate();
                throw new Exception("���������� ����� ���� �������� (noerrc)");
            }
        }

        /// <summary>
        /// ID ������
        /// </summary>
        public object DetailId{
            get { return detailId; }
            set{
                if (detailId != value){
                    detailId = value;
                    Detail = null;
                }
            }
        }

        /// <summary>
        /// ID �������
        /// </summary>
        public object ObjectId{
            get { return objectId; }
            set{
                if (objectId != value){
                    objectId = value;
                    Object = null;
                }
            }
        }
		/// <summary>
		/// ���
		/// </summary>
        public int Year { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// ������ ����
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public IZetaDetailObject Detail{
            get{
                if (null == detail && null != detailId){
                    detail = storage.Load<IZetaDetailObject>(DetailId);
                }
                return detail;
            }
            set { detail = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
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

        /// <summary>
        /// ������
        /// </summary>
        public IInputTemplate Template{
            get{
                if (null == template && TemplateCode.hasContent()){
                    ReloadTemplate();
                }
                return template;
            }
            set { template = value; }
        }

        /// <summary>
        /// ���������
        /// </summary>
        public IDictionary<string, string> Parameters{
            get { return parameters; }
        }

        /// <summary>
        /// ������� �����
        /// </summary>
        /// <returns></returns>
        public InputTemplateRequest Copy(){
            var result = (InputTemplateRequest) MemberwiseClone();
            result.cachedPkg = null;
            return result;
        }
		//NOTE: ��������� ����� ���� �� �����������
/*
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
		*/
        private IPkg cachedPkg = null;
        /// <summary>
        /// ��������� ���������� ������
        /// </summary>
        public LongTask Task { get; set; }

        /// <summary>
        /// ������� ����� �� ���������
        /// </summary>
        /// <returns></returns>
        public IPkg GetDefaultPkg(){
            if (null == cachedPkg) {
                using (var s = new TemporaryTransactionSession()) {
                    var type = storage.Load<IPkgType>("DEFAULTFORMPKG");
                    if (null == type) {
                        type = storage.New<IPkgType>();
                        type.Code = "DEFAULTFORMPKG";
                        type.Name = "����� ��� ����������� ����� � ����� (��� ������)";
                        storage.Save(type);
                        s.CanBeCommited = true;
                    }
                    var code = ToString().toSystemName().Replace("_", "");
                    var result = storage.Load<IPkg>(code);
                    if (null == result) {
                        result = storage.New<IPkg>();
                        result.Code = code;
                        result.Name = "����� ����� �� ��������� ��� " + this;
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

		/// <summary>
		/// ���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns>
		/// ������, �������������� ������� ������.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString(){
            return string.Format("FORM REQUEST T:{0},O:{1},D:{2}, Y:{3},P:{4},D:{5}", TemplateCode, ObjectId, DetailId,
                                 Year, Period, Date.ToString("ddMMyyyyHHmmss"));
        }

        /// <summary>
        /// ���������� ���������� ������
        /// </summary>
        /// <param name="xml"></param>
        /// <exception cref="Exception"></exception>
        public void Save(string xml){
            ReloadTemplate();
            Task.PhaseFinished("prepare template");
            if (IsReadOnly){
                if (!this.Template.IsPeriodOpen())
                {
                    throw new Exception("������ ��� ������������, ���� ����� ������ ����������");    
                }
                throw new Exception("������ ��� ������������, ���� ����� ������ ����������");
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

        /// <summary>
        /// ����������� ������
        /// </summary>
        public void ReloadTemplate(){
            ReloadTemplate(true);
        }

        // static object sync = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keepParameters"></param>
        /// <exception cref="NotSupportedException"></exception>
        public void ReloadTemplate(bool keepParameters){
			throw new NotSupportedException("not ported in extreme");
			//lock (typeof (InputTemplateRequest)){
			//	var oldParameters = template != null ? template.Parameters : null;
			//	if(@object == null && 0!= @objectId.toInt()) {
			//		@object = myapp.storage.Get<IZetaMainObject>().Load(@objectId);
			//	}
			//	template = new InputTemplateRepository().GetTemplate(TemplateCode).PrepareForPeriod(Year, Period, Date,
			//																						@object);
			//		//myapp.storage.Get<IInputTemplate>().Load(TemplateCode).PrepareForPeriod(Year, Period, Date);
			//	foreach (var row in template.Rows){
			//		row.Target = RowCache.get(row.Code);
			//	}

			//	foreach (var parameter in Parameters){
			//		template.Parameters[parameter.Key] = parameter.Value;
			//	}

			//	if (keepParameters && null != oldParameters){
			//		foreach (var parameter in oldParameters){
			//			template.Parameters[parameter.Key] = parameter.Value;
			//		}
			//	}
			//}
        }


        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="object"></param>
        public void SetObject(IZetaMainObject @object){
            ObjectId = @object.Code;
            Object = @object;
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="template"></param>
        public void SetTemplate(IInputTemplate template){
            TemplateCode = template.Code;
            Template = template;
        }
    }
}