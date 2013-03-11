#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ISerializableSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������� ������
	/// </summary>
	public interface ISerializableSession : ISession {
		/// <summary>
		/// 	���������� �������������
		/// </summary>
		object SerialSync { get; }

		/// <summary>
		/// 	������ ��� ���������� � ����������� ������ �� ���������������� �������
		/// </summary>
		Task<QueryResult> SerialTask { get; set; }
	}
}