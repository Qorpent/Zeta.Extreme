#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaParserConstants.cs
#endregion
namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ��������� ��������� ��� �������� ������ � ������-����
	/// </summary>
	public static class FormulaParserConstants {
		/// <summary>
		/// 	���, ������� ����� �������������� � ������ ��� ������� ��� ����������� ������������ ������
		/// </summary>
		public const string IgnoreFormulaTag = "noextreme";

		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string RowPattern = @"\$(?<r>[\w\d]+)";

		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string IndirectSafeRowPattern = @"\$(?<r>[\p{Ll}\p{Lu}][\p{Ll}\p{Lu}\d][\w\d]*)";

		/// <summary>
		/// 	������� �������� �� �������
		/// </summary>
		public const string ColPattern = @"\@(?<c>[\w\d]+)";

		/// <summary>
		/// 	������� �������� �� �������
		/// </summary>
		public const string IndirectSafeColPattern = @"\@(?<c>[\p{Ll}\p{Lu}][\p{Ll}\p{Lu}\d][\w\d]*)";

		/// <summary>
		/// 	������� �������� �� �������
		/// </summary>
		public const string ObjPattern = @"((\.toobj\(\s*(?<o>\d+)\s*\))?)"; //����� ������������� ��� ������������

		/// <summary>
		/// 	������� ������� �� ������������
		/// </summary>
		public const string AltObjFilterPattern = @"((\.altobjfilter\(""(?<aof>[^""]*)""\))?)"; //����� ������������� ��� ������������

		/// <summary>
		/// 	������� �������� �� ����
		/// </summary>
		public const string YearPattern = @"((\.Y(?<ys>-)?(?<y>\d+))?)";

		/// <summary>
		/// 	������� �� ������ �������
		/// </summary>
		public const string PeriodPattern = @"((\.P(?<ps>-)?(?<p>\d+))?)";

		/// <summary>
		/// 	������� �� ��������� ��������
		/// </summary>
		public const string PeriodsPattern = @"((\.P\((?<pds>[\d,]+)\))?)";

		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string ColOrRowPattern = "((" + RowPattern + ")|(" + ColPattern + "))";

		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string ColOrRowOptionalPattern = "(" + ColOrRowPattern + "?" + ")";


		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string IndirectSafeColOrRowPattern = "((" + IndirectSafeRowPattern + ")|(" + IndirectSafeColPattern + "))";

		/// <summary>
		/// 	������� �������� �� ������
		/// </summary>
		public const string IndirectSafeColOrRowOptionalPattern = "(" + IndirectSafeColOrRowPattern + "?" + ")";


		/// <summary>
		/// 	������� ����� ����� ��� ����������� ���������
		/// </summary>
		public const string PeriodOrPeriodsPattern = "(((" + PeriodPattern + ")|(" + PeriodsPattern + "))?)";

		/// <summary>
		/// 	����� ������� ��������� ������
		/// </summary>
		public const string DeltaPattern =
			ColOrRowPattern +
			ColOrRowOptionalPattern +
			ObjPattern +
			AltObjFilterPattern +
			YearPattern +
			PeriodOrPeriodsPattern;
			
		/// <summary>
		/// 	����� ������� ��������� ������ (� ������� �� ��������������� ���������)
		/// </summary>
		public const string IndirectSafeDeltaPattern =
			IndirectSafeColOrRowPattern +
			IndirectSafeColOrRowOptionalPattern +
			ObjPattern +
			AltObjFilterPattern +
			YearPattern +
			PeriodOrPeriodsPattern;

		/// <summary>
		/// 	������ ������ ������
		/// </summary>
		public const string CallDeltaPattern = DeltaPattern + "\\?";
		/// <summary>
		/// 	������ ������ ������
		/// </summary>
		public const string IndirectSafeCallDeltaPattern = IndirectSafeDeltaPattern + "\\?";

		/// <summary>
		/// 	���������� ��������� ������ � ������� �������� ������
		/// </summary>
		public const string PseudoSumPattern =
			@"^\s*-?\s*" + IndirectSafeCallDeltaPattern + @"(((\s*[+-]\s*)|\s+)" + IndirectSafeCallDeltaPattern + @")*\s*$";
			
		


		/// <summary>
		/// 	���������� ��������� ��� ������� ���������� �������� ���������� � ����� �������
		/// </summary>
		public const string PseudoSumVector = @"(?<s>[-+])?\s*" + IndirectSafeCallDeltaPattern;
	}
}