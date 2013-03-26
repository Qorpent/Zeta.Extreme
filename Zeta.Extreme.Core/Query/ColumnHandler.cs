#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/ColumnHandler.cs
#endregion
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

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