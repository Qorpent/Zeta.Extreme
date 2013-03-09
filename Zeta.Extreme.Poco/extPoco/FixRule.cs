#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FixRule.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class fixrule : IFixRule {
		public virtual int Salience { get; set; }

		public virtual int GetSalience() {
			return Salience;
		}
	}
}