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
	/// 	Содержит различные константы для парсинга формул и псевдо-сумм
	/// </summary>
	public static class FormulaParserConstants {
		/// <summary>
		/// 	Таг, который может использоваться в строке или столбце для обозначения игнорируемых формул
		/// </summary>
		public const string IgnoreFormulaTag = "noextreme";

		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string RowPattern = @"\$(?<r>[\w\d]+)";

		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string IndirectSafeRowPattern = @"\$(?<r>[\p{Ll}\p{Lu}][\p{Ll}\p{Lu}\d][\w\d]*)";

		/// <summary>
		/// 	Паттерн смещения по колонке
		/// </summary>
		public const string ColPattern = @"\@(?<c>[\w\d]+)";

		/// <summary>
		/// 	Паттерн смещения по колонке
		/// </summary>
		public const string IndirectSafeColPattern = @"\@(?<c>[\p{Ll}\p{Lu}][\p{Ll}\p{Lu}\d][\w\d]*)";

		/// <summary>
		/// 	Паттерн смещения по объекту
		/// </summary>
		public const string ObjPattern = @"((\.toobj\(\s*(?<o>\d+)\s*\))?)"; //сразу настраивается как опицональный

		/// <summary>
		/// 	Паттерн фильтра по контрагентам
		/// </summary>
		public const string AltObjFilterPattern = @"((\.altobjfilter\(""(?<aof>[^""]*)""\))?)"; //сразу настраивается как опицональный

		/// <summary>
		/// 	Паттерн смещения по году
		/// </summary>
		public const string YearPattern = @"((\.Y(?<ys>-)?(?<y>\d+))?)";

		/// <summary>
		/// 	Паттерн по одному периоду
		/// </summary>
		public const string PeriodPattern = @"((\.P(?<ps>-)?(?<p>\d+))?)";

		/// <summary>
		/// 	Паттерн на несколько периодов
		/// </summary>
		public const string PeriodsPattern = @"((\.P\((?<pds>[\d,]+)\))?)";

		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string ColOrRowPattern = "((" + RowPattern + ")|(" + ColPattern + "))";

		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string ColOrRowOptionalPattern = "(" + ColOrRowPattern + "?" + ")";


		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string IndirectSafeColOrRowPattern = "((" + IndirectSafeRowPattern + ")|(" + IndirectSafeColPattern + "))";

		/// <summary>
		/// 	Паттерн смещения по строке
		/// </summary>
		public const string IndirectSafeColOrRowOptionalPattern = "(" + IndirectSafeColOrRowPattern + "?" + ")";


		/// <summary>
		/// 	Вариант между одним или несколькими периодами
		/// </summary>
		public const string PeriodOrPeriodsPattern = "(((" + PeriodPattern + ")|(" + PeriodsPattern + "))?)";

		/// <summary>
		/// 	Общий паттерн выражения дельты
		/// </summary>
		public const string DeltaPattern =
			ColOrRowPattern +
			ColOrRowOptionalPattern +
			ObjPattern +
			AltObjFilterPattern +
			YearPattern +
			PeriodOrPeriodsPattern;
			
		/// <summary>
		/// 	Общий паттерн выражения дельты (с защитой от параметрических переходов)
		/// </summary>
		public const string IndirectSafeDeltaPattern =
			IndirectSafeColOrRowPattern +
			IndirectSafeColOrRowOptionalPattern +
			ObjPattern +
			AltObjFilterPattern +
			YearPattern +
			PeriodOrPeriodsPattern;

		/// <summary>
		/// 	Шаблон вызова дельты
		/// </summary>
		public const string CallDeltaPattern = DeltaPattern + "\\?";
		/// <summary>
		/// 	Шаблон вызова дельты
		/// </summary>
		public const string IndirectSafeCallDeltaPattern = IndirectSafeDeltaPattern + "\\?";

		/// <summary>
		/// 	Регулярное выражение оценки и разбора суммовых формул
		/// </summary>
		public const string PseudoSumPattern =
			@"^\s*-?\s*" + IndirectSafeCallDeltaPattern + @"(((\s*[+-]\s*)|\s+)" + IndirectSafeCallDeltaPattern + @")*\s*$";
			
		


		/// <summary>
		/// 	Регулярное выражение для выборки отдельного элемента приводимой к сумме формулы
		/// </summary>
		public const string PseudoSumVector = @"(?<s>[-+])?\s*" + IndirectSafeCallDeltaPattern;
	}
}