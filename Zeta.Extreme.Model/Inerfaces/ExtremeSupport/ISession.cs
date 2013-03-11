#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ISession.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	�����������, Zeta.Extrem cec���, ������� ���������
	/// </summary>
	public interface ISession {
		/// <summary>
		/// 	��������� ��� ��������� ������
		/// </summary>
		IMetaCache MetaCache { get; }

		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Get(string key, int timeout = -1);

		/// <summary>
		/// 	���������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		IQuery Register(IQuery query, string uid = null);

		/// <summary>
		/// 	����������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������, �� ����������� ������� ������������ ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		Task<IQuery> RegisterAsync(IQuery query, string uid = null);

		/// <summary>
		/// 	��������� ������������� � ������ �������� � ������
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);

		/// <summary>
		/// ������� ���������� �����, ��������� � ���������� �������
		/// </summary>
		/// <param name="timeout"></param>
		void WaitPrimarySource(int timeout = -1);
	}
}