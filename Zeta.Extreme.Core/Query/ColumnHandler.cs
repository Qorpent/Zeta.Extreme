#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ColumnHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на колонку
	/// </summary>
	public sealed class ColumnHandler : CachedItemHandlerBase<IZetaColumn> {
		/// <summary>
		/// Простая копия условия на время
		/// </summary>
		/// <returns></returns>
		public ColumnHandler Copy()
		{
			return MemberwiseClone() as ColumnHandler;
		}

		/// <summary>
		/// Нормализует колонку до нормали
		/// </summary>
		/// <param name="session"></param>
		public void Normalize(ZexSession session) {
			if (IsStandaloneSingletonDefinition())
			{
				//try load native
				Native = ColumnCache.get(0 == Id ? (object)Code : Id);
			}
		}
	}
}