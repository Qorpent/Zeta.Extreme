#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objtype.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using TagHelper = Qorpent.Utils.Extensions.TagHelper;

namespace Zeta.Extreme.Poco {
	public partial class objtype : IDetailObjectType {
		[Map] public virtual Guid Uid { get; set; }


		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
			var query = new MetalinkRecord
			{
				Src = this.Code,
				SrcType = "zeta.objtype",
				TrgType = nodetype,
				Type = linktype,
				SubType = subtype,
				Active = true,
			};
			return new MetalinkRepository().Search(query, system);
			 */
		}

		[Map] public virtual string Tag { get; set; }

		[Many(ClassName = typeof (detail))] public virtual IList<IZetaDetailObject> DetailObjects { get; set; }


		public virtual IDetailObjectClass Class { get; set; }


		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.noContent()) {
				tag = Class.ResolveTag(name);
			}
			return tag ?? "";
		}
	}
}