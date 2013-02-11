#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
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
		public FormulaAttribute(string key) {
			Key = key;
		}

		/// <summary>
		/// 	���� ������� ��� ��������� ����������� � ���������
		/// </summary>
		public string Key { get; private set; }
	}
}