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
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model {
	public partial class ObjectGroup : IZetaObjectGroup {
		 public virtual Guid Uid { get; set; }

		 public virtual string Tag { get; set; }

		// 

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

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