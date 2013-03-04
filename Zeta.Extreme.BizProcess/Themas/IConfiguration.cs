#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� ������������ �������������� ���� (����������� ����)
	/// </summary>
	/// <typeparam name="T"> </typeparam>
	public interface IConfiguration<T> : IWithCode, IWithName {
		/// <summary>
		/// 	��� ���� (���)
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 	���� �������
		/// </summary>
		string Role { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		string Url { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		string Template { get; set; }

		/// <summary>
		/// 	������� ������� ������
		/// </summary>
		bool IsError { get; set; }

		/// <summary>
		/// 	������������ ����
		/// </summary>
		IThemaConfiguration Thema { get; set; }

		/// <summary>
		/// 	������� �� ������������ ������������
		/// </summary>
		/// <returns> </returns>
		T Configure();
	}
}