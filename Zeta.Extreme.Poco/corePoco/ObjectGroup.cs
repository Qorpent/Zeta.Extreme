#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjectGroup.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class ObjectGroup : IZetaObjectGroup {
		[Deprecated.Map] public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual string Tag { get; set; }

		// [Many(ClassName = typeof (ObjectGroupLink))]

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		public virtual IList<IZetaMainObject> MainObjects {
			get {
				return _objcache ??
				       (_objcache =
				        myapp.storage.Get<IZetaMainObject>().Query("from ENTITY x where GroupCache like '%/'+?+'/%'", Code).ToList()
				       )
					;
			}
			set {
				if (null == value) {
					_objcache = null;
				}
			}
		}

		private IList<IZetaMainObject> _objcache;
	}
}