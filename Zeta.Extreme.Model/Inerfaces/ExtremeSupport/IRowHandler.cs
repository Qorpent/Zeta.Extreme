#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IRowHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	����������� ��������� ��������� Row
	/// </summary>
	public interface IRowHandler : IQueryDimension<IZetaRow> {
		/// <summary>
		/// 	True ���� ������� ������ - ������
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// 	True ���� ������� ������ - ������
		/// </summary>
		bool IsSum { get; }

		/// <summary>
		/// 	������� ����� ������� �� ������
		/// </summary>
		/// <returns> </returns>
		IRowHandler Copy();

		/// <summary>
		/// 	����������� ������ � ���������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		void Normalize(ISession session, IZetaColumn column);
	}
}