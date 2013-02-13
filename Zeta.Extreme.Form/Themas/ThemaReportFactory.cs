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
using Comdiv.MVC.Report;
using Comdiv.Reporting;

namespace Comdiv.Zeta.Web.Themas{
    public class ThemaReportFactory : ReportFactory{
        public IThemaFactoryProvider Provider { get; set; }

        public override IReportDefinition LoadDefinition(string code, Type definitionType){
            lock (sync){
                //first try resolve report through themas
                var thcode = code;
                if (code.EndsWith(".out")){
                    code = code.Substring(0, code.Length - 4);
                }
                var result = Provider.Get().GetReport(thcode);
                if (null != result){
                    return result.Clone();
                }
                result = Provider.Get().GetReport(thcode.Replace(".out","Aa.out"));
                if (null != result)
                {
                    return result.Clone();
                }
                return base.LoadDefinition(code, definitionType);
            }
        }
    }
}