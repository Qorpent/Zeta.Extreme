using Qorpent.Mvc;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.Actions
{
    /// <summary>
    /// Поиск по элементам subst
    /// </summary>
    [Action("zdev.elements", Role = "DEVELOPER", Arm = "dev")]
    public class GetElementsAction
    {
        protected override object MainProcess()
        {
            return Analyzer.GetElements();
        }  
    }
}