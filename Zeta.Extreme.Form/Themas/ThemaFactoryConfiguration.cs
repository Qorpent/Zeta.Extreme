#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ThemaFactoryConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Extensions;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Конфигурация фабрики тем
	/// </summary>
	public class ThemaFactoryConfiguration : IThemaFactoryConfiguration {
		/// <summary>
		/// 	Версия
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// 	Список конфигурации
		/// </summary>
		public IList<IThemaConfiguration> Configurations {
			get { return _configurations; }
		}

		/// <summary>
		/// 	Исходный XML
		/// </summary>
		public XElement[] SrcXml { get; set; }

		/// <summary>
		/// 	Вызов метода конфигурации фабрики
		/// </summary>
		/// <returns> </returns>
		public IThemaFactory Configure() {
			var result = new ThemaFactory();
			result.Version = Version;
			result.SrcXml = SrcXml.ToString();
			foreach (var thcfg in _configurations) {
				if (thcfg.Active) {
					// myapp.files.Write("~/tmp/themas/" + thcfg.Code + ".xml",((ThemaConfiguration)thcfg).SrcXml.ToString());
					var thema = thcfg.Configure();
					//  thema.Factory = result;
					result.Themas.Add(thema);
				}
			}
			foreach (var thema in result.Themas) {
				if (thema.Group.hasContent()) {
					var grp = result.Themas.FirstOrDefault(x => x.Code == thema.Group);
					if (null != grp) {
						((Thema) grp).GroupMembers.Add(thema);
					}
				}
			}

			foreach (var thema in result.Themas) {
				//NOTE: пока с отчетами не работаем
				/*
                foreach (var r in thema.GetAllReports()){
                    if (r is ZetaReportDefinitionBase){
                        var zr = (ZetaReportDefinition) r;
                        var c = zr.Configuration;
                        if (c.Sources.Length != 0){
                            foreach (var sourcecode in c.Sources){
                                var lib = result.GetReport(sourcecode);
                                zr.Sources.Add(lib);
                            }
                        }
                    }
                }
				*/
				foreach (var r in thema.GetAllForms()) {
					var c = ((InputTemplate)r).Configuration;
					if (c.Sources.Length != 0) {
						foreach (var sourcecode in c.Sources) {
							var lib = result.GetForm(sourcecode);
							r.Sources.Add(lib);
						}
					}
				}
			}

			foreach (Thema thema in result.Themas) {
				thema.Configuration = null;
				//NOTE: пока с отчетами не работаем
				/*
                foreach (ZetaReportDefinition rd in thema.Reports.Values){
                    rd.Configuration = null;
                }
				 */
				foreach (var it in thema.Forms.Values) {
					((InputTemplate)it).Configuration = null;
				}
				foreach (var document in thema.Forms.Values) {
					document.SourceXmlConfiguration = null;
				}
			}
			var newx = new XElement("root");
			foreach (var e in SrcXml) {
				foreach (var x in e.XPathSelectElements("//*[@preservexml]")) {
					newx.Add(x);
				}
			}


			result.SrcXml = newx.ToString();

			return result;
		}

		private readonly IList<IThemaConfiguration> _configurations = new List<IThemaConfiguration>();
	}
}