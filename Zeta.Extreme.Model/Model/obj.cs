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
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

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

		public virtual IMainObjectRole Type {
			get { return Otrasl; }
		}

		public virtual IList<IZetaDetailObject> InLinks {
			get { return AlternateDetailObjects; }
		}

		public virtual IList<IZetaDetailObject> OutLinks {
			get { return DetailObjects; }
		}

		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
		}

		public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.IsEmpty() && null != ObjType) {
				tag = ObjType.ResolveTag(name);
			}
			if (tag.IsEmpty() && null != Parent) {
				tag = Parent.ResolveTag(name);
			}
			return tag ?? "";
		}

		private bool matchTypeFilter(IZetaMainObject child, string typefiler) {
			if (typefiler.IsEmpty()) {
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

		public virtual bool Equals(obj org) {
			if (org == null) {
				return false;
			}
			if (Id != org.Id) {
				return false;
			}
			if (!Equals(Name, org.Name)) {
				return false;
			}
			if (!Equals(Code, org.Code)) {
				return false;
			}

			return true;
		}

		public virtual bool IsMatchZoneAcronim(string s) {
			s = s.ToUpper();
			if (!s.Contains("_")) {
				if (Regex.IsMatch(s, @"^\d+$")) {
					return s == Id.ToString();
				}
				return GroupCache.ToUpper().Contains("/" + s + "/");
			}
			if (s.StartsWith("OBJ_")) {
				return s.Substring(4) == Id.ToString();
			}
			if (s.StartsWith("GRP_") || s.StartsWith("OG_")) {
				var grp = s.Split('_')[1];
				return GroupCache.ToUpper().Contains("/" + grp + "/");
			}
			if (s.StartsWith("DIV_")) {
				if (null == Group) {
					return false;
				}
				return Group.Code.ToUpper() == s.Substring(4);
			}
			if (s.StartsWith("OTR_")) {
				if (null == Role) {
					return false;
				}
				return Role.Code.ToUpper() == s.Substring(4);
			}
			return GroupCache.ToUpper().Contains("/" + s + "/");
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return Equals(obj as obj);
		}

		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}

		public virtual detail[] FindOwnSubparts() {
			return DetailObjects.Cast<detail>().ToArray();
		}

		public virtual detail[] FindOwnSubparts(int year) {
			return DetailObjects.Where(d => d.Range.IsInRange(new DateTime(year, 1, 1))).Cast<detail>().ToArray();
		}

		public virtual int CountOwnSubparts() {
			return DetailObjects.Count;
		}
	}
}