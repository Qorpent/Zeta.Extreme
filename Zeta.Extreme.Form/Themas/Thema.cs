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
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Reporting;
using Comdiv.Security;
using Comdiv.Security.Acl;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Report;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
    public class Thema : IThema{
        public readonly IDictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();
        public readonly IDictionary<string, IDocument> Documents = new Dictionary<string, IDocument>();
        public readonly IDictionary<string, IInputTemplate> Forms = new Dictionary<string, IInputTemplate>();
        public readonly IList<IThema> GroupMembers = new List<IThema>();
        public readonly IDictionary<string, IReportDefinition> Reports = new Dictionary<string, IReportDefinition>();
        private readonly IList<IThema> _children = new List<IThema>();
        private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();

        public Thema(){
            Visible = true;
        }

    	public int Period { get; set; }

    	public int Year { get; set; }

    	public IZetaMainObject Object { get; set; }
        public string Layout { get; set; }

        public ThemaConfiguration Configuration { get; set; }

        #region IThema Members

        public string Code { get; set; }

        public bool IsFavorite { get; set; }

        public IThema ParentThema { get; set; }

        public string Parent { get; set; }

        public bool IsTemplate { get; set; }

        public IEnumerable<IDocument> GetAllDocuments(){
            return Documents.Values;
        }

        public IEnumerable<ICommand> GetAllCommands(){
            return Commands.Values;
        }


        public virtual IThema Accomodate(IZetaMainObject obj, int year, int period) {
            return Accomodate(obj, year, period, null);
        }

        public virtual IThema Accomodate(IZetaMainObject obj, int year, int period,IDictionary<string ,object > statecache){
            var result = (Thema) Clone(true);
            result.Object = obj;
            result.Year = year;
            result.Period = period;


            IList<IInputTemplate> realadd = new List<IInputTemplate>();
            foreach (var f in result.Forms.Values){
                var t = f.PrepareForPeriod(year, period, DateExtensions.Begin, obj);
                if (!t.GetIsPeriodMatched()){
                    continue;
                }

				
                if (t.ForGroup.hasContent()){
					 if(null==obj) {
                        continue;
                    }
                    if(!GroupFilterHelper.IsMatch(obj,t.ForGroup)) {
                    	continue;
                    }
                }
                var state = t.GetState(obj, null ,statecache);
                t.IsOpen = state == "0ISOPEN";
                t.IsChecked = state == "0ISCHECKED";
                realadd.Add(t);
            }
            result.Forms.Clear();
            foreach (var list in realadd){
                result.Forms[list.Code] = list;
            }

            var reportstoremove =
                result.Reports.Values.OfType<ZetaReportDefinitionBase>().Where(x => !x.IsPeriodMatch(period, obj)).
                    Select(x => x.Code).ToArray();
            foreach (var s in reportstoremove){
                result.Reports.Remove(s);
            }

            var members = result.GroupMembers.ToArray().Select(x => x.Accomodate(obj, year, period, statecache));
            result.GroupMembers.Clear();
            foreach (var member in members){
                result.GroupMembers.Add(member);
            }


            return result;
        }


        public bool Visible { get; set; }

        public IThemaFactory Factory { get; set; }

        public IEnumerable<IInputTemplate> GetAllForms(){
            return Forms.Values;
        }

        public IEnumerable<IReportDefinition> GetAllReports(){
            return Reports.Values;
        }

        public IInputTemplate GetForm(string code){
            if (!code.EndsWith(".in")){
                code += ".in";
            }
            if (!code.StartsWith(Code)){
                code = Code + code;
            }
            if (!Forms.ContainsKey(code)){
                return null;
            }
        	var result = Forms[code];
        	result.Thema = this;
        	return result;
        }

        public IDocument GetDocument(string code){
            if (!code.EndsWith(".doc")){
                code += ".doc";
            }
            if (!code.StartsWith(Code)){
                code = Code + code;
            }
            if (!Documents.ContainsKey(code)){
                return null;
            }
            return Documents[code];
        }

        public ICommand GetCommand(string code){
            if (!code.EndsWith(".cmd")){
                code += ".cmd";
            }
            if (!code.StartsWith(Code)){
                code = Code + code;
            }
            if (!Commands.ContainsKey(code)){
                return null;
            }
            return Commands[code];
        }

        public bool IsGroup { get; set; }

        public IEnumerable<IThema> GetGroup(){
            if (!IsGroup){
                return new IThema[]{};
            }
            return GroupMembers.OrderBy(x => x.Idx).ToArray();
        }

        public string Group { get; set; }

        public bool IsActive(IPrincipal usr){
            if (Role.hasContent()){
                if (!myapp.roles.IsAdmin(usr)){
                    var hasrole = false;
                    foreach (var r in Role.split()){
                        if (myapp.roles.IsInRole(r)){
                            hasrole = true;
                            break;
                        }
                    }
                    if (!hasrole){
                        return false;
                    }
                }
            }
            if (IsGroup){
                return null != GroupMembers.FirstOrDefault(x => x.IsActive(usr));
            }
			if(null!=Error) return true; //up thema error

            return internalIsActive(usr);
        }


        public bool IsVisible(){
            return IsVisible(myapp.usr);
        }

        public bool IsVisible(IPrincipal usr){
            if (!IsActive(usr)){
                return false;
            }
            if (!Visible){
                return false;
            }
            if (!IsGroup){
                return true;
            }
            return null != GroupMembers.FirstOrDefault(x => x.Visible);
        }

        public IReportDefinition GetReport(string code){
            if (!code.EndsWith(".out")){
                code += ".out";
            }
            if (!code.StartsWith(Code)){
                code = Code + code;
            }
            if (!Reports.ContainsKey(code)){
                return null;
            }
            return Reports[code];
        }

        public string Name { get; set; }

        public string Role { get; set; }

        public IThema Personalize(IPrincipal usr){
            var result = Clone(false) as Thema;
            foreach (var form in result.Forms.Values.ToArray()){
                if (!acl.get(form, null, null, usr, false)){
                    result.Forms.Remove(form.Code);
                }
            }
            foreach (var report in result.Reports.Values.ToArray()){
                if (!acl.get(report, null, null, usr, false)){
                    result.Reports.Remove(report.Code);
                }
            }

            foreach (var report in result.Reports.Values.ToArray()){
                report.CleanupParameters(usr);
            }

            foreach (var doc in result.Documents.Values.ToArray()){
                if (!acl.get(doc, null, null, usr, false)){
                    result.Documents.Remove(doc.Code);
                }
            }

            foreach (var cmd in result.Commands.Values.ToArray()){
                if (!acl.get(cmd, null, null, usr, false)){
                    result.Commands.Remove(cmd.Code);
                }
            }


            var personalizedThemas = result.GroupMembers.Select(x => x.Personalize(usr)).ToArray();
            result.GroupMembers.Clear();
            foreach (var thema in personalizedThemas){
                if (thema.IsActive(usr)){
                    result.GroupMembers.Add(thema);
                }
            }

            return result;
        }

        public IDictionary<string, object> Parameters{
            get { return _parameters; }
        }

        public object GetParameter(string code){
			if (code == "this.code") return this.Code;
			if(code=="this.name") return this.Name;
			if (code == "this.idx") return this.Idx;
            return Parameters.get(code);
        }

        public T GetParameter<T>(string code, T def)
        {
            var p = GetParameter(code);
            if(p.no())
            {
                return def;
            }
            return p.to<T>();
        }

        public int Idx { get; set; }

        public IList<IThema> Children{
            get { return _children; }
        }

    	public Exception Error { get; set; }

    	#endregion

        public override string ToString(){
            return string.Format("{0} : {1}", Code, Name);
        }

        public string GetDocText(string code){
            if (!code.StartsWith(Code)){
                code = Code + code;
            }
            var doc = GetDocument(code);
            if (doc == null){
                return "";
            }
            if (doc.Type != "text"){
                return "";
            }
            return doc.Value;
        }

        protected virtual bool internalIsActive(IPrincipal principal){
            return true;
        }

        public IThema Clone(bool full){
			lock(this) {
				var result = GetType().create<Thema>();
				result.Code = Code;
				result.Name = Name;
				result.Role = Role;
				result.Layout = Layout;
				result.IsGroup = IsGroup;
				result.Group = Group;
				result.Factory = Factory;
				result.Visible = Visible;
				result.IsFavorite = IsFavorite;
				result.Parent = Parent;
				result.IsTemplate = IsTemplate;
				result.Idx = Idx;
				result.Error = Error;


				foreach (var parameter in Parameters.ToArray()) {
					result.Parameters[parameter.Key] = parameter.Value;
				}

				foreach (Thema thema in GroupMembers.ToArray()) {
					if (full) {
						result.GroupMembers.Add(thema.Clone(true));
					}
					else {
						result.GroupMembers.Add(thema.Clone(false));
					}
				}

				foreach (var form in Forms.ToArray()) {
					if (full) {
						result.Forms[form.Key] = form.Value.Clone();
					}
					else {
						result.Forms.Add(form);
					}
				}

				foreach (var report in Reports.ToArray()) {
					result.Reports.Add(report.Key, report.Value.Clone());
				}

				foreach (var cmd in Commands.ToArray()) {
					result.Commands.Add(cmd);
				}

				foreach (var doc in Documents.ToArray()) {
					result.Documents.Add(doc);
				}

				return result;
			}
        }
    }
}