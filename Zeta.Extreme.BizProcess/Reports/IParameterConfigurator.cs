namespace Zeta.Extreme.BizProcess.Reports {
	/// <summary>
	/// ������������ ���������
	/// </summary>
	public interface IParameterConfigurator
	{	
		/// <summary>
		/// ������� ������������
		/// </summary>
		/// <param name="parameter"></param>
		void Configure(Parameter parameter);
	}
}