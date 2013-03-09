#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithFormula.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Comdiv.Olap.Model {
	public interface IWithFormula {
		[Map] bool IsFormula { get; set; }
		[Map] string Formula { get; set; }

		string ParsedFormula { get; set; }
		[Map] string FormulaEvaluator { get; set; }
	}
}