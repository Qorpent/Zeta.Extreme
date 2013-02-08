#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ColumnHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ������� �� �������
	/// </summary>
	public sealed class ColumnHandler : CachedItemHandlerBase<IZetaColumn> {
		/// <summary>
		/// ������� ����� ������� �� �����
		/// </summary>
		/// <returns></returns>
		public ColumnHandler Copy()
		{
			return MemberwiseClone() as ColumnHandler;
		}
	}
}