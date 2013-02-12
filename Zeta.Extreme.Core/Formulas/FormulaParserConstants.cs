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
		/// ���, ������� ����� �������������� � ������ ��� ������� ��� ����������� ������������ ������
		/// </summary>
		public const string IgnoreFormulaTag = "noextreme";

		/// <summary>
		/// ������� �������� �� ������
		/// </summary>
		public const string RowPattern = @"\$(?<r>[\w\d]+)";

		/// <summary>
		/// ������� �������� �� �������
		/// </summary>
		public const string ColPattern = @"\@(?<c>[\w\d]+)";

		/// <summary>
		/// ������� �������� �� �������
		/// </summary>
		public const string ObjPattern = @"((\.toobj\(\s*(?<o>\d+)\s*\))?)"; //����� ������������� ��� ������������

		/// <summary>
		/// ������� �������� �� ����
		/// </summary>
		public const string YearPattern = @"((\.Y(?<ys>-)?(?<y>\d+))?)";

		/// <summary>
		/// ������� �� ������ �������
		/// </summary>
		public const string PeriodPattern = @"((\.P(?<ps>-)?(?<p>\d+))?)";

		/// <summary>
		/// ������� �� ��������� ��������
		/// </summary>
		public const string PeriodsPattern = @"((\.P\((?<pds>[\d,]+)\))?)";

		/// <summary>
		/// ������� �������� �� ������
		/// </summary>
		public const string ColOrRowPattern ="(("+RowPattern+")|("+ColPattern+"))";

		/// <summary>
		/// ������� �������� �� ������
		/// </summary>
		public const string ColOrRowOptionalPattern = "("+ColOrRowPattern + "?"+")";

		/// <summary>
		/// ������� ����� ����� ��� ����������� ���������
		/// </summary>
		public const string PeriodOrPeriodsPattern = "(((" + PeriodPattern + ")|(" + PeriodsPattern + "))?)";

		/// <summary>
		/// ����� ������� ��������� ������
		/// </summary>
		public const string DeltaPattern = 
				ColOrRowPattern+
				ColOrRowOptionalPattern+
				ObjPattern+
				YearPattern+
				PeriodOrPeriodsPattern;

		/// <summary>
		/// ������ ������ ������
		/// </summary>
		public const string CallDeltaPattern = DeltaPattern + "\\?";
		/// <summary>
		/// 	���������� ��������� ������ � ������� �������� ������
		/// </summary>
		public const string PseudoSumPattern = @"^\s*-?\s*"+CallDeltaPattern+@"(((\s*[+-]\s*)|\s+)"+CallDeltaPattern+@")*\s*$";

		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� ���������� � ����� �������
		/// </summary>
		public const string PseudoSumVector = @"(?<s>[-+])?\s*"+CallDeltaPattern;

		
		
	}
}