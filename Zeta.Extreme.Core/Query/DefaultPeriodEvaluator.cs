using System;
using Comdiv.Zeta.Data.Minimal;

namespace Zeta.Extreme {
	/// <summary>
	/// ������� ������������ 
	/// </summary>
	public class DefaultPeriodEvaluator : IPeriodEvaluator {
		/// <summary>
		/// ���������� ��������� TimeHandler �� ����������������� ����� � ���������
		/// </summary>
		/// <param name="basePeriod"></param>
		/// <param name="period"></param>
		/// <param name="year"></param>
		/// <returns></returns>
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