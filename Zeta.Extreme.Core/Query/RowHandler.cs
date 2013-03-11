#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : RowHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Text.RegularExpressions;
using System.Threading;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на строку
	/// </summary>
	public sealed class RowHandler : CachedItemHandlerBase<IZetaRow>, IRowHandler {
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
		/// 	True если целевая строка - ссылка
		/// </summary>
		public bool IsSum {
			get { return null != Native && Native.IsMarkSeted("0SA"); }
		}

		/// <summary>
		/// 	Проверяем еще суммовые разделы
		/// </summary>
		/// <returns> </returns>
		public override bool IsPrimary() {
			if (!base.IsPrimary()) {
				return false;
			}
			return !IsSum;
		}

		/// <summary>
		/// 	Нормализует объект зоны
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public override void Normalize(ISession session) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 	Простая копия условия на строку
		/// </summary>
		/// <returns> </returns>
		public IRowHandler Copy() {
			return MemberwiseClone() as RowHandler;
		}

		/// <summary>
		/// 	Нормализует ссылки и параметры
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		public void Normalize(ISession session, IZetaColumn column) {
			var cache = session == null ? MetaCache.Default : session.GetMetaCache();
			if (IsStandaloneSingletonDefinition()) {
				//try load native
				Native = cache.Get<IZetaRow>(0 == Id ? (object) Code : Id);
			}
			NormalizeReferencedRows(session, column);
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


		private void NormalizeReferencedRows(ISession session, IZetaColumn column) {
			var initialcode = Code;
			var proceed = true;
			while (proceed) {
				ResolveHardLinks();
				Native = Native.ResolveExRef(column);
				proceed = ResolveSingleRowFormula();
			}
			if (initialcode != Code && session is Session) {
				if (((Session) session).CollectStatistics) {
					Interlocked.Increment(ref ((Session) session).Stat_Row_Redirections);
				}
			}
		}

		private bool ResolveSingleRowFormula() {
			if (IsFormula && (FormulaType == "boo" || FormulaType == "cs")) {
				var match = Regex.Match(Formula.Trim(), @"^\$([\w\d]+)\?$", RegexOptions.Compiled);
				if (match.Success) {
					var code = match.Groups[1].Value;
					var reference = RowCache.get(code);
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