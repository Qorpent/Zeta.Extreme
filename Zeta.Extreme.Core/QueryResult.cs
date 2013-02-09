#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : QueryResult.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������ ����������
	/// </summary>
	public class QueryResult {
		/// <summary>
		/// 	��� ���� - ������������� ������
		/// </summary>
		public int CellId;

		/// <summary>
		/// 	������
		/// </summary>
		public Exception Error;

		/// <summary>
		/// 	������� �������������
		/// </summary>
		public bool IsComplete;

		/// <summary>
		/// 	������ ��������� �� ������ �� �������
		/// </summary>
		public bool IsNull;

		/// <summary>
		/// 	��������� ��������
		/// </summary>
		public decimal NumericResult;

		/// <summary>
		/// 	�������� ���������
		/// </summary>
		public string StringResult;
	}
}