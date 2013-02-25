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
using Comdiv.Extensions;
using Comdiv.Olap.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class obj : IZetaMainObject, IEquatable<obj>{
        public virtual IMainObjectRole Type{
            get { return Otrasl; }
        }

        public virtual IList<IZetaDetailObject> InLinks{
            get { return AlternateDetailObjects; }
        }

        public virtual IList<IZetaDetailObject> OutLinks{
            get { return DetailObjects; }
        }

        #region IEquatable<Org> Members

        public virtual bool Equals(obj org){
            if (org == null){
                return false;
            }
            if (Id != org.Id){
                return false;
            }
            if (!Equals(Name, org.Name)){
                return false;
            }
            if (!Equals(Code, org.Code)){
                return false;
            }

            return true;
        }

        #endregion

        #region IZetaMainObject Members

        public virtual IList<IMarkLinkBase> GetMarkLinks(){
            if (null == MarkLinks && 0 == Id){
                return new IMarkLinkBase[]{};
            }
            if (null == MarkLinks){
                return new IMarkLinkBase[]{};
            }
            return MarkLinks.OfType<IMarkLinkBase>().ToList();
        }

        public virtual bool IsMarkSeted(string code){
            return WithMarksExtension.IsMarkSeted(this, code);
        }

        public virtual void RemoveMark(IMark mark){
            var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
            if (null != todel){
                MarkLinks.Remove(todel);
            }
        }

        #endregion

        public override bool Equals(object obj){
            if (ReferenceEquals(this, obj)){
                return true;
            }
            return Equals(obj as obj);
        }

        public override int GetHashCode(){
            var result = Id;
            result = 29*result + (Code != null ? Code.GetHashCode() : 0);
            result = 29*result + (Name != null ? Name.GetHashCode() : 0);

            return result;
        }

        public virtual detail[] FindOwnSubparts(){
            return DetailObjects.Cast<detail>().ToArray();
        }

        public virtual detail[] FindOwnSubparts(int year){
            return DetailObjects.Where(d => d.Range.IsInRange(new DateTime(year, 1, 1))).Cast<detail>().ToArray();
        }

        public virtual int CountOwnSubparts(){
            return DetailObjects.Count;
        }

	    public virtual bool IsMatchZoneAcronim(string s) {
		    s = s.ToUpper();
		    if(!s.Contains("_")) {
				if(s.like(@"^\d+$"))return s == this.Id.ToString();
			    return this.GroupCache.ToUpper().Contains("/" + s + "/");
		    }
			if(s.StartsWith("OBJ_")) return s.Substring(4) == this.Id.ToString();
			if(s.StartsWith("GRP_") || s.StartsWith("OG_")) {
				var grp = s.Split('_')[1];
				return this.GroupCache.ToUpper().Contains("/" + grp + "/");
			}
			if(s.StartsWith("DIV_")) {
				if(null==Group) return false;
				return Group.Code.ToUpper() == s.Substring(4);
			}
			if (s.StartsWith("OTR_"))
			{
				if (null == Role) return false;
				return Role.Code.ToUpper() == s.Substring(4);
			}
			return GroupCache.ToUpper().Contains("/"+s+"/");
	    }
    }
}