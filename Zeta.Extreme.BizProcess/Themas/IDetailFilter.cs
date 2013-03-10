#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailFilter.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� �������� �������
	/// </summary>
	public interface IDetailFilter {
		/// <summary>
		/// 	��������������� ������� �����
		/// </summary>
		/// <param name="template"> </param>
		void Configure(IInputTemplate template);

		/// <summary>
		/// 	�������� ��������������� ������
		/// </summary>
		/// <param name="allObjects"> </param>
		/// <returns> </returns>
		IList<IZetaDetailObject> GetDetails(IEnumerable<IZetaDetailObject> allObjects);
	}
}