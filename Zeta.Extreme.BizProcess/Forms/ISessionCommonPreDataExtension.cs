namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ќбщий интерфейс расширений дл€ формировани€ исходного набора строк формы, не прив€занного к определенной форме (общий контур)
	/// </summary>
	public interface ISessionCommonPreDataExtension
	{
		/// <summary>
		/// ¬озвращает рабочий набор строк дл€ формы
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		void Prepare(IFormSession session);
	}
}