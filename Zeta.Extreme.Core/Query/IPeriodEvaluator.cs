namespace Zeta.Extreme {
	/// <summary>
	/// ��������� ���������� ���������� ��������
	/// </summary>
	public interface IPeriodEvaluator {
		/// <summary>
		/// ���������� ��������� TimeHandler �� ����������������� ����� � ���������
		/// </summary>
		/// <param name="basePeriod"></param>
		/// <param name="period"></param>
		/// <param name="year"></param>
		/// <returns></returns>
		TimeHandler Evaluate(int basePeriod, int period,int year);
	}
}