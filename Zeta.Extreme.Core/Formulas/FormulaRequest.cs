#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaRequest.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������ �� ���������� �������
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// 	��� ������ - ����� �������������� ����������� ��� ����������� ����
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();

		/// <summary>
		/// 	������������ ������� �����
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// 	����� �������
		/// </summary>
		public string Formula;

		/// <summary>
		/// 	������� ����������� ������ ���������� �������
		/// </summary>
		public Task FormulaCompilationTask;

		/// <summary>
		/// 	���������� ����
		/// </summary>
		public string Key;

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string Language;

		/// <summary>
		/// 	�����
		/// </summary>
		public string Marks;

		/// <summary>
		/// 	������ ����������� ���� � ��������� ���������
		/// </summary>
		public Type PreparedType;

		/// <summary>
		/// 	�������, ��������������� � �������� C#
		/// </summary>
		public string PreprocessedFormula;

		/// <summary>
		/// 	����
		/// </summary>
		public string Tags;
	}
}