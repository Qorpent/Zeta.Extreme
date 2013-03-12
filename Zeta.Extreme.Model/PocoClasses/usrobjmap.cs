#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : usrobjmap.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class usrobjmap : IUserObjectMap {
		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Map] public virtual Guid Uid { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }


		[Map] public virtual string Domain { get; set; }

		[Map] public virtual string Role { get; set; }


		[Map] public virtual DateTime Version { get; set; }


		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		[Map] public virtual bool AllOrgs { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }
	}
}