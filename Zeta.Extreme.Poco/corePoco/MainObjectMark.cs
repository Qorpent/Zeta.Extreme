#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : MainObjectMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class MainObjectMark : IZetaMainObjectMark {
		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		[Deprecated.Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Target { get; set; }

		[Deprecated.Ref(ClassName = typeof (Mark))] public virtual IMark Mark { get; set; }
	}
}