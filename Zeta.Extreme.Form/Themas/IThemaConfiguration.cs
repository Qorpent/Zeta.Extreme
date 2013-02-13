#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IThemaConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Model.Interfaces;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	��������� ������������ ����
	/// </summary>
	public interface IThemaConfiguration : IWithCode, IWithName, IWithIdx {
		/// <summary>
		/// 	������� ���������� ������������
		/// </summary>
		bool Active { get; set; }

		/// <summary>
		/// 	���� �������� �� ���������
		/// </summary>
		string DefaultElementRole { get; set; }

		/// <summary>
		/// 	��������������� ������������
		/// </summary>
		IList<IThemaConfiguration> Imports { get; set; }

		/// <summary>
		/// 	������, ����������� ������������� ������������
		/// </summary>
		string Evidence { get; }

		/// <summary>
		/// 	����� ��������������� ����
		/// </summary>
		/// <returns> </returns>
		IThema Configure();

		/// <summary>
		/// 	����� ���������� ���������
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="def"> </param>
		/// <returns> </returns>
		TypedParameter ResolveParameter(string name, object def = null);
	}
}