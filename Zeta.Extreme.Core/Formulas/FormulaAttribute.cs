#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormulaAttribute.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ��� ������������� ���������������� ������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class FormulaAttribute : Attribute {
		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="version"> </param>
		public FormulaAttribute(string key, string version) {
			Key = key;
			Version = version;
		}

		/// <summary>
		/// 	���� ������� ��� ��������� ����������� � ���������
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// ������ � ������� �������
		/// </summary>
		public string Version { get; set; }
	}
}