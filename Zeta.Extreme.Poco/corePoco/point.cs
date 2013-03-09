#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : point.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class point : IZetaPoint {
		[Deprecated.Map] public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		[Deprecated.Many(ClassName = typeof (obj))] public virtual IList<IZetaMainObject> MainObjects { get; set; }

		[Deprecated.Ref(ClassName = typeof (region))] public virtual IZetaRegion Region { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual IList<IZetaDetailObject> DetailObjects { get; set; }
	}
}