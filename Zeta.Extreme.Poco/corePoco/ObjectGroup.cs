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
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme.Poco {
	public partial class ObjectGroup : IZetaObjectGroup {
		[Map] public virtual Guid Uid { get; set; }

		[Map] public virtual string Tag { get; set; }

		// [Many(ClassName = typeof (ObjectGroupLink))]

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual IList<IZetaMainObject> MainObjects {
			get {
				return _objcache ??
				       (_objcache =
				        new NativeZetaReader().ReadObjects("from ENTITY x where GroupCache like '%/'+" + Code + "+'/%'").OfType
					        <IZetaMainObject>().ToList()
				       )
					;
			}
			set {
				if (null == value) {
					_objcache = null;
				}
			}
		}

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }

		private IList<IZetaMainObject> _objcache;
	}
}