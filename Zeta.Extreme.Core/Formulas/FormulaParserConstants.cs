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
	/// 	Содержит различные константы для парсинга формул и псевдо-сумм
	/// </summary>
	public static class FormulaParserConstants {


		/// <summary>
		/// Таг, который может использоваться в строке или столбце для обозначения игнорируемых формул
		/// </summary>
		public const string IgnoreFormulaTag = "noextreme";

		/// <summary>
		/// Паттерн смещения по строке
		/// </summary>
		public const string RowPattern = @"\$(?<r>[\w\d]+)";

		/// <summary>
		/// Паттерн смещения по колонке
		/// </summary>
		public const string ColPattern = @"\@(?<c>[\w\d]+)";

		/// <summary>
		/// Паттерн смещения по объекту
		/// </summary>
		public const string ObjPattern = @"((\.toobj\(\s*(?<o>\d+)\s*\))?)"; //сразу настраивается как опицональный

		/// <summary>
		/// Паттерн смещения по году
		/// </summary>
		public const string YearPattern = @"((\.Y(?<ys>-)?(?<y>\d+))?)";

		/// <summary>
		/// Паттерн по одному периоду
		/// </summary>
		public const string PeriodPattern = @"((\.P(?<ps>-)?(?<p>\d+))?)";

		/// <summary>
		/// Паттерн на несколько периодов
		/// </summary>
		public const string PeriodsPattern = @"((\.P\((?<pds>[\d,]+)\))?)";

		/// <summary>
		/// Паттерн смещения по строке
		/// </summary>
		public const string ColOrRowPattern ="(("+RowPattern+")|("+ColPattern+"))";

		/// <summary>
		/// Паттерн смещения по строке
		/// </summary>
		public const string ColOrRowOptionalPattern = "("+ColOrRowPattern + "?"+")";

		/// <summary>
		/// Вариант между одним или несколькими периодами
		/// </summary>
		public const string PeriodOrPeriodsPattern = "(((" + PeriodPattern + ")|(" + PeriodsPattern + "))?)";

		/// <summary>
		/// Общий паттерн выражения дельты
		/// </summary>
		public const string DeltaPattern = 
				ColOrRowPattern+
				ColOrRowOptionalPattern+
				ObjPattern+
				YearPattern+
				PeriodOrPeriodsPattern;

		/// <summary>
		/// Шаблон вызова дельты
		/// </summary>
		public const string CallDeltaPattern = DeltaPattern + "\\?";
		/// <summary>
		/// 	Регулярное выражение оценки и разбора суммовых формул
		/// </summary>
		public const string PseudoSumPattern = @"^\s*-?\s*"+CallDeltaPattern+@"(((\s*[+-]\s*)|\s+)"+CallDeltaPattern+@")*\s*$";

		/// <summary>
		/// 	Регулярное выражение для выборки отдельного элемента приводимой к сумме формулы
		/// </summary>
		public const string PseudoSumVector = @"(?<s>[-+])?\s*"+CallDeltaPattern;

		
		
	}
}