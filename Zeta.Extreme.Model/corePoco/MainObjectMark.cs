#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : MainObjectMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class MainObjectMark : IZetaMainObjectMark {
		[Map] public virtual Guid Uid { get; set; }
		[Map] public virtual int Id { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Target { get; set; }

		[Ref(ClassName = typeof (Mark))] public virtual IMark Mark { get; set; }
	}
}