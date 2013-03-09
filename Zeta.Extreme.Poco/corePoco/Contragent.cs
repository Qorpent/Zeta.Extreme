#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Contragent.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public class contragent : IContragent {
		public virtual string Tag { get; set; }
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Code { get; set; }
		public virtual string Comment { get; set; }
		public virtual DateTime Version { get; set; }
		public virtual string FullName { get; set; }
		public virtual string Type { get; set; }
		public virtual string Address { get; set; }
		public virtual string Contact { get; set; }
	}
}