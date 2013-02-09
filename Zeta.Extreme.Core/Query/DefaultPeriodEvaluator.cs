#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultPeriodEvaluator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Data.Minimal;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовый определитель
	/// </summary>
	public class DefaultPeriodEvaluator : IPeriodEvaluator {
		/// <summary>
		/// 	Возвращает временный TimeHandler со скорректированным годом и периодами
		/// </summary>
		/// <param name="basePeriod"> </param>
		/// <param name="period"> </param>
		/// <param name="year"> </param>
		/// <returns> </returns>
		public TimeHandler Evaluate(int basePeriod, int period, int year) {
			var subresult = Periods.Eval(year, basePeriod, period);
			return new TimeHandler
				{
					Year = subresult.Year,
					Period = subresult.Periods[0],
					Periods = subresult.Periods.Length == 1 ? null : subresult.Periods
				};
		}
	}
}