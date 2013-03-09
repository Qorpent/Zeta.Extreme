namespace Zeta.Extreme.BizProcess.Reports {
	/// <summary>
	/// Конфигуратор параметра
	/// </summary>
	public interface IParameterConfigurator
	{	
		/// <summary>
		/// Команда конфигурации
		/// </summary>
		/// <param name="parameter"></param>
		void Configure(Parameter parameter);
	}
}