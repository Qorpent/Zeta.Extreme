#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Mark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model {
	public partial class Mark : IMark {
		[Map] public virtual Guid Uid { get; set; }
		[Map] public virtual int Idx { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }
	}
}