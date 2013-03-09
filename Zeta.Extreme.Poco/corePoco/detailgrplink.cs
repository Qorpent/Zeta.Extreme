#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : detailgrplink.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class detailgrplink : IZetaDetailObjectGroupLink {
		[Ref(ClassName = typeof (detail))] public virtual IZetaDetailObject Subpart { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual Guid Uid { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual IZetaDetailObject DetailObject {
			get { return Subpart; }
			set { Subpart = value; }
		}

		[Ref(ClassName = typeof (detailgrp))] public virtual IDetailObjectGroup<IZetaMainObject, IZetaDetailObject>
			DetailObjectGroup { get; set; }
	}
}