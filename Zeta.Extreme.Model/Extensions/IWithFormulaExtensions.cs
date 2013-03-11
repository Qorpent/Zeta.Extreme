#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithFormulaExtensions.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

#if NEWMODEL
using Comdiv.Extensions;
using Comdiv.Olap.Model;

#endif

namespace Zeta.Extreme.Model.Extensions {
	/// <summary>
	/// 	¬спомогательный класс дл€ работы с формулами
	/// </summary>
	public static class IWithFormulaExtensions {
		/// <summary>
		/// 	Null-safe formula
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static string Formula(this IWithFormula formula) {
			if (null == formula) {
				return null;
			}
			return formula.Formula;
		}

		/// <summary>
		/// 	null-safe formula type
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static string FormulaEvaluator(this IWithFormula formula) {
			if (null == formula) {
				return null;
			}
			return formula.FormulaEvaluator;
		}

		/// <summary>
		/// 	null-safe parsed formula
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static string ParsedFormula(this IWithFormula formula) {
			if (null == formula) {
				return null;
			}
			return formula.ParsedFormula;
		}

		/// <summary>
		/// 	null-safe is formula
		/// </summary>
		/// <param name="formula"> </param>
		/// <returns> </returns>
		public static bool IsFormula(this IWithFormula formula) {
			if (null == formula) {
				return false;
			}
			return formula.IsFormula;
		}
	}
}