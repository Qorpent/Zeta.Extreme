#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQuery.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	��������� ��������� �������
	/// </summary>
	public interface IQuery : IWithCacheKey {
		/// <summary>
		/// 	������� �� �����
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		string Valuta { get; set; }

		/// <summary>
		/// ������� ������������ ��������
		/// </summary>
		QueryResult Result { get; set; }

		/// <summary>
		/// 	������������ ������� ���������� ������� � ���������
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);

		/// <summary>
		/// 	�������������� ��� �������, ������������� ��������
		/// </summary>
		long Uid { get; set; }

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		ISession Session { get; set; }

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <param name="deep"> ���� ��, �� ������ ����� ��������� ��������� </param>
		/// <returns> </returns>
		IQuery Copy(bool deep = false);

		/// <summary>
		/// 	����������� ��������� ������������
		/// </summary>
		void Normalize(ISession session = null);

	
	}
}