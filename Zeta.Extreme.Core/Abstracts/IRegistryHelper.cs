#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IRegistryHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������������� ��������� ������ ���
	/// 	����������� ������� � ������
	/// </summary>
	public interface IRegistryHelper {
		/// <summary>
		/// 	��������� ����������� �������
		/// 	���������� ������, � ����� ������������������ � �������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> </param>
		/// <returns> �������� ������ ����� ����������� </returns>
		Query Register(Query query, string uid);
	}
}