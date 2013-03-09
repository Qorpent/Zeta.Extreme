#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CommonLog.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public class CommonLog : ICommonLog {
		public virtual int Id { get; set; }
		public virtual string Code { get; set; }
		public virtual string Type { get; set; }
		public virtual string Usr { get; set; }
		public virtual DateTime Version { get; set; }
		public virtual string Message { get; set; }
	}
}