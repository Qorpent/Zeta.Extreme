#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DetailRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public class detailrow : IDetailRow {
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual bool Active { get; set; }
		public virtual IZetaDetailObject Detail { get; set; }
		public virtual IZetaRow Row { get; set; }
		public virtual string Code { get; set; }
	}
}