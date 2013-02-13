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
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
    public class ThemaFactory : IThemaFactory{
        public readonly IList<IThema> Themas = new List<IThema>();

        private readonly IDictionary<string, object> cache = new Dictionary<string, object>();

 

        #region IThemaFactory Members

        public string SrcXml { get; set; }

        public IThema Get(string code) {
            return Themas.FirstOrDefault(x => x.Code == code);
        }

        public IDictionary<string, object> Cache{
            get { return cache; }
        }

    	public DateTime Version { get; set; }

    	public IEnumerable<IThema> GetAll(){
            return new List<IThema>(Themas);
        }

        public IEnumerable<IThema> GetForUser(){
            return GetForUser(myapp.usr);
        }

        public void CleanUser(string usrname) {
            lock (this) {
                if (cache.ContainsKey(usrname))
                {
                    cache.Remove(usrname);
                }    
            }
            
        }

        public IEnumerable<IThema> GetForUser(IPrincipal usr){
            return cache.get(usr.Identity.Name, () => internalGetForUser(usr));
        }


        public IInputTemplate GetForm(string code,bool throwerror = false){
            return cache.get(code + ".in", () =>{
                                               var thema =
                                                   Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
                                                       t => code.StartsWith(t.Code));
                                               if (null == thema){
                                                   return null;
                                               }
                                               var result= thema.GetForm(code);
				if(null==result && throwerror) {
					throw new Exception("cannto find form with code "+code);
				}
                                                	return result;
            }, true
                );
        }

        public IReportDefinition GetReport(string code){
            return cache.get(code + ".out", () =>{
                                                var thema =
                                                    Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
                                                        t => code.StartsWith(t.Code));
                                                if (null == thema){
                                                    return null;
                                                }
                                                return thema.GetReport(code);
                                            }, true);
        }

        #endregion

        private IEnumerable<IThema> internalGetForUser(IPrincipal usr){
            var personalized = Themas.Select(x => x.Personalize(usr)).ToArray();
            var active = personalized.Where(x => x.IsActive(usr)).BindParents().ToArray();

            active = active.OrderBy(x => x, new themaidxcomparer()).ToArray();
            return active;
        }

        #region Nested type: themaidxcomparer

        private class themaidxcomparer : IComparer<IThema>{
            #region IComparer<IThema> Members

            public int Compare(IThema x, IThema y){
                if (x.Parent.hasContent() && y.Parent.noContent()){
                    return -1;
                }
                if (y.Parent.hasContent() && x.Parent.noContent()){
                    return 1;
                }
                if (y.Parent.hasContent() && x.Parent.hasContent()){
                    return 0;
                }
                if (x.Parent.hasContent() && y.Code == x.Parent){
                    return 1;
                }
                if (y.Parent.hasContent() && x.Code == y.Parent){
                    return -1;
                }
                return x.Idx.CompareTo(y.Idx);
            }

            #endregion
        }

        #endregion

        public void Dispose(){
            foreach (var thema in Themas){
                ((Thema)thema).Reports.Clear();
                ((Thema)thema).Forms.Clear();
                ((Thema)thema).Documents.Clear();
                ((Thema)thema).Commands.Clear();
            }
            this.Themas.Clear();
        }
    }
}