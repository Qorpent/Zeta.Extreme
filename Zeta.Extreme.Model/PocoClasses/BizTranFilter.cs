#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Biztranfilter.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	/// <summary>
	/// 
	/// </summary>
	public partial class BizTranFilter : IBizTranFilter {
		public virtual int Id { get; set; }
		public virtual int Action { get; set; }
		public virtual int MainId { get; set; }
		public virtual int ContrId { get; set; }
		public virtual string TranCode { get; set; }
		public virtual string Role { get; set; }
		public virtual string Raw { get; set; }
		public virtual string FirstForm { get; set; }
		public virtual string FirstType { get; set; }
		public virtual string SecondType { get; set; }
		public virtual string SecondForm { get; set; }
	}
}