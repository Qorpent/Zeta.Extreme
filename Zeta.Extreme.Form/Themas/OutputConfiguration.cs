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
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Reporting;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// ������������ ������� - �� ������ ������ � Extreme �� ��������
	/// </summary>
    public class OutputConfiguration : ItemConfigurationBase<IReportDefinition>{
       /// <summary>
       /// ����� ����������
       /// </summary>
        public string PrepareViewGenerator { get; set; }
		/// <summary>
		/// ����� �����������
		/// </summary>
        public string RenderViewGenerator { get; set; }
		/// <summary>
		/// ��� ����������
		/// </summary>
        public string PrepareView { get; set; }
		/// <summary>
		/// ��� �����������
		/// </summary>
        public string RenderView { get; set; }
		/// <summary>
		/// ������� �� �������
		/// </summary>
        public string ForPeriods { get; set; }
		/// <summary>
		/// ���� ���������� (���������)
		/// </summary>
        public string[] Sources { get; set; }
		/// <summary>
		/// ���������� ��������
		/// </summary>
        public string PeriodRedirect { get; set; }
		/// <summary>
		/// ������� ������������� ������ (�����!!)
		/// </summary>
        public bool UseFormMatrix { get; set; }
		/// <summary>
		/// ���������� ����� ������� (����� !!)
		/// </summary>
        public string MatrixExRows { get; set; }
		/// <summary>
		/// ����� ������� (�����!!)
		/// </summary>
        public string MatrixExSqlHint { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        protected override bool getErrorInternal(){
            //if (Template == "empty.out"){
            //    var isvalid = TemplateXml.Elements("isvalid");
            //    if (isvalid.no()){
            //        var cols = TemplateXml.Elements("col");
            //        if (cols.no()){
            //            return true;
            //        }
            //    }
            //}
            return base.getErrorInternal();
        }

		/// <summary>
		/// ������� �� ����������������
		/// </summary>
		/// <returns></returns>
		public override IReportDefinition Configure(){
			throw new NotImplementedException("���� ��� Zeta.Extreme ���������� ���������� �������");
            /*var def = new ZetaReportDefinition();
            def.ReadFromXml(TemplateXml);
            def.bindfrom(this, true);
            def.Configuration = this;

            foreach (var parameter in Parameters){
                var p = new Parameter{
                                         Static = true,
                                         Code = parameter.Name,
                                         Target = parameter.Name,
                                         DefaultValue = parameter.GetValue()
                                     };
                if (parameter.Mode != "static"){
                    p.Static = false;
                }
                p.settype(parameter.Type);
                p.Prepare();
                def.TemplateParameters.Add(p);
            }

            var factory = myapp.Container.get<IReportViewFactory>();
            if (null != factory){
                factory.Process(def);
            }
            if (def.Name.noContent()){
                def.Name = Thema.Name;
            }
            return def;*/
        }
    }
}