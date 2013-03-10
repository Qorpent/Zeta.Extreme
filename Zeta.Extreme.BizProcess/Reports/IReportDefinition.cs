using System.Collections.Generic;
using System.Security.Principal;
using Qorpent.IO;
using Qorpent.Model;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.BizProcess.Reports{
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