#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormula.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model.ExtremeSupport;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ��������� �������
	/// </summary>
	public interface IFormula {
		/// <summary>
		/// 	����������� ������� �� ���������� ���������� ������
		/// </summary>
		/// <param name="query"> </param>
		void Init(Query query);

		/// <summary>
		/// 	������������� �������� ������������� �������
		/// </summary>
		/// <param name="request"> </param>
		void SetContext(FormulaRequest request);

		/// <summary>
		/// 	���������� � ���� ����������, ��������� ����� �������, �� ��� ���������� ��������
		/// </summary>
		/// <param name="query"> </param>
		void Playback(Query query);

		/// <summary>
		/// 	������� ���������� ����������
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	� �������� ����� ���������� ���������� ������� �� ������ ������ �����
		/// </remarks>
		QueryResult Eval();

		/// <summary>
		/// 	��������� ������� �������� ������� ����� �������������
		/// </summary>
		void CleanUp();
	}
}