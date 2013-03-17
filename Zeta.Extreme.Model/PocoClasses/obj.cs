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
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	public partial class Obj : IZetaMainObject {
		public Obj() {

			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
			// Properties = new Dictionary<string, object>();
		}

		 public virtual IMainObjectGroup Holding { get; set; }


		 public virtual IMainObjectRole Otrasl { get; set; }

		 public virtual IZetaPoint Municipal { get; set; }

		 public virtual Guid Uid { get; set; }

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
		 public virtual string Path { get; set; }

		 public virtual string Tag { get; set; }
		 public virtual string Valuta { get; set; }

		 public virtual string FullName { get; set; }
		 public virtual string ShortName { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		public virtual bool IsFormula {
			get { return !string.IsNullOrWhiteSpace(Formula); }
			set { _isFormula = value; }
		}

		 public virtual string Formula { get; set; }

		

		 public virtual bool ShowOnStartPage { get; set; }


		 public virtual IList<IZetaDetailObject> DetailObjects { get; set; }


		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }


		 public virtual string GroupCache { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		

		 public virtual string Address { get; set; }

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

		 public virtual IList<IZetaUnderwriter> Underwriters { get; set; }



		public virtual IList<IZetaDetailObject> AlternateDetailObjects { get; set; }

		//public virtual IList<IDocumentOfCorrections> Documents { get; set; }

		
		 public virtual int Idx { get; set; }
		 public virtual DateTime Start { get; set; }
		 public virtual DateTime Finish { get; set; }

		public virtual IObjectType ObjType { get; set; }

		IObjectType IWithDetailObjectType.Type {
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

		public virtual bool Equals(Obj org) {
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
			return Equals(obj as Obj);
		}

		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}

		public virtual Detail[] FindOwnSubparts() {
			return DetailObjects.Cast<Detail>().ToArray();
		}

	

		public virtual int CountOwnSubparts() {
			return DetailObjects.Count;
		}
	}
}