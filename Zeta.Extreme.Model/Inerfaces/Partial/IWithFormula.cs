#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithFormula.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithFormula {
		[Map] bool IsFormula { get; set; }
		[Map] string Formula { get; set; }

		string ParsedFormula { get; set; }
		[Map] string FormulaEvaluator { get; set; }
	}
}