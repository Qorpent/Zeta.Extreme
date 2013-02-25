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
using Comdiv.Inversion;
using Comdiv.MVC;
using Comdiv.Model;
using Comdiv.Persistence;
using Comdiv.Wiki;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
	public partial class row : IZetaRow,IDatabaseVerificator{

		[Map]
		public virtual bool Active { get; set; }

        private IDictionary<string, string> columnmap;
        private IDictionary<string, object> localProperties;
        public virtual int? ParentId { get; set; }
        public virtual int? RefId { get; set; }
        public virtual int? ObjectId { get; set; }
        public virtual void ApplyProperty(string property, object value, bool cascade = true) {
            this.LocalProperties[property] = value;
            if(cascade && null!=this.NativeChildren) {
                foreach (var c in this.NativeChildren) {
                    c.ApplyProperty(property, value, cascade);
                }
            }
        }

		public virtual object GetLocal(string name) {
			if (!LocalProperties.ContainsKey(name)) return "";
			return LocalProperties[name] ?? "";
		}

	    public virtual int Level {
			get {
                if (null == Path) {
                    if (null == this.Parent) return 0;
                    else return this.Parent.Level + 1;

                }
			    return Regex.Matches(Path, @"[^/]+").Count;
			}
	    }

	    public virtual bool HasHelp() {
            return myapp.ioc.get<IWikiRepository>().Exists("row/" + this.Code); 
        }

        [Map]
        public virtual string Grp { get; set; }

        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject Org { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        #region IZetaRow Members
        [Map(NoLazy=true)]
        public virtual string Tag { get; set; }

        [Map]
        public virtual string Lookup { get; set; }

		public virtual IZetaRow TemporalParent { get; set; }


        public virtual string FullRole {
            get {
                if (this.Role.hasContent()) return this.Role;
                if (this.Parent == null) return "";
                return Parent.FullRole;
            }
        }

        [Map]
        public virtual string Valuta { get; set; }

        [Map("IsDinamycLookUp")]
        public virtual bool IsDynamicLookup { get; set; }

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
        public virtual bool IsFormula { get; set; }

        [Map]
        public virtual string Formula { get; set; }

        [Map]
        public virtual string ParsedFormula { get; set; }

        [Map]
        public virtual string FormulaEvaluator { get; set; }

        [Map]
        public virtual string Measure { get; set; }

        [Map]
        public virtual bool IsDynamicMeasure { get; set; }


        [Ref(ClassName = typeof (IZetaRow))]
        public virtual IZetaRow Parent { get; set; }


        public virtual IList<IZetaRow> NativeChildren
        {
            get { return _children; }
        }
        private IList<IZetaRow> _children;

        [Many(ClassName = typeof (IZetaRow))]
        public virtual IList<IZetaRow> Children {
            get { return _children; }
            set { _children = value; }
        }

		public virtual string ResolveMeasure() {
			var mes = Measure;
			if(mes.noContent() && null!=RefTo) {
				mes = RefTo.Measure;
			}
			mes = mes ?? "";
			if(mes.Contains("dir")) {
				mes = myapp.storage.Get<IZetaRow>().Load(mes.Replace("/", "")).Measure;
			}
			return mes;
		}

        [Map]
        public virtual string Path { get; set; }

        [Ref(ClassName = typeof (IZetaRow))]
        public virtual IZetaRow RefTo { get; set; }



        public virtual int? ExRefToId { get; set; }

        [Ref(ClassName = typeof(IZetaRow))]
        public virtual IZetaRow ExRefTo { get; set; }

        [Map]
        public virtual int Idx { get; set; }

        [Map]
        public virtual string OuterCode { get; set; }

        public virtual string Group{
            get { return Grp; }
            set { Grp = value; }
        }

        [Many(ClassName = typeof (fixrule))]
        public virtual IList<IFixRule> FixRules { get; set; }

        [Many(ClassName = typeof (TreeMark))]
        public virtual IList<IZetaRowMark> MarkLinks { get; set; }

        [Many(ClassName = typeof (cell))]
        public virtual IList<IZetaCell> Cells { get; set; }

        public virtual IZetaMainObject Object{
            get { return Org; }
            set { Org = value; }
        }
        [Map]
        public virtual string MarkCache { get; set; }

        public virtual IDictionary<string, object> LocalProperties{
            get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
            set { localProperties = value; }
        }
        [Map]
        public virtual string ObjectGroups { get; set; }
        [Map]
        public virtual string FormElementType { get; set; }
        [Map]
        public virtual string Validator { get; set; }

        [Map]
        public virtual string ColumnSubstitution { get; set; }

        public virtual string ResolveColumnCode(string incode){
            prepareColumnMap();
            return columnmap.get(incode, incode);
        }


		public virtual string ResolveTag (string name) {
			if(TagHelper.Has(this.Tag,name)) {
				return TagHelper.Value(this.Tag, name)??"";
			}
			if(null!=TemporalParent) {
				return TemporalParent.ResolveTag(name);
			}
			if(null==this.Parent) {
				return "";
			}
			return Parent.ResolveTag(name);
		}


		public virtual void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null) {
			propname = propname ?? groupname;
			Func<IZetaRow, bool> test = r => r.Group.split(false, true, '/', ';').Any(x => x == groupname);
			ApplyPropertyByCondition(propname,true,applyUp,false,test);
		}

		public virtual void ApplyPropertyByCondition (string prop, object value, bool applyUp, bool applyDown, Func<IZetaRow,bool> test ) {
			foreach (var r in AllChildren) {
				if(r.LocalProperties.ContainsKey(prop) && Equals( r.LocalProperties[prop], value))continue;
				if(test(r)) {
					r.LocalProperties[prop] = value;
					if(applyUp) {
						var current = r;
						while (null!=(current = current.Parent)) {
							current.LocalProperties[prop] = value;
						}
					}
					if(applyDown) {
						foreach (var c in r.AllChildren) {
							c.LocalProperties[prop] = value;
						}
					}
				}
			}
		}

        public virtual void CleanupByChildren(IEnumerable<string> codes) {
            if (this.LocalProperties.ContainsKey("cleaned")) return;
        	var directcodes = codes.Where(x => !x.StartsWith("GR:")).ToArray();
        	var grpcodes = codes.Where(x => x.StartsWith("GR:")).Select(x => x.Substring(3)).ToArray();

        	bool result = false;
			
            if(grpcodes.Length != 0 ) {
				var groups = this.Group.split(false, true, '/', ';');
				if (grpcodes.Any(x => groups.Contains(x)))
				{
					result = true;
				}
				else if (null != this.AllChildren.FirstOrDefault(x => x.Group.split(false,true,';','/').Intersect(grpcodes).Count()!=0))
				{
					result = true;
				}
				
			}

			if (directcodes.Length != 0) {
				if (directcodes.Contains(this.Code)) {
					result = true;
				}
				else if (null != this.AllChildren.FirstOrDefault(x => directcodes.Contains(x.Code))) {
					result = true;
				}
				else {
					result = false;
				}

			}

			this.ApplyPropertyIfNew("cleaned",result);

        	foreach (var c in this.NativeChildren) {
                c.CleanupByChildren(codes);
            }
        }


		

        public virtual void ApplyPropertyIfNew(string property, object value, bool children = false) {
            if(!LocalProperties.ContainsKey(property)) {
                LocalProperties[property] = value;
            }
            if(children) {
                foreach (var nativeChild in NativeChildren) {
                    nativeChild.ApplyPropertyIfNew(property,value,children);
                }
            }
        }

        [Map]
        public virtual string FullName { get; set; }
        [Map]
        public virtual string Role { get; set; }

        #endregion

        public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default")
        {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
            var query = new MetalinkRecord
                            {
                                Src = Code,
                                SrcType = "zeta.row",
                                TrgType = nodetype,
                                Type = linktype,
                                SubType = subtype
                            };
            return new MetalinkRepository().Search(query, system);
			 */
        }

		public virtual void VerifySaving() {
			if(this.Parent!=null && this.Parent.Code==this.Code) {
				throw new ValidationException("Row "+this.Code+" cannot be saved - try to set parent to itself");
			}
		}

		public virtual bool IsActiveFor(IZetaMainObject obj) {
			var intag = this.ResolveTag("pr_stobj");
			if(intag.noContent()) intag = this.ResolveTag("viewforgroup");
			var extag = this.ResolveTag("pr_exobj");
			var ins = intag.ToUpper().split();
			var exs = extag.ToUpper().split();
			foreach (var ex in exs) {
				if(obj.IsMatchZoneAcronim(ex)) {
					return false;
				}
			}
			if(ins.Count>0) {
				foreach(var i in ins) {
					if(obj.IsMatchZoneAcronim(i)) return true;
				}
				return false;
			}

			if(null!=RefTo) {
				return RefTo.IsActiveFor(obj);
			}

			return true;
		}
	}
}