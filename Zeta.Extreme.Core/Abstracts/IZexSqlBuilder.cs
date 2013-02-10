#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IZexSqlBuilder.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ����������� �������� �� ��������� ������
	/// </summary>
	public interface IZexSqlBuilder {
		/// <summary>
		/// 	��������� ������ ������ ��� �������
		/// </summary>
		/// <param name="query"> </param>
		void PrepareSqlRequest(ZexQuery query);
	}
}