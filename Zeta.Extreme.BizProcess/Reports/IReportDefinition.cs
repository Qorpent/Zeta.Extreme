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
    /// Статическое определение отчета, содердит стабильные параметры по умлочанию, в ЛГО выступает как поведение
    /// по умолчанию, предполагается, что имеет тенденцию быть хранимым
    /// </summary>
    public interface IReportDefinition : IXmlReadable, IXmlWriteable,IWithName,IWithCode,IWithComment, IWithRole{
		/// <summary>
		/// Отчеты - исходники
		/// </summary>
        IList<IReportDefinition> Sources { get; }
        /// <summary>
        /// Дополнительные параметры
        /// </summary>
		string AdvancedParameters { get; set; }
		/// <summary>
		/// Вычисленные параметры
		/// </summary>
        Dictionary<string, object> Parameters { get; }
		/// <summary>
		/// Шаблонные параметры
		/// </summary>
        ParametersCollection TemplateParameters { get; }
		/// <summary>
		/// Заголовок
		/// </summary>
        string PageTitle { get; }
        /// <summary>
        /// Ссылка на тему
        /// </summary>
        IThema Thema { get; set; }
		/// <summary>
		/// Расширения
		/// </summary>
        IDictionary<string, string> Extensions { get; }
		/// <summary>
		/// Режим предпросмотра
		/// </summary>
        bool PreviewMode { get; set; }
		/// <summary>
		/// Команда клонирования
		/// </summary>
		/// <returns></returns>
		IReportDefinition Clone();
		/// <summary>
		/// Очистка параметров по полномочиям
		/// </summary>
		/// <param name="principal"></param>
        void CleanupParameters(IPrincipal principal);
    }
}