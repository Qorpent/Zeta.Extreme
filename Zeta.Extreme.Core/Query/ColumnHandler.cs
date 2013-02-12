#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ColumnHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Text.RegularExpressions;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на колонку
	/// </summary>
	public sealed class ColumnHandler : CachedItemHandlerBase<IZetaColumn> {
		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		public ColumnHandler Copy() {
			return MemberwiseClone() as ColumnHandler;
		}

		/// <summary>
		/// 	Нормализует колонку до нормали
		/// </summary>
		/// <param name="session"> </param>
		public void Normalize(Session session) {
			if (IsStandaloneSingletonDefinition()) {
				//try load native
				Native = ColumnCache.get(0 == Id ? (object) Code : Id);
			}
			ResolveSingleColFormula();
		}

		private void ResolveSingleColFormula() {
			if (IsFormula && (FormulaType == "boo" || FormulaType == "cs")) {
				var match = Regex.Match(Formula.Trim(), @"^@([\w\d]+)\?$", RegexOptions.Compiled);
				if (match.Success) {
					var reference = ColumnCache.get(match.Groups[1].Value);
					Native = reference;
				}
			}
		}
	}
}