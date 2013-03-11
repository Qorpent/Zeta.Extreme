#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPreloadProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	��������� ������, ����������� ������ �� ������� ������� �� ����� � ���������
	/// 	� �� ����������� ���� �������
	/// </summary>
	public interface IPreloadProcessor {
		/// <summary>
		/// 	��������� �������������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		IQuery Process(IQuery query);
	}
}