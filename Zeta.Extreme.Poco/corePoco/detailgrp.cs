#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : detailgrp.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class detailgrp : IZetaDetailObjectGroup {
		[Deprecated.Map] public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		[Deprecated.Many(ClassName = typeof (detailgrplink))] public virtual
			IList<IDetailObjectGroupLink<IZetaMainObject, IZetaDetailObject>> DetailGroupLinks { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }
	}
}