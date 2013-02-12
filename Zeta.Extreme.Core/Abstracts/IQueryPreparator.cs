#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IZexQueryPreparator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ����������� ���� ���������� �������
	/// </summary>
	public interface IQueryPreparator {
		/// <summary>
		/// 	��������� ���������� ������� � ����������
		/// 	����������� ����� ������������� � ��������
		/// </summary>
		/// <param name="query"> </param>
		void Prepare(Query query);
	}
}