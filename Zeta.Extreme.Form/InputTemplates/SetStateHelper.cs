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
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class SetStateHelper{
        public void Set(string code, IZetaMainObject obj, IZetaDetailObject detail, int year, int period, string state){
            var h = new InputRowHelper();
            var c = h.Get(obj, detail, RowCache.get(code), new ColumnDesc{
                                                                             Year = year,
                                                                             Period = period,
                                                                             DirectDate = DateExtensions.Begin,
                                                                             Target =
                                                                                 ColumnCache.get("0CONSTSTR")
                                                                         });
            c.RowData.StringValue = state;
            c.Usr = myapp.usrName;
            myapp.storage.Get<IZetaCell>().Save(c);
        }
    }
}