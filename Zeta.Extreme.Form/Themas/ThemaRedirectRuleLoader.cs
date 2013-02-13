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
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Zeta.Data.Rules;

namespace Comdiv.Zeta.Web.Themas{
    public class ThemaRedirectRuleLoader : IRedirectRuleLoader{
        #region IRedirectRuleLoader Members

        public IList<IRedirectRule> Load(){
            lock (this){
                var fp = myapp.ioc.get<IThemaFactoryProvider>();
                var f = fp.Get();
                var result = new List<IRedirectRule>();
                var x = XElement.Parse(f.SrcXml);
                var rules = x.XPathSelectElements("redirect");
                foreach (var rule in rules){
                    var r = new RedirectRule{Mask = new RedirectRuleSet(), Result = new RedirectRuleSet()};
                    var te = rule.Element("test");
                    var re = rule.Element("make");
                    r.Mask.Row = te.attr("row", "");
                    r.Mask.Column = te.attr("column", "");
                    r.Mask.Period = te.attr("period", 0);
                    r.Result.Row = re.attr("row", "");
                    r.Result.Column = re.attr("column", "");
                    r.Result.Period = re.attr("period", 0);
                	r.Result.Year = re.attr("year", 0);
                    result.Add(r);
                }
                return result;
            }
        }

        #endregion
    }
}