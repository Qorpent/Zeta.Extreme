#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : UsrThemaMap.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	public class UserThemaMap : IUsrThemaMap {
		public virtual int Id { get; set; }
		public virtual IZetaUnderwriter Usr { get; set; }
		public virtual IZetaMainObject Object { get; set; }
		public virtual string System { get; set; }
		public virtual string Thema { get; set; }
		public virtual DateTime Version { get; set; }

		public virtual bool IsPlan {
			get { return Thema.EndsWith("_2"); }
		}

		public virtual string ThemaCode {
			get { return Thema.Replace("_2", ""); }
		}
	}
}