#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : RowHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Text.RegularExpressions;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на строку
	/// </summary>
	public sealed class RowHandler : CachedItemHandlerBase<IZetaRow> {
		/// <summary>
		/// 	Условие на объединение по дереву (динамический суммовой набор) - для оптимизированных сумм
		/// </summary>
		public RowTreeUsage TreeUsage {
			get { return _treeUsage; }
			set {
				if (value != _treeUsage) {
					_treeUsage = value;
					InvalidateCacheKey();
				}
			}
		}

		/// <summary>
		/// 	True если целевая строка - ссылка
		/// </summary>
		public bool IsRef {
			get { return null != Native && null != Native.RefTo; }
		}

		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			if (TreeUsage != RowTreeUsage.None && !string.IsNullOrWhiteSpace(Code)) {
				return "TREE:" + Code + "~" + TreeUsage;
			}
			return base.EvalCacheKey();
		}

		/// <summary>
		/// 	Простая копия условия на строку
		/// </summary>
		/// <returns> </returns>
		public RowHandler Copy() {
			return MemberwiseClone() as RowHandler;
		}

		/// <summary>
		/// 	Нормализует ссылки и параметры
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		public void Normalize(ZexSession session, IZetaColumn column) {
			if (IsStandaloneSingletonDefinition()) {
				//try load native
				Native = RowCache.get(0 == Id ? (object) Code : Id);
			}
			NormalizeReferencedRows(column);
		}

		private void NormalizeReferencedRows(IZetaColumn column) {
			var proceed = true;
			while (proceed) {
				ResolveHardLinks();
				Native = Native.ResolveExRef(column);
				proceed = ResolveSingleRowFormula();
			}
		}

		private bool ResolveSingleRowFormula() {
			if (IsFormula && (FormulaType == "boo" || FormulaType == "cs")) {
				var match = Regex.Match(Formula.Trim(), @"^\$([\w\d]+)\?$", RegexOptions.Compiled);
				if (match.Success) {
					var reference = RowCache.get(match.Groups[1].Value);
					Native = reference;
					return true;
				}
			}
			return false;
		}

		private void ResolveHardLinks() {
			while (IsRef) {
				var refrow = Native.RefTo;
				Native = refrow;
			}
		}

		private RowTreeUsage _treeUsage;
	}
}