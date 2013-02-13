namespace Comdiv.Zeta.Web.InputTemplates {
    public interface ICustomFormSaver {
        string Save(InputTemplateRequest request, string xml);
    }
}