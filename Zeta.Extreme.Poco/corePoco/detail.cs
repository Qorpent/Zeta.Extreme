#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : detail.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class detail : IZetaDetailObject {
		public detail() {
			Range = new DateRange(QorpentConst.Date.Begin, QorpentConst.Date.End);
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.Begin;
			Date1 = QorpentConst.Date.Begin;
			Date2 = QorpentConst.Date.Begin;
		}

		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject AltParent { get; set; }
		[Map] [IgnoreSerialize] public virtual Guid Uid { get; set; }
		[Many(ClassName = typeof (SubpartMark))] public virtual IList<IZetaDetailObjectMark> MarkLinks { get; set; }
		[Map(ReadOnly = true)] public virtual string Path { get; set; }
		[Map] public virtual string Valuta { get; set; }

		[Map] public virtual DateTime Start { get; set; }

		[Map] public virtual DateTime Finish { get; set; }

		[Map] public virtual decimal Number1 { get; set; }
		[Map] public virtual decimal Number2 { get; set; }


		[Map] public virtual DateTime Date1 { get; set; }

		[Map] public virtual DateTime Date2 { get; set; }

		[Map] public virtual string Tag { get; set; }
		public virtual int Idx { get; set; }

		[Serialize] public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		[Map] public virtual string OuterCode { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }


		[Serialize] public virtual IDetailObjectType Type { get; set; }

		[Serialize] public virtual IZetaMainObject AltObject {
			get { return AltParent; }
			set { AltParent = value; }
		}

		[Many(ClassName = typeof (cell))] public virtual IList<IZetaCell> Cells { get; set; }


		[Map] public virtual int IntOwn { get; set; }

		public virtual SubpartRelation Own {
			get { return (SubpartRelation) IntOwn; }
			set { IntOwn = (int) value; }
		}

		[Many(ClassName = typeof (fixrule))] public virtual IList<IFixRule> FixRules { get; set; }

		public virtual DateRange Range { get; set; }

		public virtual string Verb { get; set; }

		public virtual IZetaDetailObject Parent { get; set; }
		public virtual IZetaPoint Location { get; set; }

		public virtual IList<IZetaDetailObject> DetailObjects { get; set; }

		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
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

		public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.IsEmpty()) {
				tag = Type.ResolveTag(name);
			}
			if (tag.IsEmpty() && null != Parent) {
				tag = Parent.ResolveTag(name);
			}
			if (tag.IsEmpty()) {
				tag = Object.ResolveTag(name);
			}
			return tag ?? "";
		}
	}
}