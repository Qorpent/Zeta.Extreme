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
using Comdiv.Extensions;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class obj : IZetaMainObject, IEquatable<obj> {
		public virtual IMainObjectRole Type {
			get { return Otrasl; }
		}

		public virtual IList<IZetaDetailObject> InLinks {
			get { return AlternateDetailObjects; }
		}

		public virtual IList<IZetaDetailObject> OutLinks {
			get { return DetailObjects; }
		}

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

		public virtual IList<IMarkLinkBase> GetMarkLinks() {
			if (null == MarkLinks && 0 == Id) {
				return new IMarkLinkBase[] {};
			}
			if (null == MarkLinks) {
				return new IMarkLinkBase[] {};
			}
			return MarkLinks.OfType<IMarkLinkBase>().ToList();
		}

		public virtual bool IsMarkSeted(string code) {
			return WithMarksExtension.IsMarkSeted(this, code);
		}

		public virtual void RemoveMark(IMark mark) {
			var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
			if (null != todel) {
				MarkLinks.Remove(todel);
			}
		}

		public virtual bool IsMatchZoneAcronim(string s) {
			s = s.ToUpper();
			if (!s.Contains("_")) {
				if (s.like(@"^\d+$")) {
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