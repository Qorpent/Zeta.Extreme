#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : obj.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using Qorpent;

namespace Zeta.Extreme.Poco {
	public partial class obj : IZetaMainObject, IZetaQueryDimension {
		public obj() {
			Range = new DateRange(QorpentConst.Date.Begin, QorpentConst.Date.End);
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
			// Properties = new Dictionary<string, object>();
		}

		[Ref(ClassName = typeof (objdiv))] public virtual IMainObjectGroup Holding { get; set; }


		[Ref(ClassName = typeof (objrole))] public virtual IMainObjectRole Otrasl { get; set; }

		[Ref(ClassName = typeof (point))] public virtual IZetaPoint Municipal { get; set; }

		[Map] public virtual Guid Uid { get; set; }

		public virtual IList<IZetaDetailObject> links {
			get { return DetailObjects; }
		}

		public virtual object this[string idx] {
			get { return Properties.get(idx); }
			set { Properties[idx] = value; }
		}

		public virtual string OuterCode { get; set; }
		public virtual int? ParentId { get; set; }

		public virtual int? ZoneId { get; set; }

		public virtual int? RoleId { get; set; }

		public virtual int? TypeId { get; set; }

		public virtual IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		public virtual int? DivId { get; set; }
		[Map(ReadOnly = true)] public virtual string Path { get; set; }

		[Map] public virtual string Tag { get; set; }
		[Map] public virtual string Valuta { get; set; }

		[Map] public virtual string FullName { get; set; }
		[Map] public virtual string ShortName { get; set; }

		[NoMap] public virtual bool IsFormula {
			get { return !string.IsNullOrWhiteSpace(Formula); }
			set { _isFormula = value; }
		}

		[Map] public virtual string Formula { get; set; }

		[NoMap] public virtual string ParsedFormula { get; set; }
		[NoMap] public virtual string FormulaEvaluator { get; set; }

		[Map] public virtual bool ShowOnStartPage { get; set; }


		[Many(ClassName = typeof (detail))] public virtual IList<IZetaDetailObject> DetailObjects { get; set; }


		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }


		[Map(ReadOnly = true)] public virtual string GroupCache { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		[Many(ClassName = typeof (MainObjectMark))] public virtual IList<IZetaMainObjectMark> MarkLinks { get; set; }

		[Many(ClassName = typeof (cell))] public virtual IList<IZetaCell> Cells { get; set; }

		[Map] public virtual string Address { get; set; }

		public virtual IMainObjectGroup Group {
			get { return Holding; }
			set { Holding = value; }
		}

		public virtual IMainObjectRole Role {
			get { return Otrasl; }
			set { Otrasl = value; }
		}

		public virtual IZetaPoint Location {
			get { return Municipal; }
			set { Municipal = value; }
		}


		public virtual IEnumerable<IZetaMainObject> AllChildren(int level, string typefiler) {
			if (0 == level) {
				yield break;
			}
			foreach (var child in Children.OrderBy(x => x.Idx*100000 + x.Id)) {
				if (matchTypeFilter(child, typefiler)) {
					yield return child;
				}
				foreach (var nest in child.AllChildren(level - 1, typefiler)) {
					yield return nest;
				}
			}
		}


		public virtual int Level {
			get { return Path.Count(x => x == '/') - 2; }
		}

		public virtual IList<IUsrThemaMap> UsrThemaMaps { get; set; }

		public virtual string[] GetConfiguredThemaCodes() {
			return UsrThemaMaps.Select(x => x.ThemaCode).Distinct().ToArray();
		}

		public virtual IZetaUnderwriter[] GetConfiguredUsers() {
			return UsrThemaMaps.Select(x => x.Usr).Distinct().ToArray();
		}

		public virtual IUsrThemaMap GetUserMap(string themacode, bool plan) {
			return UsrThemaMaps.FirstOrDefault(x => x.ThemaCode == themacode && x.IsPlan == plan);
		}

		public virtual string[] GetConfiguredThemas(IZetaUnderwriter usr, bool plan) {
			return UsrThemaMaps.Where(x => x.Usr.Id == usr.Id && x.IsPlan == plan).Select(x => x.ThemaCode).Distinct().ToArray();
		}

		[Many(ClassName = typeof (usr))] public virtual IList<IZetaUnderwriter> Underwriters { get; set; }

		[Many(ClassName = typeof (fixrule))] public virtual IList<IFixRule> FixRules { get; set; }

		public virtual IList<IZetaDetailObject> AlternateDetailObjects { get; set; }

		//public virtual IList<IDocumentOfCorrections> Documents { get; set; }

		public virtual DateRange Range { get; set; }
		[Map] public virtual int Idx { get; set; }
		[Map] public virtual DateTime Start { get; set; }
		[Map] public virtual DateTime Finish { get; set; }

		public virtual IDetailObjectType ObjType { get; set; }

		IDetailObjectType IWithDetailObjectType.Type {
			get { return ObjType; }
			set { ObjType = value; }
		}

		public virtual IZetaMainObject Parent { get; set; }
		public virtual IList<IZetaMainObject> Children { get; set; }

		public virtual IEnumerable<IZetaMainObject> AllChildren() {
			return AllChildren(10, null);
		}

		public virtual IDictionary<string, object> Properties {
			get { return properties ?? (properties = new Dictionary<string, object>()); }
			protected set { properties = value; }
		}

		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
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

		public virtual IZetaMainObject[] GetChildren(string classcode, string typecode) {
			classcode = classcode ?? "";
			typecode = typecode ?? "";
			return myapp.storage.Get<IZetaMainObject>()
				.Query(
					"from ENTITY x where x.Parent = ? and (? = '' or ? = x.ObjType.Code ) and (? = '' or ? = x.ObjType.Class.Code) "
					, this, typecode, typecode, classcode, classcode).OrderBy(x => x.Idx).ToArray();
		}

		public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.noContent() && null != ObjType) {
				tag = ObjType.ResolveTag(name);
			}
			if (tag.noContent() && null != Parent) {
				tag = Parent.ResolveTag(name);
			}
			return tag ?? "";
		}

		public virtual IList<IZetaObjectGroup> GetGroups() {
			lock (this) {
				if (_groups == null) {
					var _storage = myapp.storage.Get<IZetaObjectGroup>();
					_groups = GroupCache.split(false, true, '/').Distinct().Select(_storage.Load).OrderBy(x => x.Id).ToList();
				}
				return _groups;
			}
		}

		private bool matchTypeFilter(IZetaMainObject child, string typefiler) {
			if (typefiler.noContent()) {
				return true;
			}
			if (null == child.ObjType) {
				return false;
			}
			var s = "/" + child.ObjType.Class.Code + "/" + child.ObjType.Code + "/";
			return Regex.IsMatch(s, typefiler);
		}

		private IList<IZetaObjectGroup> _groups;
		private bool _isFormula;
		private IDictionary<string, object> localProperties;
		private IDictionary<string, object> properties;
	}
}