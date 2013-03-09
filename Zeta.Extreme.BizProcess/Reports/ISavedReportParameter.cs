

using Qorpent.Model;

namespace Zeta.Extreme.BizProcess.Reports{
	/// <summary>
	/// Интерфейс сохраненного отчета
	/// </summary>
    public interface ISavedReportParameter :IWithId, IWithName, IWithVersion{
		/// <summary>
		/// Ссылка на отчет
		/// </summary>
        ISavedReport Report { get; set; }
		/// <summary>
		/// Значение
		/// </summary>
        string Value { get; set; }
    }
}