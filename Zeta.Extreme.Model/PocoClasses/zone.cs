#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : zone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 
	/// </summary>
	public partial class Zone : IZetaZone {
		 public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		 public virtual IList<IZetaRegion> Regions { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }
	}
}