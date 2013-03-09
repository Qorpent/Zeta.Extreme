#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithFormula.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithFormula {
		[global::Zeta.Extreme.Poco.Deprecated.Map] bool IsFormula { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string Formula { get; set; }

		string ParsedFormula { get; set; }
		[global::Zeta.Extreme.Poco.Deprecated.Map] string FormulaEvaluator { get; set; }
	}
}