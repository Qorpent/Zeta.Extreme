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
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using Qorpent.Serialization;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class detail : IZetaDetailObject{
        public detail(){
            Range = new DateRange(DateExtensions.Begin, DateExtensions.End);
        	Start = DateExtensions.Begin;
        	Finish = DateExtensions.Begin;
        	Date1 = DateExtensions.Begin;
        	Date2 = DateExtensions.Begin;
        }

        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject Org { get; set; }

        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject AltParent { get; set; }
        [Map(ReadOnly = true)]
        public virtual string Path { get; set; }
        [Map]
        [IgnoreSerialize]
        public virtual Guid Uid { get; set; }
        [Map]
        public virtual string Valuta { get; set; }

		[Map]
		public virtual DateTime Start { get; set; }

		[Map]
		public virtual DateTime Finish { get; set; }

        [Map]
        public virtual decimal Number1 { get; set; }
        [Map]
        public virtual decimal Number2 { get; set; }


		[Map]
		public virtual DateTime Date1 { get; set; }

		[Map]
		public virtual DateTime Date2 { get; set; }

        #region IZetaDetailObject Members
        [Map]
        public virtual string Tag { get; set; }
        public virtual int Idx { get; set; }
        [Serialize]
        public virtual IZetaMainObject Object{
            get { return Org; }
            set { Org = value; }
        }

        [Map]
        public virtual string OuterCode { get; set; }

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

        [Many(ClassName = typeof (detailgrplink))]
        public virtual IList<IDetailObjectGroupLink<IZetaMainObject, IZetaDetailObject>> DetailGroupLinks { get; set; }

        [Many(ClassName = typeof (SubpartMark))]
        public virtual IList<IZetaDetailObjectMark> MarkLinks { get; set; }

        [Serialize]
        public virtual IDetailObjectType Type { get; set; }

        [Serialize]
        public virtual IZetaMainObject AltObject{
            get { return AltParent; }
            set { AltParent = value; }
        }

        [Many(ClassName = typeof (cell))]
        public virtual IList<IZetaCell> Cells { get; set; }


        [Map]
        public virtual int IntOwn { get; set; }

        public virtual SubpartRelation Own{
            get { return (SubpartRelation) IntOwn; }
            set { IntOwn = (int) value; }
        }

        [Many(ClassName = typeof (fixrule))]
        public virtual IList<IFixRule> FixRules { get; set; }

        public virtual DateRange Range { get; set; }
        
        public virtual string Verb { get; set; }

        public virtual IZetaDetailObject Parent { get; set; }
        public virtual IZetaPoint Location { get; set; }

        public virtual IList<IZetaDetailObject> DetailObjects { get; set; }

        #endregion


        public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default")
        {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
            var query = new MetalinkRecord
            {
                Src = this.Id.ToString(),
                SrcType = "zeta.detail",
                TrgType = nodetype,
                Type = linktype,
                SubType = subtype,
                Active = true,
            };
            return new MetalinkRepository().Search(query, system);
			 */
        }

	    public virtual  string ResolveTag(string name) {
		    var tag = TagHelper.Value(this.Tag, name);
			if(tag.noContent()) {
				tag = this.Type.ResolveTag(name);
			}
			if(tag.noContent() && null!=Parent) {
				tag = this.Parent.ResolveTag(name);
			}
			if(tag.noContent()) {
				tag = this.Object.ResolveTag(name);
			}
		    return tag ?? "";
	    }
    }
}