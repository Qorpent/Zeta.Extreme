#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFixRuleCore.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IFixRuleCore {
		FixRulePriority Priority { get; set; }
		FixRuleResult Result { get; set; }
		int AdvancedWeight { get; set; }
		int GetSalience();
	}
}