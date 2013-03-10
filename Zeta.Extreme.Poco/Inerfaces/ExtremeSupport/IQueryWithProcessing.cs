#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryWithProcessing.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	��������� ������� � ���������� ���������
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		/// <summary>
		/// 	������������ ������� ���������� �������
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);
	}
}