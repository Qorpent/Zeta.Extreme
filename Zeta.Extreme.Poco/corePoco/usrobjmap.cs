#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : usrobjmap.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class usrobjmap : IUserObjectMap {
		[Deprecated.Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Deprecated.Map] public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual string Tag { get; set; }

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }


		[Deprecated.Map] public virtual string Domain { get; set; }

		[Deprecated.Map] public virtual string Role { get; set; }


		[Deprecated.Map] public virtual DateTime Version { get; set; }


		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		[Deprecated.Map] public virtual bool AllOrgs { get; set; }
	}
}