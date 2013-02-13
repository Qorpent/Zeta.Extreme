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
using System.Security.Principal;
using System.Xml.Linq;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
    public interface IThemaFactory:IDisposable {
        IThema Get(string code);
        IDictionary<string, object> Cache { get; }
        string SrcXml { get; set; }
    	DateTime Version { get; set; }
    	IEnumerable<IThema> GetAll();
        IReportDefinition GetReport(string code);
        IEnumerable<IThema> GetForUser();
        IEnumerable<IThema> GetForUser(IPrincipal usr);
        IInputTemplate GetForm(string code,bool throwerror = false);
        void CleanUser(string usrname);
    }
}