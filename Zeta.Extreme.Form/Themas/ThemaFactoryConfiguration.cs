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
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.IO;
using Comdiv.Zeta.Report;

namespace Comdiv.Zeta.Web.Themas{
    public class ThemaFactoryConfiguration : IThemaFactoryConfiguration{
        private readonly IList<IThemaConfiguration> _configurations = new List<IThemaConfiguration>();

        #region IThemaFactoryConfiguration Members

        public IList<IThemaConfiguration> Configurations{
            get { return _configurations; }
        }

        public XElement SrcXml { get; set; }

    	public DateTime Version { get; set; }

    	public IThemaFactory Configure(){
            var result = new ThemaFactory();
    		result.Version = this.Version;
            result.SrcXml = SrcXml.ToString();
            foreach (var thcfg in _configurations){
                if (thcfg.Active){
                   // myapp.files.Write("~/tmp/themas/" + thcfg.Code + ".xml",((ThemaConfiguration)thcfg).SrcXml.ToString());
                    var thema = thcfg.Configure();
                  //  thema.Factory = result;
                    result.Themas.Add(thema);
                }
            }
            foreach (var thema in result.Themas){
                if (thema.Group.hasContent()){
                    var grp = result.Themas.FirstOrDefault(x => x.Code == thema.Group);
                    if (null != grp){
                        ((Thema) grp).GroupMembers.Add(thema);
                    }
                }
            }

            foreach (var thema in result.Themas){
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

                foreach (var r in thema.GetAllForms()){
                   
                        var c = r.Configuration;
                        if (c.Sources.Length != 0){
                            foreach (var sourcecode in c.Sources){
                                var lib = result.GetForm(sourcecode);
                                r.Sources.Add(lib);
                            }
                        }
                    
                }
            }

            foreach (Thema thema in result.Themas){
                thema.Configuration = null;
                foreach (ZetaReportDefinition rd in thema.Reports.Values){
                    rd.Configuration = null;
                }
                 foreach (var it in thema.Forms.Values){
                    it.Configuration = null;
                }
                foreach (var document in thema.Forms.Values){
                    document.SourceXmlConfiguration = null;
                }
            }
            var newx = new XElement("root");
            foreach (var x in SrcXml.XPathSelectElements("//*[@preservexml]")){
                newx.Add(x);
            }

            result.SrcXml = newx.ToString();
           
            return result;
        }

        #endregion


    }
}