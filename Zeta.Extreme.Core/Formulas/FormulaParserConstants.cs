#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaParserConstants.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ��������� ��������� ��� �������� ������ � ������-����
	/// </summary>
	public static class FormulaParserConstants {
		/// <summary>
		/// 	���������� ��������� ������ � ������� �������� ������
		/// </summary>
		public const string PseudoSumPattern =
			@"^\s*(-?\s*\$[\w\d]+(@[\w\d]+)?(\.toobj\(\s*\d+\s*\))?(\.Y-?\d+)?(\.P-?\d+)?\?)(((\s*[+-]\s*)|\s+)\$[\w\d]+(@[\w\d]+)?(\.Y-?\d+)?(\.P-?\d+)?\?)*\s*$";

		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� ���������� � ����� �������
		/// </summary>
		public const string PseudoSumVector =
			@"(?<s>[-+])?\s*\$(?<r>[\w\d]+)(@(?<c>[\w\d]+))?(\.toobj\(\s*(?<o>\d+)\s*\))?(\.Y(?<ys>-)?(?<y>\d+))?(\.P(?<ps>-)?(?<p>\d+))?\?";

		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� �������
		/// </summary>
		public const string FormulaValueVector =
			@"\$(?<r>[\w\d]+)(@(?<c>[\w\d]+))?(\.toobj\(\s*(?<o>\d+)\s*\))?(\.Y(?<ys>-)?(?<y>\d+))?(\.P(?<ps>-)?(?<p>\d+))?\?";
		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� �������
		/// </summary>
		public const string FormulaValueVectorColStart =
			@"@(?<c>[\w\d]+)(\.toobj\(\s*(?<o>\d+)\s*\))?(\.Y(?<ys>-)?(?<y>\d+))?(\.P(?<ps>-)?(?<p>\d+))?\?";

		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� ������� (��� �������)
		/// </summary>
		public const string FormulaOnlyDeltaVector =
			@"\$(?<r>[\w\d]+)(@(?<c>[\w\d]+))?(\.toobj\(\s*(?<o>\d+)\s*\))?(\.Y(?<ys>-)?(?<y>\d+))?(\.P(?<ps>-)?(?<p>\d+))?";
	}
}