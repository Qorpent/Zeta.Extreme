#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormDataSynchronize.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	��������� ������������� � ��������� ������
	/// </summary>
	public interface IFormDataSynchronize {
		/// <summary>
		/// 	����� ��� �������� ��������� ������
		/// </summary>
		void WaitData();
	}
}