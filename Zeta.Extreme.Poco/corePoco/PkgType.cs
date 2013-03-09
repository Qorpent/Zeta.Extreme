#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PkgType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public class pkgtype : IPkgType {
		public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual string Tag { get; set; }
		public virtual string Code { get; set; }
		[Deprecated.Map] public virtual string Comment { get; set; }

		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual IList<IPkg> Pkgs { get; set; }

		public virtual DateTime Version { get; set; }

		[Deprecated.Map(Title = "Версионный")] public virtual bool Versioned { get; set; }

		[Deprecated.Map(Title = "Связанная форма")] public virtual string FormCode { get; set; }
	}
}