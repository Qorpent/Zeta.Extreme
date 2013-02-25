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
using System.Text.RegularExpressions;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Persistence;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class obj : IZetaMainObject,IZetaQueryDimension{
        private IDictionary<string, object> properties;

        public obj(){
            Range = new DateRange(DateExtensions.Begin, DateExtensions.End);
            Start = DateExtensions.Begin;
            Finish = DateExtensions.End;
            // Properties = new Dictionary<string, object>();
        }

		public virtual IDictionary<string, object> LocalProperties
		{
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

        [Ref(ClassName = typeof (objdiv))]
        public virtual IMainObjectGroup Holding { get; set; }


		
	    public virtual int? DivId { get; set; }

        [Ref(ClassName = typeof (objrole))]
        public virtual IMainObjectRole Otrasl { get; set; }

        [Ref(ClassName = typeof (point))]
        public virtual IZetaPoint Municipal { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        [Map(ReadOnly = true)]
        public virtual string Path { get; set; }

        public virtual IList<IZetaDetailObject> links{
            get { return DetailObjects; }
        }

        public virtual object this[string idx]{
            get { return Properties.get(idx); }
            set { Properties[idx] = value; }
        }

        #region IZetaMainObject Members
        [Map]
        public virtual string Tag { get; set; }
        [Map]
        public virtual string Valuta { get; set; }

        [Map]
        public virtual string FullName { get; set; }
        [Map]
        public virtual string ShortName { get; set; }

		[NoMap]
	    public virtual bool IsFormula {
			get { return !string.IsNullOrWhiteSpace(Formula); }
			set { _isFormula = value; }
		}

	    [Map]
        public virtual string Formula { get; set; }

		[NoMap]
	    public virtual string ParsedFormula { get; set; }
		[NoMap]
	    public virtual string FormulaEvaluator { get; set; }

	    [Map]
        public virtual bool ShowOnStartPage { get; set; }


		

        [Many(ClassName = typeof (detail))]
        public virtual IList<IZetaDetailObject> DetailObjects { get; set; }

      

	    [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual string Name { get; set; }


        [Map(ReadOnly = true)]
        public virtual string GroupCache { get; set; }

        [Map]
        public virtual string Code { get; set; }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        [Many(ClassName = typeof (MainObjectMark))]
        public virtual IList<IZetaMainObjectMark> MarkLinks { get; set; }

        [Many(ClassName = typeof (cell))]
        public virtual IList<IZetaCell> Cells { get; set; }

        [Map]
        public virtual string Address { get; set; }

        public virtual IMainObjectGroup Group{
            get { return Holding; }
            set { Holding = value; }
        }

        public virtual IMainObjectRole Role{
            get { return Otrasl; }
            set { Otrasl = value; }
        }

        public virtual IZetaPoint Location{
            get { return Municipal; }
            set { Municipal = value; }
        }


		public virtual IEnumerable<IZetaMainObject> AllChildren(int level, string typefiler) {
			if (0 == level) yield break;
			foreach (var child in Children.OrderBy(x=>x.Idx*100000+x.Id)) {
				if(matchTypeFilter(child,typefiler))yield return child;
				foreach (var nest in child.AllChildren(level-1,typefiler)) {
					yield return nest;
				}
			}
		}

    	private bool matchTypeFilter(IZetaMainObject child, string typefiler) {
    		if(typefiler.noContent()) return true;
			if (null == child.ObjType) return false;
    		var s = "/" + child.ObjType.Class.Code + "/" + child.ObjType.Code + "/";
    		return Regex.IsMatch(s, typefiler);
    	}


    	public virtual  int Level {
    		get { return Path.Count(x => x == '/') - 2; }
    	}

		public virtual IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		public virtual string[] GetConfiguredThemaCodes() {
			return UsrThemaMaps.Select(x => x.ThemaCode).Distinct().ToArray();
		}

		public virtual IZetaUnderwriter[] GetConfiguredUsers()
		{
			return UsrThemaMaps.Select(x => x.Usr).Distinct().ToArray();
		}

		public virtual IUsrThemaMap GetUserMap(string themacode, bool plan) {
			return UsrThemaMaps.FirstOrDefault(x => x.ThemaCode == themacode && x.IsPlan == plan);
		}
		public virtual string [] GetConfiguredThemas(IZetaUnderwriter usr, bool plan) {
			return UsrThemaMaps.Where(x => x.Usr.Id == usr.Id && x.IsPlan == plan).Select(x => x.ThemaCode).Distinct().ToArray();
		}

        [Many(ClassName = typeof (usr))]
        public virtual IList<IZetaUnderwriter> Underwriters { get; set; }

        [Many(ClassName = typeof (fixrule))]
        public virtual IList<IFixRule> FixRules { get; set; }

        public virtual IList<IZetaDetailObject> AlternateDetailObjects { get; set; }

        //public virtual IList<IDocumentOfCorrections> Documents { get; set; }

        public virtual DateRange Range { get; set; }
        [Map]
        public virtual int Idx { get; set; }
        [Map]
        public virtual DateTime Start { get; set; }
        [Map]
        public virtual DateTime Finish { get; set; }

		public virtual IDetailObjectType ObjType { get; set; }

		IDetailObjectType IWithDetailObjectType.Type { get { return ObjType; } set { ObjType = value; } }

		public virtual IZetaMainObject Parent { get; set; }
		public virtual IList<IZetaMainObject> Children { get; set; }

		public virtual IEnumerable<IZetaMainObject> AllChildren () {
			return AllChildren(10, null);
		}

	    public virtual string OuterCode { get; set; }
        public virtual IDictionary<string, object> Properties{
            get { return properties ?? (properties = new Dictionary<string, object>()); }
            protected set { properties = value; }
        }

	    public virtual int? ParentId { get; set; }

	    public virtual int? ZoneId { get; set; }

	    public virtual int? RoleId { get; set; }

	    public virtual int? TypeId { get; set; }

	    public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system="Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
            var query = new MetalinkRecord
                            {
                                Src = this.Id.ToString(),
                                SrcType = "zeta.obj",
                                TrgType = nodetype,
                                Type = linktype,
                                SubType = subtype,
                                Active = true,
                            };
            return new MetalinkRepository().Search(query, system);
			 */
        }

		public virtual IZetaDetailObject[] GetDetails(string classcode, string typecode) {
			classcode = classcode ?? "";
			typecode = typecode ?? "";
			return myapp.storage.Get<IZetaDetailObject>()
				.Query("from ENTITY x where x.Org = ? and (? = '' or ? = x.Type.Code ) and (? = '' or ? = x.Type.Class.Code) "
				       , this, typecode, typecode, classcode, classcode).OrderBy(x => x.Idx).ToArray();
		}

		public virtual IZetaMainObject[] GetChildren(string classcode, string typecode)
		{
			classcode = classcode ?? "";
			typecode = typecode ?? "";
			return myapp.storage.Get<IZetaMainObject>()
				.Query("from ENTITY x where x.Parent = ? and (? = '' or ? = x.ObjType.Code ) and (? = '' or ? = x.ObjType.Class.Code) "
					   , this, typecode, typecode, classcode, classcode).OrderBy(x => x.Idx).ToArray();
		}

        #endregion

	    public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.noContent() && null!=ObjType)
			{
				tag = ObjType.ResolveTag(name);
			}
			if (tag.noContent() && null != Parent)
			{
				tag = Parent.ResolveTag(name);
			}
		    return tag ?? "";
	    }

	    private IList<IZetaObjectGroup> _groups;
	    private IDictionary<string, object> localProperties;
	    private bool _isFormula;

	    public virtual IList<IZetaObjectGroup> GetGroups() {
			lock(this) {
				if(_groups==null) {
					var _storage = myapp.storage.Get<IZetaObjectGroup>();
					_groups = GroupCache.split(false, true, '/').Distinct().Select(_storage.Load).OrderBy(x=>x.Id).ToList();
				}
				return _groups;
			}
		}
    }
}