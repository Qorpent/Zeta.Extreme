#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactoryProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	������� ��������� ���������� ������� ���
	/// </summary>
	public interface IThemaFactoryProvider {
		/// <summary>
		/// 	�������� �������
		/// </summary>
		/// <returns> </returns>
		IThemaFactory Get();

		/// <summary>
		/// 	����������� �������
		/// </summary>
		void Reload();
	}
}