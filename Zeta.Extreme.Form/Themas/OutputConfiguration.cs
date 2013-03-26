#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/OutputConfiguration.cs
#endregion
using System;
using Zeta.Extreme.BizProcess.Reports;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Конфигурация отчетов - НА ДАННЫЙ МОМЕНТ С Extreme не РАБОТАЕТ
	/// </summary>
	public class OutputConfiguration : ItemConfigurationBase<IReportDefinition> {
		/// <summary>
		/// 	Класс генератора
		/// </summary>
		public string PrepareViewGenerator { get; set; }

		/// <summary>
		/// 	Класс отрисовщика
		/// </summary>
		public string RenderViewGenerator { get; set; }

		/// <summary>
		/// 	Вид генератора
		/// </summary>
		public string PrepareView { get; set; }

		/// <summary>
		/// 	Вид отрисовщика
		/// </summary>
		public string RenderView { get; set; }

		/// <summary>
		/// 	Условия на периоды
		/// </summary>
		public string ForPeriods { get; set; }

		/// <summary>
		/// 	Коды источников (библиотек)
		/// </summary>
		public string[] Sources { get; set; }

		/// <summary>
		/// 	Редиректор периодов
		/// </summary>
		public string PeriodRedirect { get; set; }


		/// <summary>
		/// 	Команда на конфигурирование
		/// </summary>
		/// <returns> </returns>
		public override IReportDefinition Configure() {
			throw new NotImplementedException("пока для Zeta.Extreme отсутствут реализация отчетов");
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