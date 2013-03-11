#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjColQueryGeneratorStruct.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	���������� ����������� ��� �������� ������� ������� � �������� ��������� ������-�������
	/// </summary>
	internal struct ObjColQueryGeneratorStruct {
		/// <summary>
		/// 	Id �������
		/// </summary>
		public int c;

		/// <summary>
		/// 	��� ������
		/// </summary>
		public DetailMode m;

		/// <summary>
		/// 	Id �������
		/// </summary>
		public int o;

		/// <summary>
		/// 	��� �������
		/// </summary>
		public ObjType t;
	}
}