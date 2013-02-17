#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : OutputConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Reporting;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	������������ ������� - �� ������ ������ � Extreme �� ��������
	/// </summary>
	public class OutputConfiguration : ItemConfigurationBase<IReportDefinition> {
		/// <summary>
		/// 	����� ����������
		/// </summary>
		public string PrepareViewGenerator { get; set; }

		/// <summary>
		/// 	����� �����������
		/// </summary>
		public string RenderViewGenerator { get; set; }

		/// <summary>
		/// 	��� ����������
		/// </summary>
		public string PrepareView { get; set; }

		/// <summary>
		/// 	��� �����������
		/// </summary>
		public string RenderView { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		public string ForPeriods { get; set; }

		/// <summary>
		/// 	���� ���������� (���������)
		/// </summary>
		public string[] Sources { get; set; }

		/// <summary>
		/// 	���������� ��������
		/// </summary>
		public string PeriodRedirect { get; set; }


		/// <summary>
		/// 	������� �� ����������������
		/// </summary>
		/// <returns> </returns>
		public override IReportDefinition Configure() {
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