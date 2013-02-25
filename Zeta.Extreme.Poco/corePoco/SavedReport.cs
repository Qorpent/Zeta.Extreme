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
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Reporting;
using Comdiv.Security;

namespace Comdiv.Zeta.Model{
    public class SavedReport : ISavedReport{
        #region ISavedReport Members
        [Map]
        public virtual string Tag { get; set; }
        [Map]
        public virtual int Id { get; set; }
        [Map]
        public virtual string Name { get; set; }
        [Map]
        public virtual string Code { get; set; }
        [Map]
        public virtual string Comment { get; set; }
        [Map]
        public virtual DateTime Version { get; set; }
        [Map]
        public virtual string Usr { get; set; }
        [Map]
        public virtual bool Shared { get; set; }
        [Map]
        public virtual string ReportCode { get; set; }
        [Map]
        public virtual string Role { get; set; }
        public virtual IList<ISavedReportParameter> Parameters { get; set; }
        public virtual bool Authorize(IPrincipal usr) {
            if (myapp.roles.IsAdmin()) return true;
            if (this.Usr.ToUpper() == usr.Identity.Name.ToUpper()) return true;
            if (!this.Shared) return false;
            if(this.Role.noContent()) return true;
            var roles = this.Role.split();
            foreach (var role in roles) {
                if(myapp.roles.IsInRole(usr,role)) {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}