namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Общий интерфейс расширений для формирования исходного набора строк формы
	/// </summary>
	public interface ISessionPreDataExtension
	{
		/// <summary>
		/// Возвращает рабочий набор строк для формы
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		void Prepare(IFormSession session);
	}
}