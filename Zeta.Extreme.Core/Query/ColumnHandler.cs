#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ColumnHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на колонку
	/// </summary>
	public sealed class ColumnHandler : CachedItemHandlerBase<IZetaColumn>, IColumnHandler {
		private static readonly IDictionary<string, string> _resolvedColFormulaCache = new Dictionary<string, string>();

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		public IColumnHandler Copy() {
			return MemberwiseClone() as ColumnHandler;
		}

		/// <summary>
		/// 	Нормализует колонку до нормали
		/// </summary>
		/// <param name="session"> </param>
		public override void Normalize(ISession session) {
			var cache = session == null ? MetaCache.Default : session.GetMetaCache();
			if (IsStandaloneSingletonDefinition()) {
				//try load native
				Native = cache.Get<IZetaColumn>(0 == Id ? (object) Code : Id);
			}
			ResolveSingleColFormula();
		}

		private void ResolveSingleColFormula() {
			if (IsFormula && (FormulaType == "boo" || FormulaType == "cs")) {
				var formula = Formula;
				var code = GetCodeFormFormula(formula);
				if (null != code) {
					var reference = ColumnCache.get(code);
					Native = reference;
				}
			}
		}

		private static string GetCodeFormFormula(string formula) {
			if (!_resolvedColFormulaCache.ContainsKey(formula)) {
				string code = null;
				var match = Regex.Match(formula, @"^@([\w\d]+)\?$", RegexOptions.Compiled);
				if (match.Success) {
					code = match.Groups[1].Value;
				}
				_resolvedColFormulaCache[formula] = code;
				return code;
			}
			return _resolvedColFormulaCache[formula];
		}
	}
}