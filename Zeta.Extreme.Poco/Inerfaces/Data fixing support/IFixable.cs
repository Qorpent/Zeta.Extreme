#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFixable.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IFixable {
		bool Finished { get; set; }

		[NoMap] FixRuleResult Fixed { get; set; }

		[NoMap] int FixRuleId { get; set; }
	}
}