using System.Collections.Generic;
using System.Security.Principal;
using Qorpent.Model;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.BizProcess.Reports{
	/// <summary>
	/// Интерфейс сохраненного отчета
	/// </summary>
    public interface ISavedReport : IEntity, IWithUsr{
		/// <summary>
		/// Признак общего отчета
		/// </summary>
        bool Shared { get; set; }
		/// <summary>
		/// Код отчета
		/// </summary>
        string ReportCode { get; set; }
		/// <summary>
		/// Сохраненные параметры
		/// </summary>
        IList<ISavedReportParameter> Parameters { get; set; }
		/// <summary>
		/// Роль доступа к отчету
		/// </summary>
        string Role { get; set; }
		/// <summary>
		/// Авторизация отчета
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        bool Authorize(IPrincipal usr);
    }
}