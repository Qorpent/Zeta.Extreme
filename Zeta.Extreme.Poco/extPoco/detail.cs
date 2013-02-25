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
using System.Linq;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class detail : IZetaDetailObject{
        public virtual string aCode{
            get { return (Object == null ? "NONE" : Object.Code) ?? "NONE"; }
        }
        [Map]
        public virtual bool InverseControl { get; set; }

        public virtual string aName{
            get { return (Object == null ? "NONE" : Object.Name) ?? "NONE"; }
        }

        public virtual string bCode{
            get { return (AltObject == null ? Code : AltObject.Code) ?? "NONE"; }
        }

        public virtual string bName{
            get { return (AltObject == null ? Name : AltObject.Name) ?? "NONE"; }
        }

        public virtual string bComment{
            get { return (AltObject == null ? Comment : AltObject.Comment) ?? ""; }
        }

        #region IZetaDetailObject Members

        public virtual string FullName { get; set; }

        public virtual IList<IMarkLinkBase> GetMarkLinks(){
            if (null == MarkLinks && 0 == Id){
                return new IMarkLinkBase[]{};
            }
            return MarkLinks.OfType<IMarkLinkBase>().ToList();
        }

        public virtual void RemoveMark(IMark mark){
            var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
            if (null != todel){
                MarkLinks.Remove(todel);
            }
        }

        public virtual bool IsMarkSeted(string code){
            return WithMarksExtension.IsMarkSeted(this, code);
        }

        #endregion

        public virtual bool Equals(IZetaDetailObject detail){
            if (detail == null){
                return false;
            }
            if (Id != detail.Id){
                return false;
            }
            if (!Equals(Name, detail.Name)){
                return false;
            }
            if (!Equals(Code, detail.Code)){
                return false;
            }

            return true;
        }

        public override bool Equals(object obj){
            if (ReferenceEquals(this, obj)){
                return true;
            }
            return Equals(obj as IZetaDetailObject);
        }

        public override int GetHashCode(){
            var result = Id;
            result = 29*result + (Code != null ? Code.GetHashCode() : 0);
            result = 29*result + (Name != null ? Name.GetHashCode() : 0);

            return result;
        }
    }
}