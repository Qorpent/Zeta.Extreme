#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQuery.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
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
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <param name="deep"> ���� ��, �� ������ ����� ��������� ��������� </param>
		/// <returns> </returns>
		IQuery Copy(bool deep = false);
	}
}