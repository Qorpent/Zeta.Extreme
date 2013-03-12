﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : point.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class point : IZetaPoint {
		[Map] public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		[Many(ClassName = typeof (obj))] public virtual IList<IZetaMainObject> MainObjects { get; set; }

		[Ref(ClassName = typeof (region))] public virtual IZetaRegion Region { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual IList<IZetaDetailObject> DetailObjects { get; set; }
	}
}