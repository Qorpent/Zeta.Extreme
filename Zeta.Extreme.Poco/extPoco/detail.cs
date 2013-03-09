#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : detail.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Model;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class detail : IZetaDetailObject {
		public virtual string aCode {
			get { return (Object == null ? "NONE" : Object.Code) ?? "NONE"; }
		}

		public virtual string aName {
			get { return (Object == null ? "NONE" : Object.Name) ?? "NONE"; }
		}

		public virtual string bCode {
			get { return (AltObject == null ? Code : AltObject.Code) ?? "NONE"; }
		}

		public virtual string bName {
			get { return (AltObject == null ? Name : AltObject.Name) ?? "NONE"; }
		}

		public virtual string bComment {
			get { return (AltObject == null ? Comment : AltObject.Comment) ?? ""; }
		}

		[Deprecated.Map] public virtual bool InverseControl { get; set; }

		public virtual string FullName { get; set; }

		public virtual IList<IMarkLinkBase> GetMarkLinks() {
			if (null == MarkLinks && 0 == Id) {
				return new IMarkLinkBase[] {};
			}
			return MarkLinks.OfType<IMarkLinkBase>().ToList();
		}

		public virtual void RemoveMark(IMark mark) {
			var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
			if (null != todel) {
				MarkLinks.Remove(todel);
			}
		}

		public virtual bool IsMarkSeted(string code) {
			return WithMarksExtension.IsMarkSeted(this, code);
		}

		public virtual bool Equals(IZetaDetailObject detail) {
			if (detail == null) {
				return false;
			}
			if (Id != detail.Id) {
				return false;
			}
			if (!Equals(Name, detail.Name)) {
				return false;
			}
			if (!Equals(Code, detail.Code)) {
				return false;
			}

			return true;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return Equals(obj as IZetaDetailObject);
		}

		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}
	}
}