#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IColumnHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	����������� ��������� ��������� "�������"
	/// </summary>
	public interface IColumnHandler : IQueryDimension<IZetaColumn> {
		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		IColumnHandler Copy();
	}
}