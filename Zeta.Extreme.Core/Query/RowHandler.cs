#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : RowHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
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
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			if (TreeUsage != RowTreeUsage.None && !string.IsNullOrWhiteSpace(Code)) {
				return "TREE:" + Code + "~" + TreeUsage;
			}
			return base.EvalCacheKey();
		}

		private RowTreeUsage _treeUsage;

		/// <summary>
		/// Простая копия условия на строку
		/// </summary>
		/// <returns></returns>
		public RowHandler Copy()
		{
			return MemberwiseClone() as RowHandler;
		}

		/// <summary>
		/// Нормализует ссылки и параметры
		/// </summary>
		/// <param name="session"></param>

		public void Normalize(ZexSession session)
		{
			if(IsStandaloneSingletonDefinition()) {
				//try load native
				Native = RowCache.get(0 == Id ? (object) Code : Id);
			}
		}
	}
}