namespace Comdiv.Zeta.Web.InputTemplates {
    /// <summary>
    /// Нестандартный класс сохранения данных формы
    /// </summary>
    public interface ICustomFormSaver {
        /// <summary>
        /// Сохраняет данные формы
        /// </summary>
        /// <param name="request"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        string Save(InputTemplateRequest request, string xml);
    }
}