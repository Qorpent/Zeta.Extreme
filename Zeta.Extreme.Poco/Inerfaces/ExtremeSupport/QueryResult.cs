#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryResult.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// 	������������ ����������
	/// </summary>
	public class QueryResult {
		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		public QueryResult() {}

		/// <summary>
		/// 	����������� �������� ���������� ����������
		/// </summary>
		/// <param name="result"> </param>
		public QueryResult(decimal result) {
			IsComplete = true;
			NumericResult = result;
		}

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