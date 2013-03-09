#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objcls.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Persistence;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class objcls : IDetailObjectClass {
		[Deprecated.Map] public virtual Guid Uid { get; set; }


		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
			var query = new MetalinkRecord
			{
				Src = this.Code,
				SrcType = "zeta.objcls",
				TrgType = nodetype,
				Type = linktype,
				SubType = subtype,
				Active = true,
			};
			return new MetalinkRepository().Search(query, system);
			 */
		}

		[Deprecated.Map] public virtual string Tag { get; set; }

		[Deprecated.Many(ClassName = typeof (objtype))] public virtual IList<IDetailObjectType> Types { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual string ResolveTag(string name) {
			return TagHelper.Value(Tag, name);
		}
	}
}