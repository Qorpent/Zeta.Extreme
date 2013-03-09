﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : TreeMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class TreeMark : IZetaRowMark {
		public virtual int MarkId { get; set; }
		public virtual int RowId { get; set; }
		public virtual string Code { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		[Deprecated.Ref(ClassName = typeof (IZetaRow))] public virtual IZetaRow Target { get; set; }

		[Deprecated.Ref(ClassName = typeof (Mark))] public virtual IMark Mark { get; set; }
	}
}