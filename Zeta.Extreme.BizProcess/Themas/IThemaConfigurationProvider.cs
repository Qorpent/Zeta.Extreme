#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaConfigurationProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	��������� ��������� ��� ������������ ����
	/// </summary>
	public interface IThemaConfigurationProvider {
		/// <summary>
		/// 	����� ��������� ����
		/// </summary>
		ThemaLoaderOptions Options { get; set; }

		/// <summary>
		/// 	�������� ������������
		/// </summary>
		/// <returns> </returns>
		IThemaFactoryConfiguration Get();

		/// <summary>
		/// 	���������� �������� ��������� ��� ���������� ����
		/// </summary>
		/// <param name="themacode"> </param>
		/// <param name="parameter"> </param>
		/// <param name="value"> </param>
		void Set(string themacode, string parameter, object value);
	}
}