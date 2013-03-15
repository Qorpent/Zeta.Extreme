﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Mark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class Mark : IMark {
		 public virtual Guid Uid { get; set; }
		 public virtual int Idx { get; set; }

		 public virtual string Tag { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }
	}
}