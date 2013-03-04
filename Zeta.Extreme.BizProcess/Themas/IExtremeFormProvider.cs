#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IExtremeFormProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� ���������� ���� ��� Extreme
	/// </summary>
	public interface IExtremeFormProvider {
		/// <summary>
		/// 	�������������� ������������ �������
		/// </summary>
		void Reload(bool async = false);

		/// <summary>
		/// 	�������� ������ �� ����
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IInputTemplate Get(string code);
	}
}