#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ISerialSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ����� �������, ������������������ ������� ������� � ������
	/// 	��� ������� ������ ����� ��������� ���� �� ������
	/// </summary>
	public interface ISerialSession {
		/// <summary>
		/// 	����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		QueryResult Eval(ZexQuery query);

		/// <summary>
		/// 	����������� ����������, ���������������� ������ � ������, ��������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Task<QueryResult> EvalAsync(ZexQuery query);

		/// <summary>
		/// 	���������� ������ �� �������� ������
		/// </summary>
		/// <returns> </returns>
		ZexSession GetUnderlinedSession();
	}
}