#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objcls.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class ObjCls : IDetailObjectClass {
		 public virtual Guid Uid { get; set; }


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

		 public virtual string Tag { get; set; }

		 public virtual IList<IObjectType> Types { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual string ResolveTag(string name) {
			return TagHelper.Value(Tag, name);
		}
	}
}