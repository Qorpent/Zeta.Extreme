#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactoryConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� ������������� ������� ���
	/// </summary>
	public interface IThemaFactoryConfiguration {
		/// <summary>
		/// 	������ ������������
		/// </summary>
		IList<IThemaConfiguration> Configurations { get; }

		/// <summary>
		/// 	�������� XML
		/// </summary>
		XElement[] SrcXml { get; set; }

		/// <summary>
		/// 	����� ������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		IThemaFactory Configure();
	}
}