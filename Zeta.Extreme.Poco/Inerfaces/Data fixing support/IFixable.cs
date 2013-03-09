#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFixable.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Comdiv.Zeta.Model {
	public interface IFixable {
		bool Finished { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.NoMap] FixRuleResult Fixed { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.NoMap] int FixRuleId { get; set; }
	}
}