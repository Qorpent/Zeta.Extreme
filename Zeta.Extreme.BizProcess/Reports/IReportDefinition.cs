using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.MVC;
using Comdiv.Reporting;
using Comdiv.Security;
using Comdiv.Xml;
using Zeta.Extreme.BizProcess.Reports;
using Zeta.Extreme.BizProcess.Themas;

namespace Comdiv.Reporting{
    /// <summary>
    /// Static report definition - contains parameters which are stable for several requests, in 
    /// RGP it means as DEFAULT behaviour, intended ConvertTo be storable
    /// ����������� ����������� ������, �������� ���������� ��������� �� ���������, � ��� ��������� ��� ���������
    /// �� ���������, ��������������, ��� ����� ��������� ���� ��������
    /// </summary>
    public interface IReportDefinition : IXmlReadable, IXmlWriteable,IWithName,IWithCode,IWithComment, IWithRole{
		/// <summary>
		/// ������ - ���������
		/// </summary>
        IList<IReportDefinition> Sources { get; }
        /// <summary>
        /// �������������� ���������
        /// </summary>
		string AdvancedParameters { get; set; }
		/// <summary>
		/// ����������� ���������
		/// </summary>
        Dictionary<string, object> Parameters { get; }
		/// <summary>
		/// ��������� ���������
		/// </summary>
        ParametersCollection TemplateParameters { get; }
		/// <summary>
		/// ���������
		/// </summary>
        string PageTitle { get; }
        /// <summary>
        /// ������ �� ����
        /// </summary>
        IThema Thema { get; set; }
		/// <summary>
		/// ����������
		/// </summary>
        IDictionary<string, string> Extensions { get; }
		/// <summary>
		/// ����� �������������
		/// </summary>
        bool PreviewMode { get; set; }
		/// <summary>
		/// ������� ������������
		/// </summary>
		/// <returns></returns>
		IReportDefinition Clone();
		/// <summary>
		/// ������� ���������� �� �����������
		/// </summary>
		/// <param name="principal"></param>
        void CleanupParameters(IPrincipal principal);
    }
}