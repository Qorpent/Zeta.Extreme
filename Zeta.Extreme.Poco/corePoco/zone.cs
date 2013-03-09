#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : zone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class zone : IZetaZone {
		[Deprecated.Map] public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		[Deprecated.Many(ClassName = typeof (region))] public virtual IList<IZetaRegion> Regions { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }
	}
}