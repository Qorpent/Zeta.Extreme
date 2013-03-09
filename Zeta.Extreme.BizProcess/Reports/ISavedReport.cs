using System.Collections.Generic;
using System.Security.Principal;
using Comdiv.Model.Interfaces;
using Comdiv.Reporting;

namespace Zeta.Extreme.BizProcess.Reports{
	/// <summary>
	/// ��������� ������������ ������
	/// </summary>
    public interface ISavedReport : IEntityDataPattern, IWithUsr{
		/// <summary>
		/// ������� ������ ������
		/// </summary>
        bool Shared { get; set; }
		/// <summary>
		/// ��� ������
		/// </summary>
        string ReportCode { get; set; }
		/// <summary>
		/// ����������� ���������
		/// </summary>
        IList<ISavedReportParameter> Parameters { get; set; }
		/// <summary>
		/// ���� ������� � ������
		/// </summary>
        string Role { get; set; }
		/// <summary>
		/// ����������� ������
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        bool Authorize(IPrincipal usr);
    }
}